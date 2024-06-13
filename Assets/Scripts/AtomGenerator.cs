using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomGenerator : MonoBehaviour
{
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
        // Add more elements as needed
    };

    void Start()
    {
        int atomicNumber = 8; // Example: Oxygen
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
            Instantiate(electronPrefab, position, Quaternion.identity, nucleus);
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
            orbitalScript.amplitude = shell; // Adjust amplitude based on shell number
        }
    }
}
