using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource audioSource;

    [Header("Pitch Settings")]
    private float pitchSpeed = 0.01f; // Pitch 증가 속도
    private float maxPitch = 5.0f;    // Pitch 최대값

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 시간이 지남에 따라 pitch를 증가시켜 음악 속도를 빠르게 함
        if (audioSource.isPlaying && audioSource.pitch < maxPitch)
        {
            audioSource.pitch += Time.deltaTime * pitchSpeed;
        }
    }

    public void NewAudioPlay()
    {
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.pitch = 1f; // 새 음악이 재생될 때 pitch를 1로 초기화
        audioSource.Play();
    }
}
