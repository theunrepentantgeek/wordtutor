using System;
using FluentAssertions;
using WordTutor.Desktop.Tests.Fakes;
using Xunit;

namespace WordTutor.Desktop.Tests
{
    public class ViewModelBaseTests
    {
        public class IntegerPropertyTests : ViewModelBaseTests
        {
            private readonly IntViewModel _intModel = new IntViewModel(42);

            [Fact]
            public void WhenPropertyChanged_NewValueIsStored()
            {
                _intModel.Count = 100;
                _intModel.Count.Should().Be(100);
            }

            [Fact]
            public void WhenPropertyChanged_ModelIsModified()
            {
                _intModel.Count = 100;
                _intModel.Modified.Should().BeTrue();
            }

            [Fact]
            public void WhenPropertyNotChanged_ModelIsNotModified()
            {
                _intModel.Count = 42;
                _intModel.Modified.Should().BeFalse();
            }

            [Fact]
            public void WhenPropertyChangedWithSubscriber_SubscriberIsNotified()
            {
                bool notified = false;
                _intModel.PropertyChanged += (s, a) => notified = true;
                _intModel.Count = 100;
                notified.Should().BeTrue();
            }

            [Fact]
            public void WhenPropertyChangedWithSubscriber_SubscriberIsNotifiedNameOfProperty()
            {
                var property = "";
                _intModel.PropertyChanged += (s, a) => property = a.PropertyName;
                _intModel.Count = 100;
                property.Should().Be("Count");
            }

            [Fact]
            public void WhenPropertyNotChangedWithSubscriber_SubscriberIsNotNotified()
            {
                bool notified = false;
                _intModel.PropertyChanged += (s, a) => notified = true;
                _intModel.Count = 42; // Same value
                notified.Should().BeFalse();
            }

            public class IntViewModel : ViewModelBase
            {
                private int _count;

                public IntViewModel(int count)
                {
                    _count = count;
                }

                public int Count
                {
                    get => _count;
                    set => UpdateValueProperty(ref _count, value, _ => Modified = true);
                }

                public bool Modified { get; set; }
            }
        }

        public class StringPropertyTests : ViewModelBaseTests
        {
            private readonly StringViewModel _stringModel = new StringViewModel("foo");

            [Fact]
            public void WhenPropertyChanged_NewValueIsStored()
            {
                _stringModel.Name = "bar";
                _stringModel.Name.Should().Be("bar");
            }

            [Fact]
            public void WhenPropertyChanged_ModelIsModified()
            {
                _stringModel.Name = "bar";
                _stringModel.Modified.Should().BeTrue();
            }

            [Fact]
            public void WhenPropertyNotChanged_ModelIsNotModified()
            {
                _stringModel.Name = "foo";
                _stringModel.Modified.Should().BeFalse();
            }

            [Fact]
            public void WhenPropertyChangedWithSubscriber_SubscriberIsNotified()
            {
                bool notified = false;
                _stringModel.PropertyChanged += (s, a) => notified = true;
                _stringModel.Name = "bar";
                notified.Should().BeTrue();
            }

            [Fact]
            public void WhenPropertyChangedWithSubscriber_SubscriberIsNotifiedNameOfProperty()
            {
                var property = "";
                _stringModel.PropertyChanged += (s, a) => property = a.PropertyName;
                _stringModel.Name = "bar";
                property.Should().Be("Name");
            }

            [Fact]
            public void WhenPropertyNotChangedWithSubscriber_SubscriberIsNotNotified()
            {
                bool notified = false;
                _stringModel.PropertyChanged += (s, a) => notified = true;
                _stringModel.Name = "foo"; // Same value
                notified.Should().BeFalse();
            }

            public class StringViewModel : ViewModelBase
            {
                private string _name;

                public StringViewModel(string name)
                {
                    _name = name;
                }

                public string Name
                {
                    get => _name;
                    set => UpdateReferenceProperty(ref _name, value, _ => Modified = true);
                }

