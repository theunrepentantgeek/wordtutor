using System.Windows.Input;

namespace WordTutor.Desktop
{
    public static class ItemCommands
    {
        public static RoutedCommand Close { get; }
            = new RoutedCommand("close", typeof(ItemCommands));

        public static RoutedCommand Delete { get; }
            = new RoutedCommand("delete", typeof(ItemCommands));

        public static RoutedCommand New { get; }
            = new RoutedCommand("new", typeof(ItemCommands));

        public static RoutedCommand Open { get; }
            = new RoutedCommand("open", typeof(ItemCommands));

        public static RoutedCommand Save { get; }
            = new RoutedCommand("save", typeof(ItemCommands));
    }
}
