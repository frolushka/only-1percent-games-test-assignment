using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Only1PercentGames.TestAssignment
{
    public class CannonView : MonoBehaviour
    {
        // TODO
        public IObserver<Unit> Shoot => _shoot;
        private Subject<Unit> _shoot = new Subject<Unit>();
        
        // TODO
        public IObservable<Unit> OnShot => _onShot;
        private Subject<Unit> _onShot = new Subject<Unit>();
        
        [SerializeField] private Animator animator;

        private void Awake()
        {
            _shoot
                .Subscribe(_ => animator.SetTrigger("Shoot"))
                .AddTo(this);
        }

        public void OnShotHandler()
        {
            _onShot.OnNext(default);
        }
    }
}
