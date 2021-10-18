using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D playerRigidBody;

    [SerializeField] private Rigidbody2D thisRigidBody;
    
    private void Update()
    {
        thisRigidBody.velocity =  (Vector2) (playerTransform.position - transform.position) * 100f + playerRigidBody.velocity;
        thisRigidBody.angularVelocity = playerRigidBody.angularVelocity;
    }
}
