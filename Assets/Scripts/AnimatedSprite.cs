using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private int frame;

    public float acceleration = 0.1f;
    private float timeElapsed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        timeElapsed = 0f;
        Invoke(nameof(Animate), 0f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Animate()
    {
        frame++;

        if (frame >= sprites.Length)
        {
            frame = 0;
        }

        if (frame >= 0 && frame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[frame];
        }

        float currentSpeed = GameManager.Instance.gameSpeed + (timeElapsed * acceleration);
        Invoke(nameof(Animate), 1f / currentSpeed);
    }
    private void Update()
    {
        timeElapsed += Time.deltaTime;
    }
}
