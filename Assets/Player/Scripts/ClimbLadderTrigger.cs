using Farm;
using System;
using UnityEngine;

namespace Player
{
    public class ClimbLadderTrigger : MonoBehaviour
    {
        public event Action<bool> OnFoundLadder;
        public bool IsClimb => _isClimb;
        private bool _isClimb;

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Ladder ladder))
            {
                //Debug.Log("enter ladder");
                _isClimb = true;
                OnFoundLadder?.Invoke(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Ladder ladder))
            {
                //Debug.Log("exit ladder");
                _isClimb = false;
                OnFoundLadder?.Invoke(false);
            }
        }
    }
}
