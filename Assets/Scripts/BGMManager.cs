using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] audioClips;
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

    public void NewAudioPlay()
    {
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.pitch = 1f; // �� ������ ����� �� pitch�� 1�� �ʱ�ȭ
        audioSource.Play();
    }
}
