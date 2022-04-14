using System;
using UnityEngine;

namespace Player
{
    public class WallCheck : MonoBehaviour
    {
        public bool IsFrontExist => _front.IsExist;
        public bool IsBehindExist => _behind.IsExist;
        [SerializeField]
        private WallCheckTrigger _front;
        [SerializeField]
        private WallCheckTrigger _behind;
    }
}
