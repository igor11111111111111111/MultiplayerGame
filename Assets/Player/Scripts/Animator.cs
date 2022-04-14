
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class Animator : MonoBehaviour
    {
        // Necessarily through [SerializeField],
        // otherwise [PunRPC] will not see Animator
        [SerializeField] 
        private UnityEngine.Animator _animator;
        private PhotonView _photonView;
        private Controller _controller;
        private Freehang _freehang;

        public void Init(Controller controller, PhotonView photonView, Freehang freehang)
        {
            _photonView = photonView;
            _controller = controller;
            _freehang = freehang;

            _controller.OnMove += Move;
            _controller.OnRun += Run;
            _controller.OnJump += Jump;
            _controller.OnCrawl += Crawl;
            _controller.OnClimbLadder += ClimbLadder;
            _controller.OnSwim += Swim;
            _controller.OnForwardSprint += Sprint;

            _freehang.OnFreehang += ClimbFreehang;
            _freehang.OnShimmy += FreehangShimmy;
            _freehang.OnExit += FreehangUpDown;
        }

        ~ Animator()
        {
            _controller.OnMove -= Move;
            _controller.OnRun -= Run;
            _controller.OnJump -= Jump;
            _controller.OnCrawl -= Crawl;
            _controller.OnClimbLadder -= ClimbLadder;
            _controller.OnSwim -= Swim;
            _controller.OnForwardSprint -= Sprint;

            _freehang.OnFreehang -= ClimbFreehang;
            _freehang.OnShimmy -= FreehangShimmy;
            _freehang.OnExit -= FreehangUpDown;
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        private void Move(int direction)
        {
            _animator.SetFloat("MoveDirection", direction);

            bool active = direction != 0 ? true : false;
            _animator.SetBool("IsMoving", active);
        }

        private void Run(bool active)
        {
            _animator.SetBool("IsRuning", active);
        }

        private void Crawl(bool active)
        {
            _animator.SetBool("IsCrawling", active);
        }

        private void ClimbLadder(bool active)
        {
            _animator.SetBool("IsClimbingLadder", active);
        }

        private void FreehangShimmy(int direction)
        {
            _animator.SetInteger("FreehangShimmyDirection", direction);
        }

        private void FreehangUpDown(Freehang.ExitState exitState)
        {
            _animator.SetInteger("FreehangUpDownDirection", (int)exitState);
        }

        private void Swim(bool active)
        {
            _animator.SetBool("IsSwimming", active);
        }

        private void ClimbFreehang(bool active)
        {
            _animator.SetBool("IsClimbingFreehang", active);
        }

        private void Jump()
        {
            _photonView.RPC(nameof(JumpRPC), RpcTarget.All);
        }

        [PunRPC]
        private void JumpRPC()
        {
            _animator.SetTrigger("OnJump");
        }

        private void Sprint()
        {
            _photonView.RPC(nameof(SprintRPC), RpcTarget.All);
        }

        [PunRPC]
        private void SprintRPC()
        {
            _animator.SetTrigger("OnSprint");
        }
    }
}