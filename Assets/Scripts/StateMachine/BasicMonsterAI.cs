using Scripts;
using UnityEngine;
using UnityEngine.Windows;

public class BasicMonsterAI : MonsterAI
{
    protected override void Pattern1State(FSM fsm, FSM.Step step, FSM.State state) 
    {
        if (_animator.GetInteger("Pattern") > 0) return;
        if (step == FSM.Step.Enter)
        {
            _animator.CrossFade(_pattern1Hash, 0f);
        }
        else if (step == FSM.Step.Update)
        {
            if (_animator.GetInteger("Pattern") != 1)
            {
                TransitionToRunOrIdle();
            }
        }
        else
        {

        }
    }

    protected override void Pattern2State(FSM fsm, FSM.Step step, FSM.State state)
    {
        if (_animator.GetInteger("Pattern") > 0) return;
        if (step == FSM.Step.Enter)
        {
            _animator.CrossFade(_pattern2Hash, 0f);
        }
        else if (step == FSM.Step.Update)
        {
            if (_animator.GetInteger("Pattern") != 2)
            {
                TransitionToRunOrIdle();
            }
        }
        else
        {

        }
    }

    protected override void Pattern3State(FSM fsm, FSM.Step step, FSM.State state)
    {
        if (_animator.GetInteger("Pattern") > 0) return;
        if (step == FSM.Step.Enter)
        {
            _animator.CrossFade(_pattern3Hash, 0f);
        }
        else if (step == FSM.Step.Update)
        {
            if (_animator.GetInteger("Pattern") != 3)
            {
                TransitionToRunOrIdle();
            }
        }
        else
        {

        }
    }

    protected override void Pattern4State(FSM fsm, FSM.Step step, FSM.State state)
    {
        if (_animator.GetInteger("Pattern") > 0) return;
        if (step == FSM.Step.Enter)
        {
            _animator.CrossFade(_pattern4Hash, 0f);
        }
        else if (step == FSM.Step.Update)
        {
            if (_animator.GetInteger("Pattern") != 4)
            {
                TransitionToRunOrIdle();
            }
        }
        else
        {

        }
    }
}
