using UnityEngine;

public enum PlayerStateType
{
    Idle,
    Run,
    Hit,
    Death,
    Attack,
}

public class BasicBehaviour : StateMachineBehaviour
{
    public PlayerStateType type;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch(type)
        {
            case PlayerStateType.Idle:
                animator.SetBool("Idle", true);
                break;
            case PlayerStateType.Run:
                animator.SetBool("Run", true);
                break;
            case PlayerStateType.Hit:
                animator.SetBool("Hit", true);
                break;
            case PlayerStateType.Death:
                animator.SetBool("Death", true);
                break;
            case PlayerStateType.Attack:
                animator.SetBool("Attack", true);
                break;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (type)
        {
            case PlayerStateType.Idle:
                animator.SetBool("Idle", false);
                break;
            case PlayerStateType.Run:
                animator.SetBool("Run", false);
                break;
            case PlayerStateType.Hit:
                animator.SetBool("Hit", false);
                break;
            case PlayerStateType.Death:
                animator.SetBool("Death", false);
                break;
            case PlayerStateType.Attack:
                animator.SetBool("Attack", false);
                break;
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
