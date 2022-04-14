using Photon.Pun;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Input))]
    [RequireComponent(typeof(RPCEventsHandler))]
    [RequireComponent(typeof(Physics))]
    [RequireComponent(typeof(Freehang))]

    public class DependencyInjection : MonoBehaviour
    {
        [SerializeField]
        private GroundCheck _groundCheck;
        [SerializeField]
        private ClimbLadderTrigger _climbLadderTrigger;
        public ClimbFreehangTrigger ClimbFreehangTrigger => _climbFreehangTrigger;
        [SerializeField]
        private ClimbFreehangTrigger _climbFreehangTrigger;
        [SerializeField]
        private EnvironmentTrigger _environmentTrigger;
        [SerializeField]
        private WallCheck _playerWallCheck;
        public Input Input => _input;
        private Input _input;

        public void Init()
        {
            Rigidbody rigidBody = GetComponent<Rigidbody>();
            PhotonView photonView = GetComponent<PhotonView>();
            Animator pAnimator = GetComponent<Animator>();
            _input = GetComponent<Input>();
            RPCEventsHandler rpcEventsHandler = GetComponent<RPCEventsHandler>();
            Physics physics = GetComponent<Physics>();
            Freehang freehang = GetComponent<Freehang>();

            Data data = new Data(2.5f, 2f, 5f);
            _groundCheck.Init();
            _input.Init(_climbFreehangTrigger, freehang);
            Controller controller = new Controller(_input, _groundCheck, _climbLadderTrigger, _environmentTrigger, freehang);
            pAnimator.Init(controller, photonView, freehang);
            rpcEventsHandler.Init(physics, photonView, freehang);
            freehang.Init(rigidBody, controller, _climbFreehangTrigger, _input);
            physics.Init(this, controller, data, rigidBody, _groundCheck, _playerWallCheck, freehang);
            //_camera.Init(transform);
        }
    }
}