                public bool Modified { get; set; }
            }
        }

        public class DateTimeOffsetPropertyTests : ViewModelBaseTests
        {
            private readonly DateTimeOffset _reference = new DateTimeOffset(2012, 02, 28, 14, 35, 10, TimeSpan.FromHours(0));
            private readonly DateTimeOffsetViewModel _dateTimeOffsetModel;

            private readonly DateTimeOffset _now = DateTimeOffset.Now;

            public DateTimeOffsetPropertyTests()
            {
                _dateTimeOffsetModel = new DateTimeOffsetViewModel(_reference);
            }

            [Fact]
            public void WhenPropertyChanged_NewValueIsStored()
            {
                _dateTimeOffsetModel.Start = _now;
                _dateTimeOffsetModel.Start.Should().Be(_now);
            }

            [Fact]
            public void WhenPropertyChanged_ModelIsModified()
            {
                _dateTimeOffsetModel.Start = _now;
                _dateTimeOffsetModel.Modified.Should().BeTrue();
            }

            [Fact]
            public void WhenPropertyNotChanged_ModelIsNotModified()
            {
                _dateTimeOffsetModel.Start = _reference;
                _dateTimeOffsetModel.Modified.Should().BeFalse();
            }

            [Fact]
            public void WhenPropertyChangedWithSubscriber_SubscriberIsNotified()
            {
                bool notified = false;
                _dateTimeOffsetModel.PropertyChanged += (s, a) => notified = true;
                _dateTimeOffsetModel.Start = _now;
                notified.Should().BeTrue();
            }

            [Fact]
            public void WhenPropertyChangedWithSubscriber_SubscriberIsNotifiedNameOfProperty()
            {
                var property = "";
                _dateTimeOffsetModel.PropertyChanged += (s, a) => property = a.PropertyName;
                _dateTimeOffsetModel.Start = _now;
                property.Should().Be("Start");
            }

            [Fact]
            public void WhenPropertyNotChangedWithSubscriber_SubscriberIsNotNotified()
            {
                bool notified = false;
                _dateTimeOffsetModel.PropertyChanged += (s, a) => notified = true;
                _dateTimeOffsetModel.Start = _dateTimeOffsetModel.Start; // Same value
                notified.Should().BeFalse();
            }

            public class DateTimeOffsetViewModel : ViewModelBase
            {
                private DateTimeOffset _start;

                public DateTimeOffsetViewModel(DateTimeOffset start)
                {
                    _start = start;
                }

                public DateTimeOffset Start
                {
                    get => _start;
                    set => UpdateValueProperty(ref _start, value, _ => Modified = true);
                }

                public bool Modified { get; set; }
            }
        }

        public class TimeSpanPropertyTests : ViewModelBaseTests
        {
            private readonly TimeSpan _reference = TimeSpan.FromHours(7);

            private readonly TimeSpanViewModel _timeSpanViewModel;

            private readonly TimeSpan _duration = TimeSpan.FromDays(4);

            public TimeSpanPropertyTests()
            {
                _timeSpanViewModel = new TimeSpanViewModel(_reference);
            }

            [Fact]
            public void WhenPropertyChanged_NewValueIsStored()
            {
                _timeSpanViewModel.Duration = _duration;
                _timeSpanViewModel.Duration.Should().Be(_duration);
            }

            [Fact]
            public void WhenPropertyChanged_ModelIsModified()
            {
                _timeSpanViewModel.Duration = _duration;
                _timeSpanViewModel.Modified.Should().BeTrue();
            }

            [Fact]
            public void WhenPropertyNotChanged_ModelIsNotModified()
            {
                _timeSpanViewModel.Duration = _duration;
                _timeSpanViewModel.Duration.Should().Be(_duration);
            }

            [Fact]
            public void WhenPropertyChangedWithSubscriber_SubscriberIsNotified()
            {
                bool notified = false;
                _timeSpanViewModel.PropertyChanged += (s, a) => notified = true;
                _timeSpanViewModel.Duration = _duration;
                notified.Should().BeTrue();
            }

