using Player;
using UnityEngine;

namespace Farm
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField]
        private Factory _playerFactory;

        private void Awake()
        {
            _playerFactory.Init();
        }
    }
}
