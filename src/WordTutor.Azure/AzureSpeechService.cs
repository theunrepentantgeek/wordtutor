using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using WordTutor.Core.Logging;
using WordTutor.Core.Services;

namespace WordTutor.Azure
{
    public class AzureSpeechService : ISpeechService
    {
        private readonly IConfigurationRoot _configurationRoot;
        private readonly ILogger _logger;
        private readonly SpeechConfig _configuration;

        public AzureSpeechService(IConfigurationRoot configuration, ILogger logger)
        {
            _configurationRoot = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var apiKey = _configurationRoot["WordTutor:SpeechApiKey"];
            var apiRegion = _configurationRoot["WordTutor:SpeechApiRegion"];

            _logger.Debug($"ApiKey: {apiKey}");
            _logger.Debug($"ApiRegion: {apiRegion}");

            _configuration = SpeechConfig.FromSubscription(apiKey, apiRegion);
        }

        public async Task SayAsync(string content)
        {
            _logger.Info($"Saying '{content}'.");
            using (var synthesizer = new SpeechSynthesizer(_configuration))
            {
                using (var result = await synthesizer.SpeakTextAsync(content))
                {
                    if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                    {
                        _logger.Info($"Completed saying '{content}'.");
                    }
                    else
                    {
                        _logger.Info($"Failed to say '{content}'.");
                    }
                }
            }
        }

        public void Dispose()
        {
            // nothing
        }
    }
}
