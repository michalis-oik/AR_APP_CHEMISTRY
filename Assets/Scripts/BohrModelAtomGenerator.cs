using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BohrModelAtomGenerator : MonoBehaviour
{
    public GameObject protonPrefab;
    public GameObject neutronPrefab;
    public GameObject electronPrefab;

    public Color orbitColor = Color.cyan; // Color of the orbit lines
    public float baseRadius = 0.5f; // Base radius for the electron orbits
    public float orbitLineWidth = 0.01f; // Width of the orbit lines
    public float baseOrbitSpeed = 1.0f; // Base speed for the electrons
    public float protonNeutronAttractionForce = 1.0f; // Attraction force for protons and neutrons

    private Transform nucleus; // Parent object to hold all elements
    private Vector3 nucleusPosition; // Store the current position of the nucleus

    private Dictionary<int, int[]> electronConfigurations = new Dictionary<int, int[]>
    {
        // Define electron configurations for each atomic number
        // Example configurations up to atomic number 30 (Zinc)
        { 1, new int[] { 1 } }, // Hydrogen
        { 2, new int[] { 2 } }, // Helium
        { 3, new int[] { 2, 1 } }, // Lithium
        { 4, new int[] { 2, 2 } }, // Beryllium
        { 5, new int[] { 2, 3 } }, // Boron
        { 6, new int[] { 2, 4 } }, // Carbon
        { 7, new int[] { 2, 5 } }, // Nitrogen
        { 8, new int[] { 2, 6 } }, // Oxygen
        { 9, new int[] { 2, 7 } }, // Fluorine
        { 10, new int[] { 2, 8 } }, // Neon
        { 11, new int[] { 2, 8, 1 } }, // Sodium
        { 12, new int[] { 2, 8, 2 } }, // Magnesium
        { 13, new int[] { 2, 8, 3 } }, // Aluminium
        { 14, new int[] { 2, 8, 4 } }, // Silicon
        { 15, new int[] { 2, 8, 5 } }, // Phosphorus
        { 16, new int[] { 2, 8, 6 } }, // Sulfur
        { 17, new int[] { 2, 8, 7 } }, // Chlorine
        { 18, new int[] { 2, 8, 8 } }, // Argon
        { 19, new int[] { 2, 8, 8, 1 } }, // Potassium
        { 20, new int[] { 2, 8, 8, 2 } }, // Calcium
        { 21, new int[] { 2, 8, 9, 2 } }, // Scandium
        { 22, new int[] { 2, 8, 10, 2 } }, // Titanium
        { 23, new int[] { 2, 8, 11, 2 } }, // Vanadium
        { 24, new int[] { 2, 8, 13, 1 } }, // Chromium
        { 25, new int[] { 2, 8, 13, 2 } }, // Manganese
        { 26, new int[] { 2, 8, 14, 2 } }, // Iron
        { 27, new int[] { 2, 8, 15, 2 } }, // Cobalt
        { 28, new int[] { 2, 8, 16, 2 } }, // Nickel
        { 29, new int[] { 2, 8, 18, 1 } }, // Copper
        { 30, new int[] { 2, 8, 18, 2 } } // Zinc
    };

    private List<GameObject> orbitLines = new List<GameObject>(); // Store references to created orbit lines

    void Start()
    {
        int atomicNumber = 30; // Example: Zinc
        int neutronNumber = 35; // Example: approximate neutron number for Zinc

        // Create a nucleus object as the parent
        nucleus = new GameObject("Nucleus").transform;
        nucleusPosition = nucleus.position;

        GenerateAtom(atomicNumber, neutronNumber);
    }

    private void Update()
    {
        // Example: Update the nucleus position based on user input or other game logic
        nucleusPosition = nucleus.position;

        // Update electron positions
        UpdateElectronPositions();
    }

    void GenerateAtom(int atomicNumber, int neutronNumber)
    {
        PlaceProtonsAndNeutrons(atomicNumber, neutronNumber);
        if (electronConfigurations.TryGetValue(atomicNumber, out int[] configuration))
        {
            PlaceElectrons(configuration);
            DrawOrbits(configuration.Length);
        }
        else
        {
            Debug.LogError("Element not defined.");
        }
    }

    void PlaceProtonsAndNeutrons(int protonCount, int neutronCount)
    {
        for (int i = 0; i < protonCount; i++)
        {
            GameObject proton = Instantiate(protonPrefab, nucleus.position + Random.insideUnitSphere * 0.1f, Quaternion.identity, nucleus);
            SetupProtonNeutron(proton);
        }
        for (int i = 0; i < neutronCount; i++)
        {
            GameObject neutron = Instantiate(neutronPrefab, nucleus.position + Random.insideUnitSphere * 0.1f, Quaternion.identity, nucleus);
            SetupProtonNeutron(neutron);
        }
    }

    void SetupProtonNeutron(GameObject particle)
    {
        if (particle != null)
        {
            Rigidbody rb = particle.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = particle.AddComponent<Rigidbody>();
            }
            rb.useGravity = false;
            rb.drag = 1.0f;

            ProtonNeutronMovement movementScript = particle.GetComponent<ProtonNeutronMovement>();
            if (movementScript == null)
            {
                movementScript = particle.AddComponent<ProtonNeutronMovement>();
            }
            movementScript.centerPoint = nucleus.position;
            movementScript.attractionForce = protonNeutronAttractionForce;
        }
    }

    void PlaceElectrons(int[] configuration)
    {
        if (electronPrefab == null)
        {
            Debug.LogError("electronPrefab is not assigned in the inspector.");
            return;
        }

        float initialRadius = baseRadius;
        for (int shell = 0; shell < configuration.Length; shell++)
        {
            int electronCount = configuration[shell];
            float orbitalRadius = initialRadius + shell * 0.3f * baseRadius; // Adjust the radius increment for each shell

            for (int i = 0; i < electronCount; i++)
            {
                float angle = i * Mathf.PI * 2 / electronCount;
                Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * orbitalRadius;
                GameObject electron = Instantiate(electronPrefab, nucleus.position + position, Quaternion.identity, nucleus);

                if (electron == null)
                {
                    Debug.LogError("Failed to instantiate electron prefab.");
                    continue;
                }
                BohrElectronOrbital orbitalScript = electron.GetComponent<BohrElectronOrbital>();
                if (orbitalScript == null)
                {
                    Debug.LogError("Electron prefab does not have the BohrElectronOrbital component.");
                    continue;
                }
                orbitalScript.orbitCenter = nucleus.position;
                orbitalScript.orbitRadius = orbitalRadius;
                orbitalScript.orbitSpeed = baseOrbitSpeed + shell * 0.5f; // Different speed for each shell
                orbitalScript.angleOffset = angle;
            }
        }
    }

    void DrawOrbits(int shellCount)
    {
        float initialRadius = baseRadius;
        for (int shell = 0; shell < shellCount; shell++)
        {
            float orbitalRadius = initialRadius + shell * 0.3f * baseRadius; // Adjust the radius increment for each shell
            CreateOrbitLine(orbitalRadius);
        }
    }

    void CreateOrbitLine(float radius)
    {
        GameObject orbitLine = new GameObject("OrbitLine");
        LineRenderer lineRenderer = orbitLine.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 100; // Number of points in the line
        lineRenderer.startWidth = orbitLineWidth; // Line width
        lineRenderer.endWidth = orbitLineWidth;
        lineRenderer.loop = true; // Close the line
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = orbitColor;
        lineRenderer.endColor = orbitColor;

        Vector3[] positions = new Vector3[lineRenderer.positionCount];
    for (int i = 0; i < lineRenderer.positionCount; i++)
    {
        float angle = i * Mathf.PI * 2 / lineRenderer.positionCount;
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        positions[i] = new Vector3(x, 0, z) + nucleus.position;
    }

    lineRenderer.SetPositions(positions);
    orbitLine.transform.parent = nucleus.transform;

    // Optionally, you can adjust the position and rotation relative to the parent if needed
    orbitLine.transform.localPosition = Vector3.zero;
    orbitLine.transform.localRotation = Quaternion.identity;

    // Add the orbit line to the list for later reference
    orbitLines.Add(orbitLine);
}

void UpdateElectronPositions()
{
    // Update electron positions based on the current nucleus position
    foreach (Transform child in nucleus)
    {
        BohrElectronOrbital orbitalScript = child.GetComponent<BohrElectronOrbital>();
        if (orbitalScript != null)
        {
            float angle = Time.time * orbitalScript.orbitSpeed + orbitalScript.angleOffset;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * orbitalScript.orbitRadius;
            child.position = orbitalScript.orbitCenter + offset;
        }
    }

    // Update orbit lines based on the current nucleus position
    foreach (GameObject orbitLine in orbitLines)
    {
        LineRenderer lineRenderer = orbitLine.GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            float radius = lineRenderer.GetPosition(0).magnitude;
            Vector3[] positions = new Vector3[lineRenderer.positionCount];
            for (int j = 0; j < lineRenderer.positionCount; j++)
            {
                float angle = j * Mathf.PI * 2 / lineRenderer.positionCount;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                positions[j] = new Vector3(x, 0, z) + nucleus.position;
            }
            lineRenderer.SetPositions(positions);
        }
    }
}
}
