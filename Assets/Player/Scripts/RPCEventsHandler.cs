using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class RPCEventsHandler : MonoBehaviour
    {
        // Necessarily through [SerializeField],
        // otherwise [PunRPC] will not see RigidBody
        [SerializeField]
        private Rigidbody _rigidbody;
        private Physics _physics;
        private PhotonView _photonView;
        private Freehang _freehang;

        public void Init(Physics physics, PhotonView view, Freehang freehang)
        {
            _physics = physics;
            _photonView = view;
            _freehang = freehang;

            _physics.OnKinematicChanged += KinematicChanged;
            _freehang.OnExit += UpDownFreehangHadler;
            _freehang.OnFreehang += FreehangHandler;
        }

        ~ RPCEventsHandler()
        {
            _physics.OnKinematicChanged -= KinematicChanged;
            _freehang.OnExit -= UpDownFreehangHadler;
            _freehang.OnFreehang -= FreehangHandler;
        }

        private void KinematicChanged(bool active)
        {
            _photonView.RPC(nameof(ChangeKinematicRPC), RpcTarget.All, active);
        }

        private void UpDownFreehangHadler(Freehang.ExitState exitState)
        {
            bool active = exitState == Freehang.ExitState.UpStarted ? true : false;
            _photonView.RPC(nameof(ChangeKinematicRPC), RpcTarget.All, active);
        }

        private void FreehangHandler(bool active)
        {
            _photonView.RPC(nameof(ChangeRigidbodyConstaintsRPC), RpcTarget.All, active);
        }

        [PunRPC]
        private void ChangeRigidbodyConstaintsRPC(bool active)
        {
            if (active)
            {
                _rigidbody.constraints =
                RigidbodyConstraints.FreezeRotation |
                RigidbodyConstraints.FreezePositionY;
            }
            else
            {
                _rigidbody.constraints =
                RigidbodyConstraints.FreezeRotation &
                ~RigidbodyConstraints.FreezePositionY;
            }
        }

        [PunRPC]
        private void ChangeKinematicRPC(bool active)
        {
            _rigidbody.isKinematic = active;
        }
    }
}
