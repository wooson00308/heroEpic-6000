using UnityEngine;

namespace Scripts.StateMachine
{
    public class Player : BaseStateMachine
    {
        public static Player s_player;

        private FSM.State _attackState;

        private Vector2 _inputMove;
        private (Vector3 mousePos, bool isInput) _inputMouse;

        private readonly int _attackHash = Animator.StringToHash("Attack");

        protected override void SetState()
        {
            base.SetState();

            _attackState = AttackState;
        }

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

        private Vector2 GetInputMove()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            return new Vector2(moveX, moveY).normalized;
        }

        private (Vector3, bool) GetInputAttack()
        {
            if(Input.GetMouseButton(0))
            {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                return (mousePos, true);
            }

            return (Vector3.zero, false);
        }

        public override void OnUpdate()
        {
            _inputMove = GetInputMove();
            _inputMouse = GetInputAttack();
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
                if(_inputMouse.isInput)
                {
                    _fsm.TransitionTo(AttackState);
                    return;
                }

                if (_inputMove != Vector2.zero)
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
            if (step == FSM.Step.Enter)
            {
                _animator.CrossFade(_runHash, 0f);
            }
            else if (step == FSM.Step.Update)
            {
                _animator.CrossFade(_runHash, 0f);

                if (_inputMove != Vector2.zero)
                {
                    _unit.RunAgent(_inputMove);
                }
                else
                {
                    _fsm.TransitionTo(IdleState);
                    return;
                }


                if (_inputMouse.isInput)
                {
                    _fsm.TransitionTo(AttackState);
                }
            }
            else
            {

            }
        }

        private void AttackState(FSM fsm, FSM.Step step, FSM.State state)
        {
            if (step == FSM.Step.Enter)
            {
                _animator.CrossFade(_attackHash, 0f);

                var direction = _inputMouse.mousePos - _unit.transform.position;
                _unit.Rotation(direction);
            }
            else if (step == FSM.Step.Update)
            {
                _unit.RunAgent(Vector3.zero);
                if (_animator.GetBool("Attack")) return;

                if (_inputMove != Vector2.zero)
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

        protected override void HitState(FSM fsm, FSM.Step step, FSM.State state)
        {
            if (step == FSM.Step.Enter)
            {
                _animator.CrossFade(_hitHash, 0f);
            }
            else if (step == FSM.Step.Update)
            {
                _unit.RunAgent(Vector3.zero);
                StartCoroutine(DelayFrameHit());
            }
            else
            {

            }
        }

        protected override void Hit()
        {
            if (_inputMove != Vector2.zero)
            {
                _fsm.TransitionTo(RunState);
            }
            else
            {
                _fsm.TransitionTo(IdleState);
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

        public override void OnHit()
        {
            if (_animator.GetBool("Attack")) return;
            base.OnHit();
        }
    }
}
