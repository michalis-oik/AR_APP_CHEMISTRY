using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtonNeutronMovement : MonoBehaviour
{
    public Vector3 centerPoint;
    public float attractionForce = 1.0f;

    void FixedUpdate()
    {
        Vector3 directionToCenter = centerPoint - transform.position;
        GetComponent<Rigidbody>().AddForce(directionToCenter.normalized * attractionForce);
    }
}
