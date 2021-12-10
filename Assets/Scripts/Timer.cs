using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] timeTexts;

    private DateTime _startedAt;
    private DateTime _finishedAt;

    private bool _started = false;

    private void Update()
    {
        if(!_started) return;

        foreach (var timeText in timeTexts)
        {
            timeText.text = (DateTime.Now - _startedAt).Duration().ToString(@"mm\:ss\.ff");
        }
        
    }
    
    public void StartTimer()
    {
        _startedAt = DateTime.Now;
        _started = true;
    }

    public void StopTimer()
    {
        if(!_started) return;
        
        _started = false;
        _finishedAt = DateTime.Now;
        foreach (var timeText in timeTexts)
        {
            timeText.text = (DateTime.Now - _startedAt).Duration().ToString(@"mm\:ss\.ff");
        }
    }
}
