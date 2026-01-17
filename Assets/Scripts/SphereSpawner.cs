using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    public GameObject spherePrefab;    // Sphere prefab to spawn
    public Transform ringCenter;       // Center of the ring (empty GameObject at middle)
    public float ringRadius = 10f;     // Radius of the ring/arena
    public int initialSpawnCount = 20; // How many spheres to start with
    public float spawnInterval = 1f;   // Time between spawns (smaller = faster)
    public int maxSpheres = 50;        // Prevent unlimited spawning

    private int currentSpheres;

    void Start()
    {
        // Spawn initial spheres
        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnSphere();
        }

        // Start timed spawns
        InvokeRepeating(nameof(SpawnSphere), spawnInterval, spawnInterval);
    }

    void SpawnSphere()
    {
        if (currentSpheres >= maxSpheres) return; // Limit

        // Pick a random position inside the ring
        Vector2 randomCircle = Random.insideUnitCircle * ringRadius;
        Vector3 spawnPos = new Vector3(randomCircle.x, 1f, randomCircle.y) + ringCenter.position;

        // Create sphere
        GameObject sphere = Instantiate(spherePrefab, spawnPos, Quaternion.identity);

        // 30% chance for a trap sphere
        if (Random.value < 0.15f)
        {
            sphere.GetComponent<Renderer>().material.color = Color.red;
            sphere.tag = "Trap";
        }
        else
        {
            sphere.GetComponent<Renderer>().material.color = Color.white;
            sphere.tag = "Normal";
        }

        currentSpheres++;
    }

    // Gizmo for visualizing spawn area
    void OnDrawGizmos()
    {
        if (ringCenter == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(ringCenter.position, ringRadius);
    }
}
