namespace WordTutor.Core.Common
{
    public static class Validation
    {
        private static readonly ValidationResult _success = new SuccessResult();

        public static ValidationResult Success() => _success;

        public static ValidationResult Warning(
            string message, params ValidationMetadata[] metadata)
            => new WarningResult(message, metadata);

        public static ValidationResult WarningWhen(
            bool isWarning, string message, params ValidationMetadata[] metadata)
            => isWarning ? new WarningResult(message, metadata) : _success;

        public static ValidationResult Error(
            string message, params ValidationMetadata[] metadata)
            => new ErrorResult(message, metadata);

        public static ValidationResult ErrorWhen(
            bool isError, string message, params ValidationMetadata[] metadata)
            => isError ? new ErrorResult(message, metadata) : _success;
    }
}
