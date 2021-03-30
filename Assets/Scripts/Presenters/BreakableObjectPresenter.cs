using System;
using UniRx;
using UnityEngine;

namespace Only1PercentGames.TestAssignment.Presenters
{
    public class BreakableObjectPresenter : MonoBehaviour
    {
        public IObserver<Unit> Spawn => _spawn;
        private Subject<Unit> _spawn = new Subject<Unit>();
        
        [SerializeField] private BreakableObject[] brekablePrefabs;
        [SerializeField] private Transform spawnPoint;
        
        private void Awake()
        {
            _spawn
                .Subscribe(_ => Instantiate(brekablePrefabs.Random(), spawnPoint))
                .AddTo(this);
        }
    }
}