using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Only1PercentGames.TestAssignment
{
    public class BreakableObject : MonoBehaviour
    {
        [SerializeField] private PhysicMaterial noFrictionMaterial;
        [SerializeField] private GameObject[] parts;

        private void Start()
        {
            InitializePhysics();
            InitializeBreakableParts();
        }

        private void InitializePhysics()
        {
            var meshFilters = transform.GetComponentsInChildren<MeshFilter>();
            foreach (var mf in meshFilters)
            {
                var boxCollider = mf.gameObject.AddComponent<BoxCollider>();
                boxCollider.material = noFrictionMaterial;
            }
        }

        private void InitializeBreakableParts()
        {
            foreach (var part in parts)
            {
                part.AddComponent<Rigidbody>();
                part.tag = "Target";
            }
        }
    }
}