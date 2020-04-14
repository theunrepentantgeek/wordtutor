using System.Windows.Input;

namespace WordTutor.Desktop
{
    public static class VoiceCommands
    {
        public static RoutedCommand StartSpeaking { get; }
            = new RoutedCommand("speak", typeof(VoiceCommands));
    }
}
