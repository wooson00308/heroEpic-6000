using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using System.Collections;

namespace Scripts.StateMachine
{
    [Serializable]
    public class PatternCondition
    {
        private float _currentDelayTime;
        public bool IsDelaying => _currentDelayTime > 0;

        public int partternNumber;

        [Header("Config")]
        public float distance;
        public float minDelay;
        public float maxDelay;

        public int StateNameToHash => Animator.StringToHash($"Pattern {partternNumber}");

        public void ResetDelay()
        {
            _currentDelayTime = UnityEngine.Random.Range(minDelay, maxDelay);
        }

        public void Tick()
        {
            if (!IsDelaying)
            {
                _currentDelayTime = 0;
                return;
            }

            _currentDelayTime -= Time.deltaTime;
        }
    }

    public class MonsterAI : BaseStateMachine
    {
        protected FSM.State _pattern1State;
        protected FSM.State _pattern2State;
        protected FSM.State _pattern3State;
        protected FSM.State _pattern4State;

        protected readonly int _pattern1Hash = Animator.StringToHash("Pattern 1");
        protected readonly int _pattern2Hash = Animator.StringToHash("Pattern 2");
        protected readonly int _pattern3Hash = Animator.StringToHash("Pattern 3");
        protected readonly int _pattern4Hash = Animator.StringToHash("Pattern 4");

        public float chaseDistance;
        public float minChaseDistance;
        public float chaseBufferRange;
        public List<PatternCondition> patternConditions;

        protected PatternCondition _readyPattern = null;
        protected float _readyPatternDistance = 999;

        protected override void SetState()
        {
            base.SetState();

            _pattern1State = Pattern1State;
            _pattern2State = Pattern2State;
            _pattern3State = Pattern3State;
            _pattern4State = Pattern4State;
        }

        public override void OnUpdate()
        {
            if (_animator.GetInteger("Pattern") == 0)
            {
                foreach (var patternCondition in patternConditions)
                {
                    patternCondition.Tick();
                }
            }

            base.OnUpdate();
        }

        protected virtual void Pattern1State(FSM fsm, FSM.Step step, FSM.State state) { }
        protected virtual void Pattern2State(FSM fsm, FSM.Step step, FSM.State state) { }
        protected virtual void Pattern3State(FSM fsm, FSM.Step step, FSM.State state) { }
        protected virtual void Pattern4State(FSM fsm, FSM.Step step, FSM.State state) { }

        /// <summary>
        /// 플레이어와의 거리 float 제공
        /// </summary>
        /// <returns></returns>
        protected float GetDistance()
        {
            if (Player.s_player == null) return 999;
            return Vector2.Distance(Player.s_player.transform.position, _unit.transform.position);
        }

        /// <summary>
        /// 유닛이 플레이어 방향으로 가기위한 각도를 제공
        /// </summary>
        /// <returns></returns>
        protected Vector3 GetDirection()
        {
            if (Player.s_player == null) return Vector3.zero;
            return Player.s_player.transform.position - _unit.transform.position;
        }

        protected bool TryTransitionToPattern()
        {
            _readyPattern = null;
            
            foreach (var condition in patternConditions)
            {
                if (condition.IsDelaying) continue;

                _readyPattern = condition;
            }

            if (_readyPattern != null)
            {
                if (!_readyPattern.IsDelaying)
                {
                    int stateHash = _readyPattern.StateNameToHash;

                    if (stateHash == _pattern1Hash)
                    {
                        TryTransitionTo(Pattern1State);
                    }
                    else if (stateHash == _pattern2Hash)
                    {
                        TryTransitionTo(Pattern2State);
                    }
                    else if (stateHash == _pattern3Hash)
                    {
                        TryTransitionTo(Pattern3State);
                    }
                    else if (stateHash == _pattern4Hash)
                    {
                        TryTransitionTo(Pattern4State);
                    }

                    return true;
                }
            }

            return false;
        }

        protected void TransitionToRunOrIdle()
        {
            float distance = GetDistance();
            if (distance > chaseDistance && distance > minChaseDistance)
            {
                TryTransitionTo(IdleState);
            }
            else
            {
                TryTransitionTo(RunState);
            }
        }

        protected override void IdleState(FSM fsm, FSM.Step step, FSM.State state)
        {
            if (step == FSM.Step.Enter)
            {
                _animator.CrossFade(_idleHash, 0f);
            }
            else if (step == FSM.Step.Update)
            {
                Vector3 direction = GetDirection().normalized;
                _unit.Stop(direction);

                float distance = GetDistance();

                if (TryTransitionToPattern()) return;

                // 마진 범위 안에 있을 경우 Idle 상태 유지
                if (distance <= minChaseDistance + chaseBufferRange && 
                    distance >= minChaseDistance - chaseBufferRange)
                {
                    return; 
                }

                if (distance < chaseDistance && distance > minChaseDistance)
                {
                    TryTransitionTo(RunState);
                }
            }
            else
            {
                // 기타 처리
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
                if (TryTransitionToPattern()) return;

                Vector3 direction = GetDirection().normalized;
                float distance = GetDistance();

                // 마진 범위 안에 있을 경우 Idle 상태로 전환
                if (distance <= minChaseDistance + chaseBufferRange && 
                    distance >= minChaseDistance - chaseBufferRange)
                {
                    TryTransitionTo(IdleState);
                    return;
                }
                
                if (distance >= chaseDistance)
                {
                    TryTransitionTo(IdleState);
                }
                else if (distance <= minChaseDistance)
                {
                    _unit.BackMoveAgent(direction);
                }
                else if (distance > minChaseDistance)
                {
                    _unit.RunAgentToTarget(Player.s_player.transform);
                    _unit.Rotation(direction);
                }
            }
            else
            {
                // 기타 처리
            }
        }

        protected override void HitState(FSM fsm, FSM.Step step, FSM.State state)
        {
            if (step == FSM.Step.Enter)
            {
                _animator.CrossFade(_hitHash, 0f);
                _unit.Stop(Vector3.zero, false);
            }
            else if (step == FSM.Step.Update)
            {
                StartCoroutine(DelayFrameHit());
            }
            else
            {

            }
        }

        protected override void Hit()
        {
            TransitionToRunOrIdle();
        }

        protected override void DeathState(FSM fsm, FSM.Step step, FSM.State state)
        {
            if (step == FSM.Step.Enter)
            {
                _animator.CrossFade(_deathHash, 0f);
                _unit.Stop(Vector3.zero, false);
            }
            else if (step == FSM.Step.Update)
            {

            }
            else
            {

            }
        }

        protected override void TryTransitionTo(FSM.State state)
        {
            base.TryTransitionTo(state);
        }
    }
}
