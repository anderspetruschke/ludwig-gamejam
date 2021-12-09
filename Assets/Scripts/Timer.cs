using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    
    private DateTime _startedAt;
    private DateTime _finishedAt;

    private bool _started = false;

    private void Update()
    {
        if(!_started) return;

        timeText.text = (DateTime.Now - _startedAt).Duration().ToString(@"mm\:ss\.ff");
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
        timeText.text = (DateTime.Now - _startedAt).Duration().ToString(@"mm\:ss\.ff");
    }
}
