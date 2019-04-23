using System;

namespace WordTutor.Desktop.Tests.Fakes
{
    public class FakeViewModel<T> : ViewModelBase
        where T : IEquatable<T>
    {
        private readonly Action<T> _whenUpdated;

        private T _model;

        public FakeViewModel(Action<T> whenUpdated)
        {
            _whenUpdated = whenUpdated;
        }

        public T Model
        {
            get => _model;
            set => UpdateProperty(ref _model, value, m => ModelUpdated(m));
        }

        protected void ModelUpdated(T model) => _whenUpdated(model);
    }
}
