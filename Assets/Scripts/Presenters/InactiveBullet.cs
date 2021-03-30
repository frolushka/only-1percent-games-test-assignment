using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Only1PercentGames.TestAssignment.Presenters
{
    public class InactiveBullet : MonoBehaviour
    {
        public IObservable<GameObject> OnEnterReloadZone => _onEnterReloadZone;
        private Subject<GameObject> _onEnterReloadZone = new Subject<GameObject>();

        private Rigidbody _rb;
        private Vector3 _defaultPosition;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _defaultPosition = _rb.position;
            
            gameObject.OnMouseDownAsObservable()
                .SelectMany(_ => gameObject.UpdateAsObservable())
                .TakeUntil(gameObject.OnMouseUpAsObservable())
                .Select(_ => Input.mousePosition)
                .RepeatSafe()
                .Subscribe(x =>
                {
                    var ray = Camera.main.ScreenPointToRay(x);
                    // TODO
                    var point = ray.GetPoint(5);
                    _rb.MovePosition(point);
                })
                .AddTo(this);

            gameObject.OnMouseUpAsObservable()
                .Subscribe(_ => _rb.MovePosition(_defaultPosition))
                .AddTo(this);

            gameObject.OnTriggerEnterAsObservable()
                .Where(collider => collider.CompareTag("Reload Zone"))
                .Subscribe(collider =>
                {
                    var view = transform.GetChild(0).gameObject;
                    view.transform.SetParent(null);
                    view.SetActive(false);
                    _onEnterReloadZone.OnNext(view);
                    Destroy(gameObject);
                })
                .AddTo(this);
        }
    }
}