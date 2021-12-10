using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TriggerTimerEnd : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private AudioSource sound;
    
    private bool _wasTriggered;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_wasTriggered) return;
        
        if (other.CompareTag("Player"))
        {
            timer.StopTimer();
            sound.Play();
            _wasTriggered = true;
            transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 2, 1f);
        }
    }
}
