using System;
using UniRx;
using UnityEngine;

namespace Only1PercentGames.TestAssignment.Presenters
{
    public class CannonReloader : MonoBehaviour
    {
        public IObservable<GameObject> OnEnterReloadZone => _onEnterReloadZone;
        private Subject<GameObject> _onEnterReloadZone = new Subject<GameObject>();
        
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private InactiveBullet inactiveBulletPrefab;

        public void SpawnBullets(GameObject bulletViewPrefab, int count)
        {
            for (var i = 0; i < spawnPoints.Length && i < count; i++)
            {
                var bullet = Instantiate(inactiveBulletPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
                var bulletView = Instantiate(bulletViewPrefab, bullet.transform);

                bullet.OnEnterReloadZone
                    .Subscribe(_onEnterReloadZone)
                    .AddTo(this);
            }
        }
    }
}