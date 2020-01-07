using WordTutor.Core.Logging;
using WordTutor.Core.Redux;

namespace WordTutor.Core.Reducers
{
public class LoggingReducer : IReduxReducer<WordTutorApplication>
{
    // Reference to our logger
    private readonly ILogger _logger;

    public LoggingReducer(ILogger logger)
    {
        _logger = logger;
    }

    public WordTutorApplication Reduce(IReduxMessage message, WordTutorApplication currentState)
    {
        _logger.Info($"Message: {message}");
        return currentState;
    }
}
}
