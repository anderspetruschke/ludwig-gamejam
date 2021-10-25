using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

public class Coin : MonoBehaviour
{
    public static UnityAction OnCoinCollected;

    [SerializeField] private Light2D light;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private AudioSource source;
    private SpriteRenderer _renderer;
    private bool _collected = false;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_collected) return;
        
        if (other.CompareTag("Player"))
        {
            OnCoinCollected?.Invoke();
            var color = Color.grey;
            color.a = 0.1f;

            light.enabled = false;
            _renderer.color = color;
            particles.Stop();
            _collected = true;
            source.Play();
        }
    }
}
