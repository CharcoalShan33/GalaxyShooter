using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    private AudioSource _audioSource;
   
    private void Awake()
    {
        _audioSource = GetComponentInChildren<AudioSource>();

    }

    private void Start()
    {
       
        if(_audioSource == null)
        {
            Debug.LogError("Get the Component");
        }
    }


    public void PlayClip(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
