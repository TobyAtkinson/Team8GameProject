using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void PlayOnce(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.PlayOneShot(s.source.clip);
    }



    public void StopPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

       

        s.source.Stop();
    }

    public void LoopSound(string name, bool loop)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.loop = loop;

    }

    public bool hasFinishedPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source.isPlaying)
            return false;
        return true;
    }

}
