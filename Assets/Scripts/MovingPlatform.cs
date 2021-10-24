using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private List<Vector3> positions;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private float pauseTime = 2f;
    [SerializeField] private Rigidbody2D rigidBody;
    
    private int _index;
    private bool _paused;
    
    
    private void Start()
    {
        transform.position = positions[0];
        _index = 0;
    }

    private void FixedUpdate()
    {
        if(_paused) return;

        var direction = positions[_index] - transform.position;
        var force = direction.normalized * Mathf.Min(direction.magnitude * 2f, speed);

        force =  Vector2.Lerp(rigidBody.velocity, force, 0.08f);

            rigidBody.velocity = force;
        

        if (Vector3.Distance(transform.position, positions[_index]) < 0.05f)
        {
            _index = (_index + 1) % positions.Count;
            _paused = true;
            Invoke(nameof(UnPause), pauseTime);
            rigidBody.velocity = Vector2.zero;
        }
    }

    private void UnPause()
    {
        _paused = false;
    }

    [ContextMenu("AddPosition")]
    public void AddPosition()
    {
        positions ??= new List<Vector3>();

        positions.Add(transform.position);
    }
}
