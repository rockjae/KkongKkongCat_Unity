using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    int count = 0;

    private CharacterController character;
    private Vector3 direction;

    public float jumpForce = 8f;
    public float gravity = 9.81f * 2f;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
    }

    private void Update()
    {
        direction += gravity * Time.deltaTime * Vector3.down;

        if (character.isGrounded)
        {
            direction = Vector3.down;

            if (Input.GetButton("Jump") || Input.GetMouseButtonDown(0)) {
                direction = Vector3.up * jumpForce;
                playerJumpSound();
            }
        }

        character.Move(direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle")) {
            GameManager.Instance.GameOver();
        }
    }

    void playerJumpSound()
    {
        audioSource.clip = audioClips[Random.Range(0,audioClips.Length)];
        audioSource.Play();
    }

}
