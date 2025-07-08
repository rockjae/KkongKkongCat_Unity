using UnityEngine;
using UnityEngine.Audio;

public class FootBgmManager : MonoBehaviour
{
    private AudioSource audioSource; 
    
    [Header("Pitch Settings")]
    private float pitchSpeed = 0.01f; // Pitch ���� �ӵ�
    private float maxPitch = 5.0f;    // Pitch �ִ밪
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // �ð��� ������ ���� pitch�� �������� ���� �ӵ��� ������ ��
        if (audioSource.isPlaying && audioSource.pitch < maxPitch)
        {
            audioSource.pitch += Time.deltaTime * pitchSpeed;
        }
    }
}
