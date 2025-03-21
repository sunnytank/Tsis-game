using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static AudioClip BgSound;
    static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Time.timeScale == 1f)
        {
            audioSrc.Play();
        }
        else
        {
            audioSrc.Stop();
        }*/
    }

}
