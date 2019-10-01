using System;

namespace WordTutor.Desktop.Tests.Fakes
{
    public sealed class FakeViewModel<T> : ViewModelBase
        where T : class, IEquatable<T>?
    {
        private readonly Action<T> _whenUpdated;

        private T _model;

        public FakeViewModel(T model, Action<T> whenUpdated)
        {
            _model = model;
            _whenUpdated = whenUpdated;
        }

        public T Model
        {
            get => _model;
            set => UpdateReferenceProperty(  
                ref _model, 
                value, 
                _whenUpdated);
        }
        /*        
Warning	CS8631	
The type 'T?' cannot be used as type parameter 'T' in the generic type 
or method 'ViewModelBase.UpdateProperty<T>(ref T, T, Action<T>?, string?)'. 
Nullability of type argument 'T?' doesn't match constraint type 'System.IEquatable<T?>?'.	
         */
    }
}
