using Photon.Pun;
using System;
using UnityEngine;

namespace Player
{

    public class Input : MonoBehaviour
    {
        public event Action<int> OnMove;
        public event Action<int> OnRotate;
        public event Action<bool> OnRun;
        public event Action<bool> OnCrawl;
        public event Action<Climb> OnClimbFreehang;
        public event Action OnJump;
        public event Action OnForwardSprint;
        
        public enum Climb
        {
            Start = 4,
            DropUp = 3,
            DropDown = 2,
            Right = 1,
            Stay = 0,
            Left = -1
        }

        // Necessarily through [SerializeField],
        // otherwise "PhotonView" will not see "PhotonView.IsMine"
        [SerializeField]
        private PhotonView _photonView;
        private ClimbFreehangTrigger _climbFreehangTrigger;
        private Freehang _freehang;

        private int _oldMove;
        private int _oldRotate;
        public bool IsRun => _isRun;
        private bool _isRun;
        private bool _oldIsRun;
        public bool IsClawl => _isCrawl;
        private bool _isCrawl;
        private bool _oldIsCrawl;

        private float _oldSprintClick;
        public bool isFreezed;

        private Climb _oldLateralMove;

        public void Init(ClimbFreehangTrigger climbFreehangTrigger, Freehang freehang)
        {
            _climbFreehangTrigger = climbFreehangTrigger;
            _freehang = freehang;
        }

        private void Update()
        {
            if (isFreezed)
                return;

            Move();
            Run();
            Rotate();
            Jump();
            Crawl();
            ClimbFreehang();
        }

        private void Move()
        {
            int move = 0;

            if (_photonView.IsMine)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.W))
                {
                    Sprint();
                }
                else if (UnityEngine.Input.GetKey(KeyCode.W))
                {
                    move = 1;
                }
                else if (UnityEngine.Input.GetKey(KeyCode.S))
                {
                    move = -1;
                }

                if (_freehang.State != Freehang.FreehangState.None)
                    move = 0;

                if (move == _oldMove)
                    return;
            }

            OnMove?.Invoke(move);
            _oldMove = move;
        }

        private void Rotate()
        {
            int rotate = 0;

            if (_photonView.IsMine)
            {
                if (UnityEngine.Input.GetKey(KeyCode.D))
                {
                    rotate = 1;
                }
                else if (UnityEngine.Input.GetKey(KeyCode.A))
                {
                    rotate = -1;
                }

                if (rotate == _oldRotate)
                    return;
            }

            OnRotate?.Invoke(rotate);
            _oldRotate = rotate;
        }

        private void Run()
        {
            if (_photonView.IsMine)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
                {
                    _isRun = true;
                }
                else if (UnityEngine.Input.GetKeyUp(KeyCode.LeftShift))
                {
                    _isRun = false;
                }

                if (_oldIsRun == _isRun)
                    return;
            }

            OnRun?.Invoke(_isRun);
            _oldIsRun = _isRun;
        }

        private void Crawl()
        {
            if (_photonView.IsMine)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.LeftControl))
                {
                    _isCrawl = true;
                }
                else if (UnityEngine.Input.GetKeyUp(KeyCode.LeftControl))
                {
                    _isCrawl = false;
                }

                if (_oldIsCrawl == _isCrawl)
                    return;
            }

            OnCrawl?.Invoke(_isCrawl);
            _oldIsCrawl = _isCrawl;
        }

        private void Jump()
        {
            if (_photonView.IsMine)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
                {
                    OnJump?.Invoke();
                }
            }
        }

        private void Sprint()
        {
            if (_freehang.State != Freehang.FreehangState.None)
                return;

            float deltaBetwClicks = Time.time - _oldSprintClick;
            _oldSprintClick = Time.time;
            if (deltaBetwClicks < 0.2)
            {
                OnForwardSprint?.Invoke();
            }
        }

        private void ClimbFreehang()
        {
            if (_photonView.IsMine)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.E) && _climbFreehangTrigger.CanClimb)
                {
                    OnClimbFreehang?.Invoke(Climb.Start);
                }

                if (_freehang.State != Freehang.FreehangState.Stay)
                    return;

                if (UnityEngine.Input.GetKeyDown(KeyCode.W))
                {
                    OnClimbFreehang?.Invoke(Climb.DropUp);
                }
                else if (UnityEngine.Input.GetKeyDown(KeyCode.S))
                {
                    OnClimbFreehang?.Invoke(Climb.DropDown);
                }
            }

            ClimblLateralMove();
        }

        private void ClimblLateralMove()
        {
            Climb move = Climb.Stay;

            if (_photonView.IsMine)
            {
                if (UnityEngine.Input.GetKey(KeyCode.D))
                {
                    move = Climb.Right;
                }
                else if (UnityEngine.Input.GetKey(KeyCode.A))
                {
                    move = Climb.Left;
                }

                if (_oldLateralMove == move)
                    return;
            }

            OnClimbFreehang?.Invoke(move);
            _oldLateralMove = move;
        }
    }
}
