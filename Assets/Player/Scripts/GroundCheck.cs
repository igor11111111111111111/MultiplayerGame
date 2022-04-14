using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class GroundCheck : MonoBehaviour
    {
        public event Action<bool> OnGrounded;
        public bool IsGrounded => _colliders.Count > 0;
        private List<Collider> _colliders;

        public void Init()
        {
            _colliders = new List<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("add " + other.name);
            if(!_colliders.Contains(other))
                _colliders.Add(other);
            OnGrounded?.Invoke(true);
        }

        private void OnTriggerExit(Collider other)
        {
            //Debug.Log("remove " + other.name);
            if (_colliders.Contains(other))
                _colliders.Remove(other);
            OnGrounded?.Invoke(false);
        }
    }
}
