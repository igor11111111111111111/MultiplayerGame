using UnityEngine;
using Photon.Pun;

namespace Player
{
    public class Factory : MonoBehaviour
    {
        [SerializeField]
        private DependencyInjection _playerPrefab;
        [SerializeField]
        private Camera _playerCameraPrefab;
        [SerializeField]
        private FreehangPanel _freehangPanel;
        [SerializeField]
        private Canvas _canvas;

        public void Init()
        {
            Create(new Vector3(0, 1, 0));
        }

        private void Create(Vector3 position)
        {
            GameObject go = PhotonNetwork.Instantiate(_playerPrefab.name, position, Quaternion.identity);
            DependencyInjection injection = go.GetComponent<DependencyInjection>();
            injection.Init();

            var camera = Instantiate(_playerCameraPrefab);
            camera.Init(injection.transform);

            var freehangPanel = Instantiate(_freehangPanel, _canvas.transform);
            freehangPanel.Init(injection.Input, injection.ClimbFreehangTrigger);
        }
    }
}
