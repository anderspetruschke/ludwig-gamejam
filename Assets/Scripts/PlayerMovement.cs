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
    public float cameraRotationFactor = 0.5f;

    private bool _leftPressed;
    private bool _rightPressed;
    [SerializeField] private bool cameraRotationOn;

    private Quaternion _worldRotation;

    private Vector2 _lastVelocity;

    [Header("Effects")] [SerializeField] private float impactThreshold = 5f;
    [SerializeField] private Transform cameraParent;
    [SerializeField] private ParticleSystem groundParticleSystem;
    [SerializeField] private Transform groundParticleTransform;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private AudioSource groundSource;
    
    public bool _activateTrail;

    private bool _overrideCameraSize;
    private float _cameraSizeOverridden;
    private Vector3 _cameraOffsetOverridden;

    private bool _onGround;
    
    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Application.targetFrameRate = 60;
        }

        _worldRotation = Quaternion.identity;
        _lastVelocity = new Vector2();
    }

    private void FixedUpdate()
    {
        if (ballRigidBody.velocity.magnitude < 7f)
        {
            _activateTrail = false;
        }
        
        //Camera Rotation
        var horizontalInput = Input.GetAxis("Horizontal") * rotationAngle;

        if (_leftPressed && !_rightPressed)
        {
            horizontalInput = -rotationAngle;
        }

        if (_rightPressed && !_leftPressed)
        {
            horizontalInput = rotationAngle;
        }

        var currentRotationAngle = _worldRotation.eulerAngles.z;
        var rotationTarget = Quaternion.Euler(new Vector3(0, 0, horizontalInput));

        _worldRotation = Quaternion.Lerp(_worldRotation, rotationTarget, rotationSpeed);

        if (cameraRotationOn)
        {
            cameraTransform.rotation = Quaternion.Lerp(Quaternion.identity, _worldRotation, cameraRotationFactor);
        }

        //Camera Position
        var targetCameraPosition = playerTransform.position + cameraOffset;
        targetCameraPosition.z = cameraTransform.position.z;
        
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetCameraPosition, followSpeed);


        //Camera Zoom
        var playerSpeed = Mathf.Abs(ballRigidBody.velocity.x);
        if (_overrideCameraSize)
        {
            mainCamera.orthographicSize =
                Mathf.Clamp(Mathf.Lerp(mainCamera.orthographicSize, _cameraSizeOverridden, 0.01f), 4f, 10f);
        }
        else
        {
            mainCamera.orthographicSize =
                Mathf.Clamp(Mathf.Lerp(mainCamera.orthographicSize, playerSpeed / 2f + 4f, 0.01f), 4f, 10f);
        }
        

        //Apply gravity
        var force = _worldRotation * Vector3.up * gravity;
        ballRigidBody.AddForce(force);

        foreach (var body in applyGravity)
        {
            body.AddForce(force);
        }

        //Angular Velocity
        ballRigidBody.angularVelocity += horizontalInput * -angularForce;

        //Effects
        var emissionModule = groundParticleSystem.emission;

        
        groundSource.volume = Mathf.Lerp(groundSource.volume ,Mathf.Lerp(0f, 0.4f, (ballRigidBody.velocity.magnitude / 12f) - 0.6f), 0.05f);

        emissionModule.enabled = false;
        _onGround = false;
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

    public Quaternion GetWorldRotation()
    {
        return _worldRotation;
    }

    public bool GetCameraRotationOn()
    {
        return cameraRotationOn;
    }

    public void SetCameraRotation(bool value)
    {
        cameraRotationOn = value;

        if (!cameraRotationOn)
        {
            cameraTransform.rotation = Quaternion.identity;
        }
    }

    private IEnumerator CameraShake(float duration, float magnitude)
    {
        StopAllCoroutines();
        cameraParent.position = Vector3.zero;

        var elapsed = 0.0f;

        while (elapsed < duration)
        {
            var x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            var y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            cameraParent.localPosition = new Vector3(x, y, 0f);
            elapsed += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        cameraParent.position = Vector3.zero;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        _onGround = true;
        
        if (ballRigidBody.velocity.magnitude > 6f)
        {
            var emissionModule = groundParticleSystem.emission;
            emissionModule.enabled = true;
            groundParticleTransform.position = other.contacts[0].point;
            _onGround = true;
        }
        else
        {
            var emissionModule = groundParticleSystem.emission;
            emissionModule.enabled = false;
        }

        if (Mathf.Abs(ballRigidBody.velocity.x) > 10f)
        {
            _activateTrail = true;
        }

        trail.emitting = _activateTrail;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (ballRigidBody.velocity.magnitude < 5f)
        {
            trail.emitting = _activateTrail;
        }

        var dot = Vector3.Dot(other.contacts[0].normal, other.relativeVelocity);
        var value = Mathf.Clamp((dot / 8f) - 0.4f, 0f, 0.6f);

        StartCoroutine(CameraShake(Mathf.Lerp(0f, 0.5f, value),
            Mathf.Lerp(0f, 0.15f, value)));


        if (other.contacts[0].collider.CompareTag("Jump"))
        {
            SoundManager.PlaySound("Jump", 0.3f * value, 1.5f, 0.1f);
        }
        else
        {
            SoundManager.PlaySound("Hurt", value, 1f, 0.2f);
        }
        

        if (value > 0)
        {
            _activateTrail = false;
            trail.emitting = false;
        }
        
    }

    public void OverrideCameraSize(float size, Vector3 offset)
    {
        _overrideCameraSize = true;
        _cameraSizeOverridden = size;
        _cameraOffsetOverridden = offset;
    }

    public void StopOverridingCamera()
    {
        _overrideCameraSize = false;
    }
}
