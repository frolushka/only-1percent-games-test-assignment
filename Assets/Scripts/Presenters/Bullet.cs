using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Only1PercentGames.TestAssignment.Presenters
{
    public class Bullet : MonoBehaviour
    {
        public IObservable<Unit> OnHit => _onHit;
        private Subject<Unit> _onHit = new Subject<Unit>();

        public IObservable<Unit> OnMiss => _onMiss;
        private Subject<Unit> _onMiss = new Subject<Unit>();

        private CompositeDisposable _disposables;
        private void Awake()
        {
            InitializeLogging();
            
            _disposables = new CompositeDisposable();

            var hitObservable = gameObject.OnCollisionEnterAsObservable()
                .Where(collision => collision.gameObject.CompareTag("Target"));
            hitObservable
                .Subscribe(_ => _disposables.Dispose())
                .AddTo(_disposables);
            hitObservable
                .Subscribe(_ => _onHit.OnNext(default))
                .AddTo(_disposables);

            var missObservable = gameObject.OnCollisionEnterAsObservable()
                .Delay(TimeSpan.FromSeconds(1));
            missObservable                
                .Subscribe(_ => _onMiss.OnNext(default))
                .AddTo(_disposables);
            missObservable
                .Subscribe(_ => _disposables.Dispose())
                .AddTo(_disposables);
        }

        private void InitializeLogging()
        {
            gameObject.OnCollisionEnterAsObservable()
                .Subscribe(collision => Debug.Log($"{name} collided with {collision.gameObject.name}"))
                .AddTo(this);
        }
    }
}