using UnityEngine;

namespace Scripts.StateMachine
{
    public class BasicMonsterAI : MonsterAI
    {
        protected bool _onDistanceInReadyPattern = false;

        private void TryPatternState(FSM.Step step, int animHash, int patternNum)
        {
            if (_animator.GetInteger("Pattern") > 0) return;
            if (step == FSM.Step.Enter)
            {
                _animator.CrossFade(_runHash, 0f);
                _unit.Emotion(EmotionType.Anger);
            }
            else if (step == FSM.Step.Update)
            {
                if (_onDistanceInReadyPattern && _animator.GetInteger("Pattern") != patternNum)
                {
                    _readyPattern.ResetDelay();

                    _onDistanceInReadyPattern = false;
                    TransitionToRunOrIdle();

                    return;
                }

                if (_readyPattern.IsDelaying) return;

                if (!_onDistanceInReadyPattern)
                {
                    float distance = GetDistance();
                    if (distance >= chaseDistance)
                    {
                        TryTransitionTo(IdleState);
                        _onDistanceInReadyPattern = true;
                        return;
                    }

                    if (distance <= _readyPattern.distance)
                    {
                        _animator.CrossFade(animHash, 0f);
                        _unit.Stop(GetDirection().normalized, false);

                        _onDistanceInReadyPattern = true;
                    }
                    else
                    {
                        _unit.RunAgentToTarget(Player.s_player.transform);
                        _unit.Rotation(GetDirection().normalized);
                    }
                }
            }
            else
            {

            }
        }

        protected override void Pattern1State(FSM fsm, FSM.Step step, FSM.State state) 
        {
            TryPatternState(step, _pattern1Hash, 1);
        }

        protected override void Pattern2State(FSM fsm, FSM.Step step, FSM.State state)
        {
            TryPatternState(step, _pattern2Hash, 2);
        }

        protected override void Pattern3State(FSM fsm, FSM.Step step, FSM.State state)
        {
            TryPatternState(step, _pattern3Hash, 3);
        }

        protected override void Pattern4State(FSM fsm, FSM.Step step, FSM.State state)
        {
            TryPatternState(step, _pattern4Hash, 4);
        }
    }
}
