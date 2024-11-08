using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public Animator animator;

    public void OnAttack(bool isCritical)
    {
        if(isCritical)
        {
            animator.CrossFade("Critical", 0);
            return;
        }

        int random = Random.Range(0, 2);
        animator.CrossFade($"State {random + 1}", 0);
    }
}
