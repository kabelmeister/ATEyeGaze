using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryMusic : MonoBehaviour
{
    private static AudioSource _audioSource;

    private void Awake()
    {
        if (_audioSource != null)
        {
            return;
        }
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;

        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}

// GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().StopMusic();
