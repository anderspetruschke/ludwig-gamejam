using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] private float force = 1000f;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BoxCollider2D _boxCollider;

    private void Start()
    {
        if(_boxCollider == null || _spriteRenderer == null) return;
        _boxCollider.size = _spriteRenderer.size;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var rigidBody = other.GetComponent<Rigidbody2D>();
        if (rigidBody != null)
        {
            rigidBody.AddForce(transform.up * force * Time.deltaTime);
        }
    }
}
