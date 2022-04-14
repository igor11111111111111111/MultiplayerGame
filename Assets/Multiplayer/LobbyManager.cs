using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Farm
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private Text _log;
        [SerializeField]
        private Button _createRoom;
        [SerializeField]
        private Button _joinRoom;

        private bool _isConnectedToMaster;

        private async void Start()
        {
            _createRoom.onClick.AddListener(CreateRoom);
            _joinRoom.onClick.AddListener(JoinRoom);

            PhotonNetwork.NickName = "Player " + Random.Range(0, 99);
            Log("Current NickName " + PhotonNetwork.NickName);

            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.ConnectUsingSettings();

            await UniTask.WaitUntil(() => _isConnectedToMaster);
            CreateRoom();
        }

        public override void OnConnectedToMaster()
        {
            _isConnectedToMaster = true;
            Log("Connected to master");
        }

        public override void OnJoinedRoom()
        {
            Log("Joined to room");
            PhotonNetwork.LoadLevel("Game");
        }

        private void CreateRoom()
        {
            PhotonNetwork.CreateRoom(null, 
                new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
        }

        private void JoinRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        private void Log(string message)
        {
            //Debug.Log(message);
            //_log.text += "\n";
            //_log.text += message;
        }
    }
}
