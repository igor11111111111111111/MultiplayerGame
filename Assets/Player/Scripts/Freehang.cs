using Cysharp.Threading.Tasks;
using Farm;
using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class Freehang : MonoBehaviour
    {
        public event Action<bool> OnFreehang;
        public event Action<int> OnShimmy;
        public event Action<ExitState> OnExit;
        public enum ExitState
        {
            UpStarted = 1,
            DownStarted = -1,
            Canceled = 0
        }

        public enum FreehangState
        {
            None,
            Start,
            Stay,
            End
        }
        public FreehangState State => _state;
        private FreehangState _state;

        [SerializeField]
        AnimationClip _freehangUp, _freehangDown;

        private Rigidbody _rigidbody;
        private Controller _controller;
        private ClimbFreehangTrigger _climbFreehangTrigger;
        private Input _input;

        private int _moveDirection;
        private Input.Climb _inputClimb;

        public void Init(Rigidbody rigidbody, Controller playerController, ClimbFreehangTrigger climbFreehangTrigger, Input input)
        {
            _rigidbody = rigidbody;
            _controller = playerController;
            _climbFreehangTrigger = climbFreehangTrigger;
            _input = input;

            _state = FreehangState.None;

            _input.OnClimbFreehang += InputFreehangHandler;
        }

        ~ Freehang()
        {
            _input.OnClimbFreehang -= InputFreehangHandler;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void InputFreehangHandler(Input.Climb inputClimb)
        {
            _inputClimb = inputClimb;

            if(inputClimb == Input.Climb.Start)
            {
                ClimbFreehang(true);
            }

            if (_state == FreehangState.Start)
                return;

            if (inputClimb == Input.Climb.DropUp || inputClimb == Input.Climb.DropDown)
            {
                ClimbFreehang(false);
            }
            else
            {
                _moveDirection = (int)inputClimb;
            }

            OnShimmy?.Invoke(_moveDirection);
        }

        private void Move()
        {
            if (_state == FreehangState.None)
                return;

            if (_moveDirection == 0)
            {
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
            }

            GetForce(Vector3.right, _moveDirection);
            CatchLedgeEdge();
        }

        private void GetForce(Vector3 orientation, int direction)
        {
            Vector3 force = orientation * direction;
            _rigidbody.AddRelativeForce(force, ForceMode.VelocityChange);
        }

        private async void ClimbFreehang(bool active)
        {
            OnFreehang?.Invoke(active);

            if (active)
            {
                StartCoroutine(JumpToFreehang());
            }
            else
            {
                _state = FreehangState.End;

                if(_inputClimb == Input.Climb.DropUp)
                {
                    OnExit?.Invoke(ExitState.UpStarted);
                    await UniTask.Delay(_freehangUp.LengthMillisec());

                    float angleRad = Mathf.Deg2Rad * _climbFreehangTrigger.Rotation.y;
                    Vector3 animationOffset = new Vector3
                    (
                        Mathf.Sin(angleRad) * 0.6f, 
                        2.11f,
                        Mathf.Cos(angleRad) * 0.6f
                    );
                    transform.position += animationOffset;
                }
                else if (_inputClimb == Input.Climb.DropDown)
                {
                    OnExit?.Invoke(ExitState.DownStarted);
                    await UniTask.Delay(_freehangDown.LengthMillisec());
                }

                _state = FreehangState.None;
                OnExit?.Invoke(ExitState.Canceled);
            }
        }

        private IEnumerator JumpToFreehang()
        {
            _state = FreehangState.Start;
            _moveDirection = 0;
            float speed = 3f;

            transform.rotation = Quaternion.Euler(0, _climbFreehangTrigger.Rotation.y, 0);

            Vector3 offset = new Vector3(0, 0.392f, 0);

            Vector3 target = new Vector3
            (
                _climbFreehangTrigger.ColliderClosestPoint.x,
                transform.position.y,
                _climbFreehangTrigger.ColliderClosestPoint.z
            )
            + offset;

            var center = transform.position + (target - transform.position) / 2;
            Vector3 middleTarget = new Vector3(center.x, center.y + 1, center.z);

            while (transform.position != middleTarget)
            {
                transform.position = Vector3.MoveTowards(transform.position, middleTarget, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            while (transform.position != target)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(1f);
            _state = FreehangState.Stay;
        }

        private void CatchLedgeEdge()
        {
            var delta = _input.transform.position - _climbFreehangTrigger.Transform.position;
            var absDelta = delta.Absolute();
            var absDeltaXZ = new Vector2(absDelta.x, absDelta.z);
            var magnitudeAbsDeltaXZ = absDeltaXZ.magnitude;

            if (magnitudeAbsDeltaXZ > _climbFreehangTrigger.Transform.localScale.x / 2f)
            {
                _inputClimb = Input.Climb.DropDown;
                ClimbFreehang(false);
            }
        }
    }
}
