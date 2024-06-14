using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronOrbital : MonoBehaviour
{
    public Vector3 axis = Vector3.right; // Default axis for "p" orbitals
    public float amplitude = 1.0f;
    public float frequency = 1.0f;
    public bool isSOrbital = false; // Flag to differentiate "s" and "p" orbitals
    public float angleOffset = 0.0f; // Angular offset for unique electron paths
    public float spinDirection = 1.0f; // Direction of the electron's spin

    private Vector3 center;

    void Start()
    {
        center = transform.parent.position; // Center is the nucleus
    }

    void Update()
    {
        float angle = frequency * Time.time * spinDirection + angleOffset;

        if (isSOrbital)
        {
            float x = center.x + amplitude * Mathf.Cos(angle);
            float y = center.y + amplitude * Mathf.Sin(angle);
            float z = center.z + amplitude * Mathf.Sin(angle / 2); // Adds 3D movement

            transform.position = new Vector3(x, y, z);
        }
        else
        {
            Vector3 offset = amplitude * Mathf.Sin(angle) * axis;
            transform.position = center + offset;
        }
    }
}
