using System;
using System.Threading.Tasks;

namespace WordTutor.Core.Services
{
    public interface ISpeechService : IDisposable
    {
        Task SayAsync(string content);
    }
}
