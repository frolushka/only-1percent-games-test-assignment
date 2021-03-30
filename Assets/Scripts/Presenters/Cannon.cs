using System;
using UniRx;
using UnityEngine;

namespace Only1PercentGames.TestAssignment.Presenters
{
    public class Cannon : MonoBehaviour
    {
        public IObserver<GameObject> Load => _load;
        private Subject<GameObject> _load = new Subject<GameObject>();

        public IObserver<float> Shoot => _shoot;
        private Subject<float> _shoot = new Subject<float>();

        private Subject<float> _onShot = new Subject<float>();
        
        public IObservable<Unit> OnHit => _onHit;
        private Subject<Unit> _onHit = new Subject<Unit>();

        public IObservable<Unit> OnMiss => _onMiss;
        private Subject<Unit> _onMiss = new Subject<Unit>();

        [SerializeField] private Transform shootPoint;
        [SerializeField] private CannonView cannonView;
        [SerializeField] private GameObject explosionEffect;
        [SerializeField] private Bullet bulletPrefab;

        private GameObject _loadedBullet;
        private float _currentPower;
        
        private void Awake()
        {
            InitializeLogging();
            
            _shoot
                .Subscribe(_ => cannonView.Shoot.OnNext(default))
                .AddTo(this);
            _shoot
                .Subscribe(power => _currentPower = power)
                .AddTo(this);

            cannonView.OnShot
                .Subscribe(_ => _onShot.OnNext(_currentPower))
                .AddTo(this);

            _load
                .Subscribe(bullet => _loadedBullet = bullet)
                .AddTo(this);
            
            _onShot
                .Subscribe(force =>
                {
                    const float powerMultiplier = 2000;
                    var bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
                    
                    _loadedBullet.transform.SetParent(bullet.transform);
                    _loadedBullet.transform.localPosition = Vector3.zero;
                    _loadedBullet.SetActive(true);
                    _loadedBullet = null;

                    bullet.OnHit.Subscribe(_onHit);
                    bullet.OnMiss.Subscribe(_onMiss);
                    bullet.GetComponent<Rigidbody>().AddForce(shootPoint.forward * powerMultiplier * force);
                })
                .AddTo(this);
            _onShot
                .Subscribe(_ => explosionEffect.SetActive(true))
                .AddTo(this);
            _onShot
                .Delay(TimeSpan.FromSeconds(1))
                .Subscribe(_ => explosionEffect.SetActive(false))
                .AddTo(this);
        }

        private void InitializeLogging()
        {
            _onMiss
                .Subscribe(_ => Debug.Log("Cannon.OnMiss"))
                .AddTo(this);
            _onHit
                .Subscribe(_ => Debug.Log("Cannon.OnHit"))
                .AddTo(this);
        }
    }
}