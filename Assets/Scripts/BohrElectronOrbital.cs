using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BohrElectronOrbital : MonoBehaviour
{
    public Vector3 orbitCenter;
    public float orbitRadius;
    public float orbitSpeed;
    public float angleOffset;

    void Update()
    {
        float angle = Time.time * orbitSpeed + angleOffset;
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * orbitRadius;
        transform.position = orbitCenter + offset;
    }
}
