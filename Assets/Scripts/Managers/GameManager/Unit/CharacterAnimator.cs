using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // 0: idle, 1: move, 2: other
    public int State => animator.GetInteger("State");
    public void UpdateState(int state)
    {
        if (animator.GetInteger("State") != state) animator.SetInteger("State", state);
    }

    public void UpdateDir(int dir)
    {
        if (animator.GetInteger("Dir") != dir) animator.SetInteger("Dir", dir);
    }

    public void TriggerAttack()
    {
        UpdateState(2);
        animator.SetTrigger("Attack");
    }
}
