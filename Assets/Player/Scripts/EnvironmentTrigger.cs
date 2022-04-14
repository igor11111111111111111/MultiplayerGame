using Farm;
using System;
using UnityEngine;

namespace Player
{
    public class EnvironmentTrigger : MonoBehaviour
    {
        public event Action<bool> OnWater;
        public bool InWater => _inWater;
        public bool _inWater;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Water water))
            {
                _inWater = true;
                OnWater?.Invoke(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Water water))
            {
                _inWater = false;
                OnWater?.Invoke(false);
            }
        }
    }
}
