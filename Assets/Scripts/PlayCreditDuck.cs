using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCreditDuck : MonoBehaviour
{
    public AudioSource _audioSource;
    public AudioClip _audioClip;


    private void Start()
    {
       // _audioSource = GetComponent<AudioSource>();
    }

    public void playQuack()
    {
        Debug.Log("dio");
        _audioSource.clip = _audioClip;
        _audioSource.Play();
    }
}
