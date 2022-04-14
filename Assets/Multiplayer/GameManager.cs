using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Farm
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private Button _leaveRoom;

        private void Awake()
        {
            _leaveRoom.onClick.AddListener(Leave);
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.LogFormat("Player {0} entered room", newPlayer.NickName);
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Debug.LogFormat("Player {0} left room", otherPlayer.NickName);
        }

        private void Leave()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}
