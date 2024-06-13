using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronPOrbital : MonoBehaviour
{
    public float amplitude = 0.2f;
    public float frequency = 1.0f;
    public Vector3 axis = Vector3.right; // x, y, or z axis

    private Vector3 center;

    void Start()
    {
        center = transform.parent.position; // Center is the nucleus
    }

    void Update()
    {
        float angle = frequency * Time.time;
        Vector3 offset = amplitude * Mathf.Sin(angle) * axis;
        transform.position = center + offset;
    }
}
