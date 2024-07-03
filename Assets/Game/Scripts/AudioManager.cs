using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Audios
{
    Explode,
    Collect
}

public class AudioManager : MonoBehaviour
{
    [SerializeField]private AudioClip explodeSound;
    [SerializeField] private AudioClip collectSound;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayAudio(Audios audios)
    {
        audioSource.clip = GetAudioFromEnum(audios);
        audioSource.Play();
    }

    private AudioClip GetAudioFromEnum(Audios audios)
    {
        switch (audios)
        {
            case Audios.Explode:
                return explodeSound;
            default:
                return null;
        }
    }
}
