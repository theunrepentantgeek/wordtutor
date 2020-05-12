using System;
using System.IO;
using System.Threading.Tasks;

namespace WordTutor.Core.Services
{
    public interface IRenderSpeechService : IDisposable
    {
        /// <summary>
        /// Renders specified content into a stream of audio data ready for playback
        /// </summary>
        /// <param name="content">Speech to render into audio</param>
        /// <returns>A stream containing the audio data</returns>
        Task<Stream> RenderSpeechAsync(string content);
    }

    /// <summary>
    /// Custom exception type for when rendering of speech fails
    /// </summary>
    public class RenderSpeechException : Exception
    {
        public RenderSpeechException()
            : base()
        { }

        public RenderSpeechException(string message)
            : base(message)
        { }

        public RenderSpeechException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
