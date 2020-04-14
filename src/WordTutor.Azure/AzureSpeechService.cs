using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
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

        private readonly SoundPlayer _player = new SoundPlayer();
        private readonly Dictionary<string, Stream> _cache 
            = new Dictionary<string, Stream>();

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

        public async Task SayAsync(string content)
        {
            var speech = await GetSpeechStream(content);

            _player.Stop();
            if (speech is Stream)
            {
                speech.Seek(0, SeekOrigin.Begin);
                _player.Stream = speech;
                _player.Play();
            }
        }

        private async Task<Stream> GetSpeechStream(string content)
        {
            if (_cache.TryGetValue(content, out var stream))
            {
                return stream;
            }

            stream = await RenderSpeech(content);
            if (stream is Stream)
            {
                // Successfully rendered, so store the result
                // (Don't want to cache failures)
                _cache[content] = stream;
            }

            return stream;
        }

        private async Task<Stream> RenderSpeech(string content)
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

            _logger.Info($"Failed to say '{content}'.");
            return null;
        }

        public void Dispose()
        {
            _player.Dispose();
        }
    }
}