            [Fact]
            public void WhenPropertyChangedWithSubscriber_SubscriberIsNotifiedNameOfProperty()
            {
                var property = "";
                _timeSpanViewModel.PropertyChanged += (s, a) => property = a.PropertyName;
                _timeSpanViewModel.Duration = _duration;
                property.Should().Be("Duration");
            }

            [Fact]
            public void WhenPropertyNotChangedWithSubscriber_SubscriberIsNotNotified()
            {
                bool notified = false;
                _timeSpanViewModel.PropertyChanged += (s, a) => notified = true;
                _timeSpanViewModel.Duration = _timeSpanViewModel.Duration; // Same value
                notified.Should().BeFalse();
            }

            public class TimeSpanViewModel : ViewModelBase
            {
                private TimeSpan _duration;

                public TimeSpanViewModel(TimeSpan duration)
                {
                    _duration = duration;
                }

                public TimeSpan Duration
                {
                    get => _duration;
                    set => UpdateValueProperty(ref _duration, value, _ => Modified = true);
                }

                public bool Modified { get; set; }
            }
        }

        public class EnumPropertyTests : ViewModelBaseTests
        {
            private readonly ColorViewModel _colorModel = new ColorViewModel(Color.Blue);

            [Fact]
            public void WhenPropertyChanged_NewValueIsStored()
            {
                _colorModel.Color = Color.Red;
                _colorModel.Color.Should().Be(Color.Red);
            }

            [Fact]
            public void WhenPropertyChanged_ModelIsModified()
            {
                _colorModel.Color = Color.Red;
                _colorModel.Modified.Should().BeTrue();
            }

            [Fact]
            public void WhenPropertyChanged_ModelIsNotModified()
            {
                _colorModel.Color = Color.Blue;
                _colorModel.Modified.Should().BeFalse();
            }

            [Fact]
            public void WhenPropertyChangedWithSubscriber_SubscriberIsNotified()
            {
                bool notified = false;
                _colorModel.PropertyChanged += (s, a) => notified = true;
                _colorModel.Color = Color.Red;
                notified.Should().BeTrue();
            }

            [Fact]
            public void WhenPropertyChangedWithSubscriber_SubscriberIsNotifiedNameOfProperty()
            {
                var property = "";
                _colorModel.PropertyChanged += (s, a) => property = a.PropertyName;
                _colorModel.Color = Color.Red;
                property.Should().Be("Color");
            }

            [Fact]
            public void WhenPropertyNotChangedWithSubscriber_SubscriberIsNotNotified()
            {
                bool notified = false;
                _colorModel.PropertyChanged += (s, a) => notified = true;
                _colorModel.Color = Color.Blue; // Same value
                notified.Should().BeFalse();
            }

            public enum Color
            {
                Black,
                Red,
                Green,
                Blue,
                White
            }

            public class ColorViewModel : ViewModelBase
            {
                private Color _color;

                public ColorViewModel(Color color)
                {
                    _color = color;
                }

                public Color Color
                {
                    get => _color;
                    set => UpdateEnumProperty(ref _color, value, _ => Modified = true);
                }

                public bool Modified { get; set; }
            }
        }

        public class ModelPropertyTests : ViewModelBaseTests
        {
            private readonly FakeViewModel<string> _viewModel;

            private string? _lastValueSeen = null;

            public ModelPropertyTests()
            {
                _viewModel = new FakeViewModel<string>("", WhenUpdated);
            }

            [Fact]
            public void WhenUpdated_UpdatesProperty()
            {
                _viewModel.Model = "newValue";
                _viewModel.Model.Should().Be("newValue");
            }

            [Fact]
            public void WhenUpdated_ModelUpdatedIsCalled()
            {
                _viewModel.Model = "newValue";
                _lastValueSeen.Should().Be("newValue");
            }

            private void WhenUpdated(string? newModel)
            {
                _lastValueSeen = newModel;
            }
        }
    }
}
