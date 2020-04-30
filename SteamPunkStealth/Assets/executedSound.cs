using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class executedSound : MonoBehaviour
{
    bool playSound = true;
    AudioSource source;
    public AudioClip deathSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf && playSound)
        {
            source.PlayOneShot(deathSound, 0.2f);
            
        }
    }
}
