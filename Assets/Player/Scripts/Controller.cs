using System;
using UnityEngine;

namespace Player
{
    public class Controller
    {
        public event Action<int> OnMove;
        public event Action<int> OnRotate;
        public event Action<bool> OnRun;
        public event Action<bool> OnCrawl;
        public event Action<bool> OnClimbLadder;
        public event Action<bool> OnSwim;
        public event Action OnJump;
        public event Action OnForwardSprint;

        private Input _input;
        private GroundCheck _groundCheck;
        private ClimbLadderTrigger _climbLadderTrigger;
        private EnvironmentTrigger _environmentTrigger;
        private Freehang _freehang;

        public Controller(Input input, GroundCheck groundCheck, ClimbLadderTrigger climbLadderTrigger, EnvironmentTrigger environmentTrigger, Freehang freehang)
        {
            _input = input;
            _groundCheck = groundCheck;
            _climbLadderTrigger = climbLadderTrigger;
            _environmentTrigger = environmentTrigger;
            _freehang = freehang;

            _groundCheck.OnGrounded += Grounded;
            _climbLadderTrigger.OnFoundLadder += ClimbLadder;
            _environmentTrigger.OnWater += Swim;

            _input.OnMove += Move;
            _input.OnRun += Run;
            _input.OnRotate += Rotate;
            _input.OnJump += Jump;
            _input.OnCrawl += Crawl;
            _input.OnForwardSprint += Sprint;
        }

        ~ Controller()
        {
            _groundCheck.OnGrounded -= Grounded;
            _climbLadderTrigger.OnFoundLadder -= ClimbLadder;
            _environmentTrigger.OnWater -= Swim;

            _input.OnMove -= Move;
            _input.OnRun -= Run;
            _input.OnRotate -= Rotate;
            _input.OnJump -= Jump;
            _input.OnCrawl -= Crawl;
            _input.OnForwardSprint -= Sprint;
        }

        private void Move(int direction)
        {
            OnMove?.Invoke(direction);
        }

        private void Run(bool active)
        {
            if (_input.IsClawl)
                return;

            OnRun?.Invoke(active);
        }

        private void Rotate(int direction)
        {
            OnRotate?.Invoke(direction);
        }

        private void Jump()
        {
            if (!_groundCheck.IsGrounded || 
                _input.IsClawl || 
                _environmentTrigger.InWater)
                return;

            OnJump?.Invoke();
        }

        private void Crawl(bool active)
        {
            if (_input.IsRun)
                return;
            
            OnCrawl?.Invoke(active);
        }

        private void Grounded(bool active)
        {
            if (active)
                OnClimbLadder?.Invoke(false);
        }

        private void ClimbLadder(bool active)
        {
            OnClimbLadder?.Invoke(active);
        }

        private void Swim(bool active)
        {
            OnSwim?.Invoke(active);
        }

        private void Sprint()
        {
            OnForwardSprint?.Invoke();
        }
    }
}
