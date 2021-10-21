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
    
    private Vector3 _offset;

    private void Start()
    {
        _offset = transform.position - otherTransform.position;
    }

    void Update()
    {
        transform.position = otherTransform.position + _offset;

        if (invertRotation)
        {
            transform.rotation = Quaternion.Lerp(otherTransform.rotation, Quaternion.identity, dampenRotation);
        }
    }
}
