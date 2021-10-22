using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BoxCollider2D _boxCollider;
    
    private void Start()
    {
        if(_boxCollider == null || _spriteRenderer == null) return;
        var newSize = _spriteRenderer.size;
        newSize.y = 0.01f;
        newSize.x -= 0.5f;
        _boxCollider.size = newSize;
    }
}
