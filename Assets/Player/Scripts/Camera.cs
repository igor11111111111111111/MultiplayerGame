
using UnityEngine;
using Cinemachine;

namespace Player
{
    public class Camera : MonoBehaviour
    {
        private CinemachineVirtualCamera _camera;

        public void Init(Transform transform)
        {
            _camera = GetComponent<CinemachineVirtualCamera>();

            _camera.Follow = transform;
            _camera.LookAt = transform;
        }
    }
}
