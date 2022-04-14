using UnityEngine;

namespace Player
{
    public class SpeedHandler
    {
        public float MoveSpeed => _moveSpeed;
        private float _moveSpeed;
        public bool IsClimbing => _isClimbing;
        private bool _isClimbing;

        private Data _data;
        private Controller _controller;
        
        public enum Direction
        {
            forward,
            up
        }

        public SpeedHandler(Data data, Controller controller)
        {
            _data = data;
            _controller = controller;
            _moveSpeed = _data.MoveSpeed;

            _controller.OnCrawl += Crawl;
            _controller.OnClimbLadder += Climb;
            _controller.OnRun += Run;
        }

        ~ SpeedHandler()
        {
            _controller.OnCrawl -= Crawl;
            _controller.OnClimbLadder -= Climb;
            _controller.OnRun -= Run;
        }

        private void Crawl(bool active)
        {
            _moveSpeed = active 
                ? _data.MoveSpeed * Data.SIT_COEFF 
                : _data.MoveSpeed;
        }

        private void Run(bool active)
        {
            _moveSpeed = active
                ? _data.MoveSpeed * Data.RUN_COEFF
                : _data.MoveSpeed;
        }

        private void Climb(bool active)
        {
            _isClimbing = active;
        }

        public bool IsHorizontal()
        {
            Direction dir = _isClimbing
                ? Direction.up
                : Direction.forward;

            return dir == Direction.forward;
        }
    }
}
