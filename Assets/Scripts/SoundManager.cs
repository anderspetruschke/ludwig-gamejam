using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    
    [SerializeField] private AudioSource source;
    [SerializeField] private List<Sound> sounds;

    private void Start()
    {
        instance = this;
    }

    public static void PlaySound(string name, float volume = 1f, float pitch = 1f, float pitchRange = 0)
    {
        foreach (var sound in instance.sounds)
        {
            if (sound.name == name)
            {
                instance.source.pitch = UnityEngine.Random.Range(pitch-pitchRange, pitch+pitchRange);
                instance.source.PlayOneShot(sound.clip, volume);
                return;
            }
        }
    }
}

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}