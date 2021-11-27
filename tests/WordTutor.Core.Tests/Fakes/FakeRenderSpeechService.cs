using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WordTutor.Core.Services;

namespace WordTutor.Core.Tests.Fakes
{
    public sealed class FakeRenderSpeechService : IRenderSpeechService
    {
        private readonly Dictionary<string, int> _callCounts = new Dictionary<string, int>();
        private readonly Dictionary<string, TaskCompletionSource<Stream>> _sources
            = new Dictionary<string, TaskCompletionSource<Stream>>();

        public async Task<Stream> RenderSpeechAsync(string content)
        {
            var currentCount = CallCountFor(content);
            _callCounts[content] = currentCount + 1;

            if (!_sources.TryGetValue(content, out var source))
            {
                source = new TaskCompletionSource<Stream>();
                _sources[content] = source;
            }

            return await source.Task.ConfigureAwait(false);
        }

        public int CallCountFor(string content)
            => _callCounts.TryGetValue(content, out var count) ? count : 0;

        public void ProvideResult(string content, Stream stream)
        {
            if (!_sources.TryGetValue(content, out var source))
            {
                source = new TaskCompletionSource<Stream>();
                _sources[content] = source;
            }

            source.SetResult(stream);
        }

        public void Dispose() => Dispose(true);

        [SuppressMessage(
            "Performance",
            "CA1822:Mark members as static",
            Justification = "Fake has nothing to dispose")]
        private void Dispose(bool _)
        {
        }
    }
}
