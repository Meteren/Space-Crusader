using UnityEngine;

public class Heart : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void GetHeart()
    {
        animator.SetBool("get", true);
        animator.SetBool("release", false);
    }
    public void ReleaseHeart()
    {
        animator.SetBool("get", false);
        animator.SetBool("release", true);
    }
}
