using System;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using WordTutor.Core.Logging;
using WordTutor.Core.Services;

namespace WordTutor.Desktop
{
    public sealed class SpeechService : ISpeechService
    {
        private readonly SoundPlayer _player = new SoundPlayer();
        private readonly IRenderSpeechService _renderSpeechService;
        private readonly ILogger _logger;

        public SpeechService(
            IRenderSpeechService renderSpeechService,
            ILogger logger)
        {
            _renderSpeechService = renderSpeechService ?? throw new ArgumentNullException(nameof(renderSpeechService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        ~SpeechService()
        {
            Dispose(false);
        }

        public async Task SayAsync(string content)
        {
            using var logger = _logger.Action($"Say {content}");
            try
            {
                var speech = await _renderSpeechService.RenderSpeechAsync(content).ConfigureAwait(false);

                _player.Stop();
                speech.Seek(0, SeekOrigin.Begin);
                _player.Stream = speech;
                _player.Play();
            }
            catch(RenderSpeechException ex)
            {
                logger.Failure(ex.Message);
            }
        }

        public void Dispose()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool includeManaged)
        {
            if (includeManaged)
            {
                _renderSpeechService.Dispose();
            }
        }
    }
}
