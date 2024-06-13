using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomVisualization : MonoBehaviour
{
    [SerializeField]
    private GameObject electronPrefab; // Assign the electron prefab in the inspector
    
    [SerializeField]
    private Transform nucleus; // Assign the nucleus transform in the inspector

    void Start()
    {
        // Instantiate electrons for 2p orbitals
        InstantiateElectron(nucleus.position, Vector3.right);  // 2px
        InstantiateElectron(nucleus.position, Vector3.up);     // 2py
        InstantiateElectron(nucleus.position, Vector3.forward); // 2pz
    }

    void InstantiateElectron(Vector3 position, Vector3 axis)
    {
        GameObject electron = Instantiate(electronPrefab, position, Quaternion.identity);
        ElectronPOrbital orbitalScript = electron.GetComponent<ElectronPOrbital>();
        orbitalScript.axis = axis;
    }
}
