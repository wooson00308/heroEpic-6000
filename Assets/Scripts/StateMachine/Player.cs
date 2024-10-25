using UnityEngine;

namespace Scripts.StateMachine
{
    public class Player : BaseStateMachine
    {
        public static Player s_player;

        private void OnEnable()
        {
            if (s_player == null || !s_player.GetInstanceID().Equals(GetInstanceID()))
            {
                s_player = this;
            } 
        }

        private void OnDisable()
        {
            if(s_player.GetInstanceID().Equals(GetInstanceID()))
            {
                s_player = null;
            }
        }

        Vector2 _input = Vector2.zero;
        private Vector2 GetInput()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            return new Vector2(moveX, moveY).normalized;
        }

        public override void OnUpdate()
        {
            _input = GetInput();
            base.OnUpdate();
        }

        protected override void IdleState(FSM fsm, FSM.Step step, FSM.State state)
        {
            if(step == FSM.Step.Enter)
            {
                _animator.CrossFade(_idleHash, 0f);
            }
            else if(step == FSM.Step.Update)
            {
                if (_input != Vector2.zero)
                {
                    _fsm.TransitionTo(RunState);
                }
            }
            else
            {

            }
        }

        protected override void RunState(FSM fsm, FSM.Step step, FSM.State state)
        {
            if(step == FSM.Step.Enter)
            {
                _animator.CrossFade(_runHash, 0f);
            }
            else if (step == FSM.Step.Update)
            {
                if(_input != Vector2.zero)
                {
                    _unit.RunAgent(_input);
                }
                else
                {
                    _fsm.TransitionTo(IdleState);
                }
            }
            else
            {

            }
        }

        protected override void HitState(FSM fsm, FSM.Step step, FSM.State state)
        {
            if (step == FSM.Step.Enter)
            {
                _animator.CrossFade(_hitHash, 0f);
            }
            else if (step == FSM.Step.Update)
            {
                if (_animator.GetBool("Hit")) return;

                if (_input != Vector2.zero)
                {
                    _fsm.TransitionTo(RunState);
                }
                else
                {
                    _fsm.TransitionTo(IdleState);
                }
            }
            else
            {

            }
        }

        protected override void DeathState(FSM fsm, FSM.Step step, FSM.State state)
        {
            if (step == FSM.Step.Enter)
            {
                _animator.CrossFade(_deathHash, 0f);
            }
            else if (step == FSM.Step.Update)
            {

            }
            else
            {

            }
        }
    }
}
