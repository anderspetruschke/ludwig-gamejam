using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownIndicator : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private RectTransform shadow;

    [SerializeField] private Vector2 shadowOffset;
    

    private void FixedUpdate()
    {
        var inverseRotation = Quaternion.Inverse(cameraTransform.rotation);
        transform.rotation = inverseRotation;
        shadow.transform.rotation = inverseRotation;
        shadow.transform.localPosition = transform.localPosition + (Vector3) shadowOffset;
    }
}
