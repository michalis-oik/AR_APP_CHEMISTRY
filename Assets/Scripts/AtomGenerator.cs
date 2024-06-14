using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AtomGenerator : MonoBehaviour
{
    public GameObject electronPrefab;
    public Transform nucleus;
    public int atomicNumber = 1;

    // Define electron configurations for elements up to argon (atomic number 18)
    private Dictionary<int, string> electronConfigurations = new Dictionary<int, string>
    {
        { 1, "1s1" },
        { 2, "1s2" },
        { 3, "1s2 2s1" },
        { 4, "1s2 2s2" },
        { 5, "1s2 2s2 2p1" },
        { 6, "1s2 2s2 2p2" },
        { 7, "1s2 2s2 2p3" },
        { 8, "1s2 2s2 2p4" },
        { 9, "1s2 2s2 2p5" },
        { 10, "1s2 2s2 2p6" },
        { 11, "1s2 2s2 2p6 3s1" },
        { 12, "1s2 2s2 2p6 3s2" },
        { 13, "1s2 2s2 2p6 3s2 3p1" },
        { 14, "1s2 2s2 2p6 3s2 3p2" },
        { 15, "1s2 2s2 2p6 3s2 3p3" },
        { 16, "1s2 2s2 2p6 3s2 3p4" },
        { 17, "1s2 2s2 2p6 3s2 3p5" },
        { 18, "1s2 2s2 2p6 3s2 3p6" },
        { 19, "1s2 2s2 2p6 3s2 3p6 4s1" },
        { 20, "1s2 2s2 2p6 3s2 3p6 4s2" },
        { 21, "1s2 2s2 2p6 3s2 3p6 4s2 3d1" },
        { 22, "1s2 2s2 2p6 3s2 3p6 4s2 3d2" },
        { 23, "1s2 2s2 2p6 3s2 3p6 4s2 3d3" },
        { 24, "1s2 2s2 2p6 3s2 3p6 4s1 3d5" },
        { 25, "1s2 2s2 2p6 3s2 3p6 4s2 3d5" },
        { 26, "1s2 2s2 2p6 3s2 3p6 4s2 3d6" },
        { 27, "1s2 2s2 2p6 3s2 3p6 4s2 3d7" },
        { 28, "1s2 2s2 2p6 3s2 3p6 4s2 3d8" },
        { 29, "1s2 2s2 2p6 3s2 3p6 4s1 3d10" },
        { 30, "1s2 2s2 2p6 3s2 3p6 4s2 3d10" },
    };

    void Start()
    {
        //int atomicNumber = 30; // Example: Zinc
        GenerateAtom(atomicNumber);
    }

    void GenerateAtom(int atomicNumber)
    {
        if (electronConfigurations.TryGetValue(atomicNumber, out string configuration))
        {
            Debug.Log("Electron configuration: " + configuration);
            PlaceElectrons(configuration);
        }
        else
        {
            Debug.LogError("Element not defined.");
        }
    }

    void PlaceElectrons(string configuration)
    {
        if (electronPrefab == null)
        {
            Debug.LogError("electronPrefab is not assigned in the inspector.");
            return;
        }

        if (nucleus == null)
        {
            Debug.LogError("nucleus is not assigned in the inspector.");
            return;
        }

        string[] orbitals = configuration.Split(' ');

        foreach (string orbital in orbitals)
        {
            char shell = orbital[0]; // 1, 2, 3, etc.
            char subshell = orbital[1]; // s, p, d, f
            int electronCount = int.Parse(orbital.Substring(2)); // number of electrons

            switch (subshell)
            {
                case 's':
                    PlaceElectronsInSOrbital(shell - '0', electronCount);
                    break;
                case 'p':
                    PlaceElectronsInPOrbital(shell - '0', electronCount);
                    break;
                case 'd':
                    PlaceElectronsInDOrbital(shell - '0', electronCount);
                    break;
                // Handle 'f' orbitals similarly
            }
        }
    }

    void PlaceElectronsInSOrbital(int shell, int count)
    {
        float orbitalRadius = shell * 0.2f; // Adjust the multiplier to make the orbit smaller

        for (int i = 0; i < count; i++)
        {
            Vector3 position = nucleus.position + Random.insideUnitSphere * orbitalRadius;
            GameObject electron = Instantiate(electronPrefab, position, Quaternion.identity, nucleus);
            if (electron == null)
            {
                Debug.LogError("Failed to instantiate electron prefab.");
                continue;
            }
            ElectronOrbital orbitalScript = electron.GetComponent<ElectronOrbital>();
            if (orbitalScript == null)
            {
                Debug.LogError("Electron prefab does not have the ElectronOrbital component.");
                continue;
            }
            orbitalScript.isSOrbital = true;
            orbitalScript.amplitude = orbitalRadius;
            orbitalScript.frequency = 1.0f + i * 0.1f; // Slightly different frequencies for visual variety
            orbitalScript.angleOffset = i * Mathf.PI / count; // Different angles for each electron
            orbitalScript.spinDirection = (i % 2 == 0) ? 1.0f : -1.0f; // Alternate spin direction
        }
    }

    void PlaceElectronsInPOrbital(int shell, int count)
    {
        float orbitalAmplitude = shell * 0.2f; // Adjust the multiplier to make the orbit smaller
        Vector3[] axes = { Vector3.right, Vector3.up, Vector3.forward };

        for (int i = 0; i < count; i++)
        {
            Vector3 axis = axes[i % 3]; // Cycle through x, y, z axes
            GameObject electron = Instantiate(electronPrefab, nucleus.position, Quaternion.identity, nucleus);
            if (electron == null)
            {
                Debug.LogError("Failed to instantiate electron prefab.");
                continue;
            }
            ElectronOrbital orbitalScript = electron.GetComponent<ElectronOrbital>();
            if (orbitalScript == null)
            {
                Debug.LogError("Electron prefab does not have the ElectronOrbital component.");
                continue;
            }
            orbitalScript.isSOrbital = false;
            orbitalScript.axis = axis;
            orbitalScript.amplitude = orbitalAmplitude;
            orbitalScript.frequency = 1.0f + i * 0.1f; // Slightly different frequencies for visual variety
            orbitalScript.angleOffset = i * Mathf.PI / count; // Different angles for each electron
            orbitalScript.spinDirection = (i % 2 == 0) ? 1.0f : -1.0f; // Alternate spin direction
        }
    }

    void PlaceElectronsInDOrbital(int shell, int count)
    {
        float orbitalAmplitude = shell * 0.2f; // Adjust the multiplier to make the orbit smaller
        Vector3[] axes = { Vector3.right, Vector3.up, Vector3.forward, 
                           new Vector3(1, 1, 0), new Vector3(1, 0, 1) };

        for (int i = 0; i < count; i++)
        {
            Vector3 axis = axes[i % 5]; // Cycle through x, y, z, xy, xz axes
            GameObject electron = Instantiate(electronPrefab, nucleus.position, Quaternion.identity, nucleus);
            if (electron == null)
            {
                Debug.LogError("Failed to instantiate electron prefab.");
                continue;
            }
            ElectronOrbital orbitalScript = electron.GetComponent<ElectronOrbital>();
            if (orbitalScript == null)
            {
                Debug.LogError("Electron prefab does not have the ElectronOrbital component.");
                continue;
            }
            orbitalScript.isSOrbital = false;
            orbitalScript.axis = axis;
            orbitalScript.amplitude = orbitalAmplitude;
            orbitalScript.frequency = 1.0f + i * 0.1f; // Slightly different frequencies for visual variety
            orbitalScript.angleOffset = i * Mathf.PI / count; // Different angles for each electron
            orbitalScript.spinDirection = (i % 2 == 0) ? 1.0f : -1.0f; // Alternate spin direction
        }
    }
}
