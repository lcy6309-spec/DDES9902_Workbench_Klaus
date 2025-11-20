using UnityEngine;

public class FireSoundController : MonoBehaviour
{
    public AudioSource fireAudio; 
    public AudioClip fireClip;    

    private bool isPlaying = false; 

    public void ToggleFireSound()
    {
        if (isPlaying)
        {
            fireAudio.Stop();
            isPlaying = false;
        }
        else
        {
            fireAudio.clip = fireClip;
            fireAudio.Play();
            isPlaying = true;
        }
    }
}

