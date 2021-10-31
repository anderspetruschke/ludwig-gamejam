using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider.value = AudioListener.volume;
    }

    public void ChangeVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
