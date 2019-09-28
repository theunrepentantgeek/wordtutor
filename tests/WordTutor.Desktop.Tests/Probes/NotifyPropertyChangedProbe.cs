using FluentAssertions;
using System.ComponentModel;

namespace WordTutor.Desktop.Tests.Probes
{
    public class NotifyPropertyChangedProbe
    {
        private readonly INotifyPropertyChanged _model;
        private PropertyChangedEventArgs? _args;

        public NotifyPropertyChangedProbe(INotifyPropertyChanged model)
        {
            _model = model;
            _model.PropertyChanged += (_, a) => _args = a;
        }

        public void AssertNotFired()
        {
            _args.Should().BeNull();
        }

        public void AssertFired(string propertyName)
        {
            _args.Should().NotBeNull(
                "we expect the event to have been fired.");
            _args!.PropertyName.Should().Be(
                propertyName,
                $"we expect the event to have been fired for '{propertyName}.");
        }
    }
}
