using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] private float force = 1000f;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _secondAudioSource;

    private bool _boostingPlayer;
    
    private void Start()
    {
        if(_boxCollider == null || _spriteRenderer == null) return;
        _boxCollider.size = _spriteRenderer.size;
    }

    private void FixedUpdate()
    {
        if (_boostingPlayer)
        {
            _audioSource.spatialBlend = Mathf.Lerp(_audioSource.spatialBlend, 0f, 0.05f);
            _secondAudioSource.volume = Mathf.Lerp(_secondAudioSource.volume, 0.07f, 0.1f);
        }
        else
        {
            _audioSource.spatialBlend = Mathf.Lerp(_audioSource.spatialBlend, 1f, 0.05f);
            _secondAudioSource.volume = Mathf.Lerp(_secondAudioSource.volume, 0f, 0.1f);
        }
        
        _boostingPlayer = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var rigidBody = other.GetComponent<Rigidbody2D>();
        if (rigidBody != null)
        {
            rigidBody.AddForce(transform.up * force * Time.deltaTime);
        }

        if (other.CompareTag("Player"))
        {
            _boostingPlayer = true;
        }
    }
}
