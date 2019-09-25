using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WordTutor.Desktop
{
    public static class ItemCommands
    {
        public static RoutedCommand Delete { get; }
            = new RoutedCommand("delete", typeof(ItemCommands));

        public static RoutedCommand New { get; }
            = new RoutedCommand("new", typeof(ItemCommands));

        public static RoutedCommand Save { get; }
            = new RoutedCommand("save", typeof(ItemCommands));
    }
}
