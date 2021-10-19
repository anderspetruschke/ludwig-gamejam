using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownIndicator : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private RectTransform shadow;

    [SerializeField] private Vector2 shadowOffset;

    private void FixedUpdate()
    {
        var rotation = Quaternion.Lerp(playerMovement.GetWorldRotation(), Quaternion.identity, playerMovement.cameraRotationFactor);
        transform.rotation = rotation;
        shadow.transform.rotation = rotation;
        shadow.transform.localPosition = transform.localPosition + (Vector3) shadowOffset;
    }
}
