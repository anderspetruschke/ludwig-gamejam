using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float followSpeed = 0.08f;
    [SerializeField] private Rigidbody2D ballRigidBody;
    [SerializeField] private float rotationAngle = 35;
    [SerializeField] private float rotationSpeed = 0.08f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private List<Rigidbody2D> applyGravity;
    [SerializeField] private float angularForce = 10f;

    private bool _leftPressed;
    private bool _rightPressed;
    
    private void FixedUpdate()
    {
        //Camera Rotation
        var cameraRotation = cameraTransform.rotation;
        
        var horizontalInput = Input.GetAxis("Horizontal") * rotationAngle;

        if (_leftPressed && !_rightPressed)
        {
            horizontalInput = -rotationAngle;
        }

        if (_rightPressed && !_leftPressed)
        {
            horizontalInput = rotationAngle;
        }
        
        var currentRotationAngle = cameraRotation.eulerAngles.z;
        var rotationTarget = Quaternion.Euler(new Vector3(0, 0, horizontalInput));

        cameraRotation = Quaternion.Lerp(cameraRotation, rotationTarget, rotationSpeed);
        cameraTransform.rotation = cameraRotation;
        
        //Camera Position
        var targetCameraPosition = playerTransform.position + cameraOffset;
        targetCameraPosition.z = cameraTransform.position.z;
        
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetCameraPosition, followSpeed);
        
        //Camera Zoom
        var playerSpeed = Mathf.Abs(ballRigidBody.velocity.x);
        mainCamera.orthographicSize = Mathf.Clamp(Mathf.Lerp(mainCamera.orthographicSize, playerSpeed / 2f + 4f, 0.01f), 4f, 10f);
        
        //Apply gravity
        var force = cameraTransform.up * gravity;
        ballRigidBody.AddForce(force);

        foreach (var body in applyGravity)
        {
            body.AddForce(force);
        }
        
        //Angular Velocity
        ballRigidBody.angularVelocity += horizontalInput * -angularForce;
    }

    public void PressLeft()
    {
        _leftPressed = true;
    }
    
    public void ReleaseLeft()
    {
        _leftPressed = false;
    }
    
    public void PressRight()
    {
        _rightPressed = true;
    }
    
    public void ReleaseRight()
    {
        _rightPressed = false;
    }
}
