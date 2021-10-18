using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] private float force = 1000f;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        var rigidBody = other.GetComponent<Rigidbody2D>();
        if (rigidBody != null)
        {
            rigidBody.AddForce(transform.right * force * Time.deltaTime);
        }
    }
}
