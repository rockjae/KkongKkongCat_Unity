using UnityEngine;

public class AnimationSpeedController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float acceleration = 0.1f;

    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        animator.speed += acceleration * Time.deltaTime;
    }
}