using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public void UpdateState(int state)
    {
        animator.SetInteger("State", state);
    }
    public void UpdateDir(int dir)
    {
        animator.SetInteger("Dir", dir);
    }

    public void UpdateSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }
}
