using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Only1PercentGames.TestAssignment.Presenters
{
    public class LevelPresenter : MonoBehaviour
    {
        [SerializeField] private Cannon cannon;
        [SerializeField] private PowerChoice powerChoice;
        [SerializeField] private BreakableObjectPresenter breakableObjectPresenter;
        [SerializeField] private CannonReloader cannonReloader;
        [SerializeField] private CameraPresenter cameraPresenter;
        [Space]
        [SerializeField] private ScreenPresenter victoryScreenPresenter;
        [SerializeField] private ScreenPresenter loseScreenPresenter;
        [Space] 
        [SerializeField] private int bulletsCount;
        [SerializeField] private GameObject bulletViewPrefab;

        private ReactiveProperty<int> _remainBullets;

        private void Awake()
        {
            _remainBullets = new ReactiveProperty<int>(bulletsCount);
        } 

        private void Start()
        {
            _remainBullets
                .Subscribe()
                .AddTo(this);
            
            powerChoice.OnPowerChoosen
                .Subscribe(cannon.Shoot)
                .AddTo(this);

            cannon.OnHit
                .Subscribe(victoryScreenPresenter.Show)
                .AddTo(this);
            
            cannon.OnMiss
                .Subscribe(cameraPresenter.ShowNextView)
                .AddTo(this);
            cannon.OnMiss
                .Subscribe(_ => _remainBullets.Value -= 1)
                .AddTo(this);

            _remainBullets
                .Where(count => count <= 0)
                .Subscribe(_ => loseScreenPresenter.Show.OnNext(default))
                .AddTo(this);

            cannonReloader.OnEnterReloadZone
                .Subscribe(cannon.Load)
                .AddTo(this);
            cannonReloader.OnEnterReloadZone
                .Subscribe(_ => cameraPresenter.ShowNextView.OnNext(default))
                .AddTo(this);
            cannonReloader.OnEnterReloadZone
                .Subscribe(_ => powerChoice.StartChoosing.OnNext(default))
                .AddTo(this);
            
            var start = new ReactiveCommand();
            start
                .Subscribe(breakableObjectPresenter.Spawn)
                .AddTo(this);
            start
                .Subscribe(_ => cannonReloader.SpawnBullets(bulletViewPrefab, bulletsCount))
                .AddTo(this);
            start.Execute();
        }
    }
}