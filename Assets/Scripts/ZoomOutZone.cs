using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOutZone : MonoBehaviour
{
    [SerializeField] private float cameraSize = 10f;
    [SerializeField] private Vector3 cameraOffset;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.OverrideCameraSize(cameraSize, cameraOffset);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.StopOverridingCamera();
        }
    }
}
