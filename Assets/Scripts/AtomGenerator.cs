using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AtomGenerator : MonoBehaviour
{
    private int countForRotate;
    public GameObject electronPrefab;
    public Transform nucleus;

    // Define electron configurations for elements up to oxygen
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
        { 9, "1s2 2s2 2p5" },  // Fluorine
        { 10, "1s2 2s2 2p6" }, // Neon
        { 11, "1s2 2s2 2p6 3s1" }, // Sodium
        { 12, "1s2 2s2 2p6 3s2" }, // Magnesium
        // Add more elements as needed
    };

    void Start()
    {
        int atomicNumber = 12; // Example: Oxygen
        GenerateAtom(atomicNumber);
        countForRotate = 0;
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
                // Handle 'd' and 'f' orbitals similarly
            }
        }
    }

    void PlaceElectronsInSOrbital(int shell, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = nucleus.position + Random.insideUnitSphere * shell * 0.5f; // Random position within the shell radius
            GameObject electron = Instantiate(electronPrefab, position, Quaternion.identity, nucleus);
            ElectronMovement movementScript = electron.AddComponent<ElectronMovement>();
            if(countForRotate!=1)
            {
                movementScript.radius = shell * 0.2f;
                movementScript.speed = 1.0f + i * 0.1f; // Slightly different speeds for visual variety
                countForRotate++;
            }
            else
            {
                movementScript.radius = shell * -0.2f;
                movementScript.speed = 1.0f + i * 0.1f; // Slightly different speeds for visual variety
                countForRotate = 0;
            }
            
            
        }
    }

    void PlaceElectronsInPOrbital(int shell, int count)
    {
        Vector3[] axes = { Vector3.right, Vector3.up, Vector3.forward };

        for (int i = 0; i < count; i++)
        {
            Vector3 axis = axes[i % 3]; // Cycle through x, y, z axes
            GameObject electron = Instantiate(electronPrefab, nucleus.position, Quaternion.identity, nucleus);
            ElectronPOrbital orbitalScript = electron.AddComponent<ElectronPOrbital>();
            orbitalScript.axis = axis;
            orbitalScript.amplitude = shell * 0.2f; // Adjust amplitude based on shell number
            orbitalScript.frequency = 1.0f + i * 0.1f; // Slightly different frequencies for visual variety
        }
    }
}
