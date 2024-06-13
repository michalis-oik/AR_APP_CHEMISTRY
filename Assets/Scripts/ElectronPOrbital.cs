using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronPOrbital : MonoBehaviour
{
    public float amplitude = 1.0f;
    public float frequency = 1.0f;
    public Vector3 axis = Vector3.right; // x, y, or z

    private Vector3 center;

    void Start()
    {
        center = transform.position;
    }

    void Update()
    {
        float angle = frequency * Time.time;
        Vector3 offset = amplitude * Mathf.Sin(angle) * axis;
        transform.position = center + offset;
    }
}
