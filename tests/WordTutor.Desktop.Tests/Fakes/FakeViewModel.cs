using System;

namespace WordTutor.Desktop.Tests.Fakes
{
    public class FakeViewModel<T> : ViewModelBase<T>
    {
        private readonly Action<T> _whenUpdated;

        public FakeViewModel(Action<T> whenUpdated)
        {
            _whenUpdated = whenUpdated;
        }

        protected override void ModelUpdated(T model)
        {
            _whenUpdated(model);
        }
    }
}
