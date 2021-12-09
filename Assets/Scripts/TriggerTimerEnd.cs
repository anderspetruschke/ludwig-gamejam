using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTimerEnd : MonoBehaviour
{
    [SerializeField] private Timer timer;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timer.StopTimer();
        }
    }
}
