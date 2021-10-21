using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StickToTransform : MonoBehaviour
{
    [FormerlySerializedAs("playerTransform")] [SerializeField] private Transform otherTransform;

    private Vector3 _offset;

    private void Start()
    {
        _offset = transform.position - otherTransform.position;
    }

    void Update()
    {
        transform.position = otherTransform.position + _offset;
    }
}
