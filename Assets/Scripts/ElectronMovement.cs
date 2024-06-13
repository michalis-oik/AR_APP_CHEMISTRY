using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronMovement : MonoBehaviour
{
    [SerializeField]
    private float radius = 1.0f;
    
    [SerializeField]
    private float speed = 1.0f;

    private Vector3 center;

    void Start()
    {
        center = transform.position;
    }

    void Update()
    {
        float angle = speed * Time.time;
        float x = center.x + radius * Mathf.Cos(angle);
        float y = center.y + radius * Mathf.Sin(angle);
        float z = center.z + radius * Mathf.Sin(angle / 2); // Adds 3D movement

        transform.position = new Vector3(x, y, z);
    }
}
