using UnityEngine;

public class AnimationSpeedController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private float acceleration = 0.1f;

    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            animator.speed = 0;
        }
    }

    void Update()
    {
        animator.speed += acceleration * Time.deltaTime;
    }

    public void ResetAcceleration()
    {
        animator.speed = 0;
        acceleration = 0.1f;
    }
}