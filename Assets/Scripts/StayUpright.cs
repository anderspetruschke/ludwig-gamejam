using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayUpright : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Rigidbody2D rigidBody;
    private void FixedUpdate()
    {
        var rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 0.1f);
        
        rigidBody.transform.rotation = rotation;
    }
}
