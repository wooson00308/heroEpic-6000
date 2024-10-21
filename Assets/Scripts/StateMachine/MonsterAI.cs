using System;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using Scripts.StateMachine;

[Serializable]
public class PatternCondition
{
    private float _currentDelayTime;
    public bool IsDelaying => _currentDelayTime > 0;

    public int partternNumber;

    [Header("Config")]
    public float distance;
    public float delay;

    public int StateNameToHash => Animator.StringToHash($"Pattern {partternNumber}");

    public void StartDelay()
    {
        _currentDelayTime = delay;
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
    public List<PatternCondition> patternConditions;

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
        foreach (var patternCondition in patternConditions)
        {
            patternCondition.Tick();
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
        int stateHash = 0;
        float maxDistance = 0;
        PatternCondition readyPattern = null;
        foreach (var condition in patternConditions)
        {
            if (maxDistance >= condition.distance) continue;

            readyPattern = condition;
        }

        if (readyPattern != null)
        {
            if (!readyPattern.IsDelaying)
            {
                stateHash = readyPattern.StateNameToHash;
                maxDistance = readyPattern.distance;

                float distance = GetDistance();
                if (distance <= maxDistance)
                {
                    readyPattern.StartDelay();

                    if (stateHash == _pattern1Hash)
                    {
                        _fsm.TransitionTo(Pattern1State);
                    }
                    else if (stateHash == _pattern2Hash)
                    {
                        _fsm.TransitionTo(Pattern2State);
                    }
                    else if (stateHash == _pattern3Hash)
                    {
                        _fsm.TransitionTo(Pattern3State);
                    }
                    else if (stateHash == _pattern4Hash)
                    {
                        _fsm.TransitionTo(Pattern4State);
                    }

                    return true;
                }
            }
        }

        return false;
    }

    protected void TransitionToRunOrIdle()
    {
        float distance = GetDistance();
        if (distance > chaseDistance && distance > minChaseDistance)
        {
            _fsm.TransitionTo(IdleState);
        }
        else
        {
            _fsm.TransitionTo(RunState);
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
            float distance = GetDistance();

            if (TryTransitionToPattern()) return;

            if (distance < chaseDistance && distance > minChaseDistance)
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
            Vector3 direction = GetDirection().normalized;
            _unit.Run(direction);

            float distance = GetDistance();

            if (TryTransitionToPattern()) return;

            if (distance >= chaseDistance || distance <= minChaseDistance)
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

            TransitionToRunOrIdle();
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
