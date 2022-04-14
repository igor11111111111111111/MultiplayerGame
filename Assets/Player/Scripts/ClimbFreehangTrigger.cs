using Farm;
using System;
using UnityEngine;

namespace Player
{
    public class ClimbFreehangTrigger : MonoBehaviour
    {
        public Action<bool> OnCanClimb;
        public bool CanClimb => _canClimb;
        private bool _canClimb;
        public Vector3 ColliderClosestPoint => _collider.ClosestPoint(transform.position);
        public Vector3 Rotation => _collider.transform.eulerAngles;
        public Transform Transform => _collider.transform;
        private Collider _collider;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Ledge ledge))
            {
                //Debug.Log("enter ledge");
                _collider = other;
                _canClimb = true;
                OnCanClimb?.Invoke(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Ledge ledge))
            {
                //Debug.Log("exit ledge");
                _canClimb = false;
                OnCanClimb?.Invoke(false);
            }
        }
    }
}
