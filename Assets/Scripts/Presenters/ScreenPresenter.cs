using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Only1PercentGames.TestAssignment.Presenters
{
    public class ScreenPresenter : MonoBehaviour
    {
        // TODO
        public IObserver<Unit> Show => _show;
        private Subject<Unit> _show = new Subject<Unit>();
            
        [SerializeField] private GameObject screenObject;
        [SerializeField] private Button restartButton;

        private void Awake()
        {
            _show
                .Subscribe(_ => screenObject.SetActive(true))
                .AddTo(this);
            restartButton.OnClickAsObservable()
                .Subscribe(_ => SceneManager.LoadScene(SceneManager.GetActiveScene().name))
                .AddTo(this);
        }
    }
}