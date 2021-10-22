using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private List<Vector3> positions;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private float pauseTime = 2f;
    
    private int _index;
    private bool _paused;
    
    private void Start()
    {
        positions.Add(transform.position);
        _index = 0;
    }

    private void FixedUpdate()
    {
        if(_paused) return;
        
        transform.position = Vector3.MoveTowards(transform.position, positions[_index], speed);

        if (Vector3.Distance(transform.position, positions[_index]) < 0.1f)
        {
            _index = (_index + 1) % positions.Count;
            _paused = true;
            Invoke(nameof(UnPause), pauseTime);
        }
    }

    private void UnPause()
    {
        _paused = false;
    }

    [ContextMenu("AddPosition")]
    public void AddPosition()
    {
        positions.Add(transform.position);
    }
}
