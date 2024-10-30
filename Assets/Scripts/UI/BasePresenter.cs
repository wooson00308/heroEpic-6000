using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System;

namespace Scripts.UI
{
    public interface IBasePresenter<out TView, out TModel>
        where TView : BaseView
        where TModel : BaseModel
    {
        public TView View { get; }
        public TModel Model { get; }

        public Transform tr { get; }
        // 인터페이스 정의
        public void ReactiveDispose();
    }

    public abstract class BasePresenter<V, M> : MonoBehaviour, IBasePresenter<V, M>
        where V : BaseView
        where M : BaseModel
    {
        public Transform tr => transform;
        public V View => _view;
        public M Model => _model;

        [SerializeField]
        protected V _view;
        [SerializeField]
        protected M _model;

        protected List<IDisposable> _disposables = new List<IDisposable>();

        protected IDisposable Subscribe<T>(IObservable<T> observable, Action<T> action)
        {
            if (observable == null || action == null)
            {
                Debug.Log("잘못 사용하고 있습니다.");
                return null;
            }

            _disposables.Add(observable.Subscribe(action));
            return _disposables[^1];
        }

        protected void AddDisposable(IDisposable disposable)
        {
            if (_disposables.Contains(disposable)) return;
            _disposables.Add(disposable);
        }

        public virtual void ReactiveDispose()
        {
            if (_disposables.Count == 0 || _disposables == null) return;
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            _disposables.Clear();
        }
    }
}