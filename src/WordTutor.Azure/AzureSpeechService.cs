using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using WordTutor.Core.Logging;
using WordTutor.Core.Services;

namespace WordTutor.Azure
{
    public sealed class AzureSpeechService : IRenderSpeechService
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
            _configuration.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Riff16Khz16BitMonoPcm);
        }

        public async Task<Stream> RenderSpeechAsync(string content)
        {
            var audioStream = AudioOutputStream.CreatePullStream();
            var audioConfig = AudioConfig.FromStreamOutput(audioStream);

            using var _synthesizer = new SpeechSynthesizer(_configuration, audioConfig);
            using var result = await _synthesizer.SpeakTextAsync(content);

            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                var stream = new MemoryStream();
                stream.Write(result.AudioData, 0, result.AudioData.Length);
                return stream;
            }

            var details = SpeechSynthesisCancellationDetails.FromResult(result);
            throw new RenderSpeechException(details.ErrorDetails);
        }

        public void Dispose()
        {
            // Nothing to clean up
        }
    }
}
