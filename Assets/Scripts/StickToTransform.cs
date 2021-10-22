using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StickToTransform : MonoBehaviour
{
    [FormerlySerializedAs("playerTransform")] [SerializeField] private Transform otherTransform;
    [SerializeField] private bool invertRotation;
    [SerializeField] private float dampenRotation = 0.5f;
    [SerializeField] private bool parallax;
    [SerializeField] private float parallaxFactor = 0.9f;
    
    private Vector3 _offset;

    private void Start()
    {
        _offset = transform.position - otherTransform.position;
    }

    void Update()
    {
        var targetPosition = otherTransform.position;

        if (parallax)
        {
            targetPosition = Vector3.Lerp(Vector3.zero, targetPosition, parallaxFactor);
        }
        
        transform.position = targetPosition + _offset;

        if (invertRotation)
        {
            transform.rotation = Quaternion.Lerp(otherTransform.rotation, Quaternion.identity, dampenRotation);
        }
    }
}
