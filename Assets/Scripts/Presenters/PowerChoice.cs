using System;
using InputObservable;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Only1PercentGames.TestAssignment.Presenters
{
    public class PowerChoice : MonoBehaviour
    {
        public IObserver<Unit> StartChoosing => _startChoosing;
        private Subject<Unit> _startChoosing = new Subject<Unit>();
        
        public IObservable<float> OnPowerChoosen => _onPowerChoosen;
        private Subject<float> _onPowerChoosen = new Subject<float>();

        [SerializeField] private GameObject powerScreen;
        [SerializeField] private Slider powerSlider;

        private CompositeDisposable _disposables;
        private ReactiveProperty<float> _powerValue = new ReactiveProperty<float>();

        private InputObservableContext _context;
        
        private void Awake()
        {
            _context = this.DefaultInputContext();
            
            gameObject.OnDestroyAsObservable()
                .Subscribe(_ => _disposables?.Dispose());

            _startChoosing
                .Subscribe(_ => powerScreen.SetActive(true))
                .AddTo(this);
            _onPowerChoosen
                .Subscribe(_ => powerScreen.SetActive(false))
                .AddTo(this);
            
            _startChoosing
                .Subscribe(_ =>
                {
                    _disposables?.Dispose();
                    _disposables = new CompositeDisposable();

                    Observable.EveryFixedUpdate()
                        .Subscribe(tick => _powerValue.Value = Mathf.PingPong(tick * Time.fixedDeltaTime, 1))
                        .AddTo(_disposables);

                    _powerValue
                        .Subscribe(value => powerSlider.value = value)
                        .AddTo(_disposables);

                    _context.GetObservable(0)
                        .OnBegin
                        .Subscribe(_ => _onPowerChoosen.OnNext(TransformPowerValue(_powerValue.Value)))
                        .AddTo(_disposables);
                })
                .AddTo(this);
            
            _onPowerChoosen
                .Subscribe(_ => _disposables?.Dispose())
                .AddTo(this);
        }
        
        private static float TransformPowerValue(float value) => 1 - 2 * Mathf.Abs(value - 0.5f);
    }
}