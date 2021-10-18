using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
        
    void Update()
    {
        transform.position = playerTransform.position;
    }
}
