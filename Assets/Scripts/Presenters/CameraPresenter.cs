using System;
using UniRx;
using UnityEngine;

namespace Only1PercentGames.TestAssignment.Presenters
{
    public class CameraPresenter : MonoBehaviour
    {
        public IObserver<Unit> ShowNextView => _showNextView;
        private Subject<Unit> _showNextView = new Subject<Unit>();

        [SerializeField] private int firstActiveIndex;
        [SerializeField] private GameObject[] cameras;

        private ReactiveProperty<int> _currentActiveIndex;

        private void Awake()
        {
            _currentActiveIndex = new ReactiveProperty<int>(firstActiveIndex);
            
            Initialize();

            _showNextView
                .Subscribe(_ => cameras[_currentActiveIndex.Value].SetActive(false))
                .AddTo(this);
            _showNextView
                .Subscribe(_ => _currentActiveIndex.Value = (_currentActiveIndex.Value + 1) % cameras.Length)
                .AddTo(this);
            _currentActiveIndex
                .Subscribe(index => cameras[index].SetActive(true))
                .AddTo(this);
        }

        private void Initialize()
        {
            foreach (var camera in cameras)
            {
                camera.SetActive(false);
            }
            cameras[_currentActiveIndex.Value].SetActive(true);
        }
    }
}