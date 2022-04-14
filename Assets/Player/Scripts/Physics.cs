using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    public class Physics : MonoBehaviour
    {
        public event Action<bool> OnKinematicChanged;

        private DependencyInjection _player;
        private Controller _controller;
        private Data _data;
        private Rigidbody _rigidbody;
        private GroundCheck _groundCheck;
        private WallCheck _playerWallCheck;
        private SpeedHandler _speedHandler;
        private Freehang _freehang;

        private int _moveDirection;
        private int _rotateDirection;

        public void Init(DependencyInjection player, Controller controller, Data data, Rigidbody rigidbody, GroundCheck groundCheck, WallCheck playerWallCheck, Freehang freehang)
        {
            _player = player;
            _controller = controller;
            _data = data;
            _rigidbody = rigidbody;
            _groundCheck = groundCheck;
            _playerWallCheck = playerWallCheck;
            _freehang = freehang;
            _speedHandler = new SpeedHandler(_data, _controller);

            _controller.OnMove += MoveHandler;
            _controller.OnRotate += RotateHandler;
            _controller.OnJump += Jump;
        }

        ~ Physics()
        {
            _controller.OnMove -= MoveHandler;
            _controller.OnRotate -= RotateHandler;
            _controller.OnJump -= Jump;
        }

        private void FixedUpdate()
        {
            Move(_moveDirection);
            Rotate(_rotateDirection);
        }

        private void MoveHandler(int direction)
        {
            _moveDirection = direction;
        }

        private void RotateHandler(int direction)
        {
            _rotateDirection = direction;

            if (_freehang.State == Freehang.FreehangState.None)
                SetRotateConstaint(direction);
        }

        private void Move(int direction)
        {
            if (_speedHandler == null)
                return;

            if (_speedHandler.IsHorizontal())
            {
                HorizontalMove(direction);
            }
            else
            {
                VerticalMove(direction);
            }
        }

        private void HorizontalMove(int direction)
        {

            if (direction == 1 && _playerWallCheck.IsFrontExist ||
                direction == -1 && _playerWallCheck.IsBehindExist)
                return;

            if(direction == 0)
            {
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
            }

            GetForce(Vector3.forward, direction);

            _rigidbody.velocity = new Vector3
            (
                _rigidbody.velocity.normalized.x * _speedHandler.MoveSpeed,
                _rigidbody.velocity.y,
                _rigidbody.velocity.normalized.z * _speedHandler.MoveSpeed
            );
        }

        private void VerticalMove(int direction)
        {
            KinematicRefresh(direction);

            GetForce(Vector3.up, direction);

            _rigidbody.velocity = new Vector3
            (
                0,
                _rigidbody.velocity.normalized.y * _data.MoveSpeed,
                0
            );
        }

        private void GetForce(Vector3 orientation, int direction)
        {
            Vector3 force = orientation * direction;
            _rigidbody.AddRelativeForce(force, ForceMode.VelocityChange);
        }

        private void KinematicRefresh(int direction)
        {
            bool isKinematic = (direction == 0 && !_groundCheck.IsGrounded)
                ? true
                : false;

            OnKinematicChanged?.Invoke(isKinematic);
        }

        private void Rotate(int direction)
        {
            if (_speedHandler == null || _speedHandler.IsClimbing)
                return;

            if(_freehang.State == Freehang.FreehangState.None && direction != 0)
            {
                _player.transform.Rotate(0, direction * _data.RotationSpeed, 0);
            }
        }

        private void SetRotateConstaint(int direction)
        {
            if (direction == 0)
            {
                _rigidbody.constraints =
                    RigidbodyConstraints.FreezeRotation &
                    ~RigidbodyConstraints.FreezePosition;
            }
            else
            {
                _rigidbody.constraints =
                    ~RigidbodyConstraints.FreezeRotationY &
                    ~RigidbodyConstraints.FreezePosition;
            }
        }

        private void Jump()
        {
            Vector3 force = Vector3.up * _data.JumpForce;
            _rigidbody.AddRelativeForce(force, ForceMode.Impulse);
        }
    }
}
