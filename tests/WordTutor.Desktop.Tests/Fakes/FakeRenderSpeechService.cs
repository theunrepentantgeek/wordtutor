using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WordTutor.Core.Services;

namespace WordTutor.Desktop.Tests.Fakes
{
    public sealed class FakeRenderSpeechService : IRenderSpeechService
    {
        public Task<Stream> RenderSpeechAsync(string content)
        {
            var result = new MemoryStream();
            return Task.FromResult<Stream>(result);
        }

        public void Dispose()
        { }
    }
}
