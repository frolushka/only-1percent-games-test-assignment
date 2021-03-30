using System;
using UniRx;
using UnityEngine;

namespace Only1PercentGames.TestAssignment.Presenters
{
    public class CameraPresenter : MonoBehaviour
    {
        // TODO
        public IObserver<Unit> ShowNextView => _showNextView;
        private Subject<Unit> _showNextView = new Subject<Unit>();

        [SerializeField] private int firstActiveIndex;
        [SerializeField] private GameObject[] cameras;

        private int _currentActive;

        private void Awake()
        {
            Initialize();

            _showNextView
                .Subscribe(_ => SetNextViewInternal())
                .AddTo(this);
        }

        private void Initialize()
        {
            _currentActive = firstActiveIndex;
            foreach (var camera in cameras)
            {
                camera.SetActive(false);
            }
            cameras[_currentActive].SetActive(true);
        }

        private void SetNextViewInternal()
        {
            cameras[_currentActive].SetActive(false);
            _currentActive = (_currentActive + 1) % cameras.Length;
            cameras[_currentActive].SetActive(true);
        }
    }
}