
using UnityEngine;

namespace Player
{
    public class WallCheckTrigger : MonoBehaviour
    {
        public bool IsExist => _isExist;
        private bool _isExist;

        private void OnTriggerEnter(Collider other)
        {
            _isExist = true;
        }

        private void OnTriggerExit(Collider other)
        {
            _isExist = false;
        }
    }
}
