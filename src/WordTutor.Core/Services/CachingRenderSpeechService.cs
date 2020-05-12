using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WordTutor.Core.Services
{
    public sealed class CachingRenderSpeechService : IRenderSpeechService
    {
        private readonly IRenderSpeechService _renderSpeechService;

        // Cache of completed speech rendering
        private Dictionary<string, Stream> _completedRenders 
            = new Dictionary<string, Stream>();

        // Details of speech rendering that is still under way
        private Dictionary<string, TaskCompletionSource<Stream>> _pendingRenders 
            = new Dictionary<string, TaskCompletionSource<Stream>>();

        private readonly object _padlock = new object();

        public CachingRenderSpeechService(IRenderSpeechService renderSpeechService)
        {
            _renderSpeechService = renderSpeechService ?? throw new ArgumentNullException(nameof(renderSpeechService));
        }

        /// <summary>
        /// Renders specified content into a stream of audio data ready for playback
        /// </summary>
        /// <param name="content">Speech to render into audio</param>
        /// <returns>A stream containing the audio data</returns>
        public async Task<Stream> RenderSpeechAsync(string content)
        {
             bool haveSource;
            TaskCompletionSource<Stream>? tcsStream;

            lock(_padlock)
            {
                if (_completedRenders.TryGetValue(content, out var existingStream))
                {
                    return existingStream;
                }

                haveSource = _pendingRenders.TryGetValue(content, out tcsStream);
            }

            if (haveSource)
            {
                return await tcsStream!.Task.ConfigureAwait(false);
            }

            tcsStream = new TaskCompletionSource<Stream>();
            lock(_padlock)
            {
                _pendingRenders[content] = tcsStream;
            }

            var stream = await _renderSpeechService.RenderSpeechAsync(content).ConfigureAwait(false);
            lock(_padlock)
            {
                _completedRenders[content] = stream;
                _pendingRenders.Remove(content);
            }

            tcsStream.SetResult(stream);

            return stream;
        }

        public void Dispose()
            => Dispose(true);

        private void Dispose(bool includeManaged)
        {
            if (includeManaged)
            {
                _renderSpeechService.Dispose();
            }
        }
    }
}
