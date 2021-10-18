using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D playerRigidBody;

    [SerializeField] private Rigidbody2D thisRigidBody;
    [SerializeField] private bool rotate;
    
    private void Update()
    {
        thisRigidBody.velocity =  (Vector2) (playerTransform.position - transform.position) * 50f + playerRigidBody.velocity;
        if(rotate)
            thisRigidBody.angularVelocity = playerRigidBody.angularVelocity;
    }
}
