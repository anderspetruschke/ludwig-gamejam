using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCollider : MonoBehaviour
{
    [SerializeField] private Collider2D collider;

    private void Awake()
    {
        collider.enabled = true;
    }
}
