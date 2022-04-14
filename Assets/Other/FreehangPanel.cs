using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class FreehangPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject _body;
        private Input _input;
        private ClimbFreehangTrigger _climbFreehangTrigger;

        public void Init(Input input, ClimbFreehangTrigger climbFreehangTrigger)
        {
            _input = input;
            _climbFreehangTrigger = climbFreehangTrigger;

            _input.OnClimbFreehang += Show;
            _climbFreehangTrigger.OnCanClimb += Show;

            Show(false);
        }

        ~ FreehangPanel()
        {
            _input.OnClimbFreehang -= Show;
            _climbFreehangTrigger.OnCanClimb -= Show;
        }

        private void Show(Input.Climb climb)
        {
            if(climb == Input.Climb.Start)
            {
                Show(false);
            }
        }

        private void Show(bool active)
        {
            _body.SetActive(active);
        }
    }
}
