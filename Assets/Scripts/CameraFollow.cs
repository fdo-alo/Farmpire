using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Vector3 camOffset;

    private void Start()
    {
        camOffset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        transform.position = target.position + camOffset;
    }
}
