using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource AsMusic, AsShot, AsDie, AsDieGob, AsReload;
    private void Start()
    {
        Play();   
    }

    private void Play()
    {
        AsMusic.Play();
    }
	
    
    void Update(){
    	if(PlayerHealth.PlayerIsDead)
	   AsMusic.Stop();
    }
    
    public void PlayShot()
    {
        AsShot.Play();
    }
 
    public void PlayDie()
    {
        AsDie.Play();
    }
    
    public void PlayDieGoblin()
    {
        AsDieGob.time = 0.5f;
        AsDieGob.Play();
    }
    
    public void PlayReload()
    {
        AsReload.Play();
    }

    public void MuteAudio()
    {
        bool Audio = AsDie.mute;
        AsDie.mute = !Audio;
        AsReload.mute = !Audio;
        AsShot.mute = !Audio;
        AsDieGob.mute = !Audio;
    }
    
    public void MuteMusic()
    {
        AsMusic.mute = !AsMusic.mute;
    }
}
