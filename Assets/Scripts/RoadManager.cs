using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public GameObject[] roadPrefabs; // Array of road prefabs for variety
    public int roadCount = 5; // Number of road segments to load initially
    public float roadLength = 20f; // Length of each road segment
    public float roadSpeed = 5f; // Initial speed at which the road moves
    public float speedIncreaseRate = 0.1f; // Rate at which speed increases (per second)
    public float maxSpeed = 20f; // Maximum speed limit for the road

    private GameObject[] roads; // Array to hold active road segments

    void Start()
    {
        roads = new GameObject[roadCount];

        // Instantiate and position the initial road segments
        for (int i = 0; i < roadCount; i++)
        {
            // Randomly select a road prefab for each initial segment
            int randomIndex = Random.Range(0, roadPrefabs.Length);
            roads[i] = Instantiate(roadPrefabs[randomIndex], new Vector3(0, 0, i * roadLength), Quaternion.identity);
        }
    }

    void Update()
    {
        IncreaseRoadSpeed();  // Increase road speed over time
        MoveRoads();
    }

    void MoveRoads()
    {
        for (int i = 0; i < roads.Length; i++)
        {
            // Move each road backward based on current speed
            roads[i].transform.Translate(Vector3.back * roadSpeed * Time.deltaTime);

            // Check if the road has moved out of view
            if (roads[i].transform.position.z < -roadLength)
            {
                RecycleRoad(roads[i]);
            }
        }
    }

    void RecycleRoad(GameObject road)
    {
        // Check if the road contains any obstacles and clear them
        foreach (Transform child in road.transform)
        {
            if (child.CompareTag("Obstacle"))
            {
                Destroy(child.gameObject);
            }
        }

        // Find the farthest road segment
        float farthestZ = float.MinValue;
        foreach (GameObject r in roads)
        {
            if (r.transform.position.z > farthestZ)
            {
                farthestZ = r.transform.position.z;
            }
        }

        // Reposition the road segment
        Vector3 newPosition = new Vector3(0, 0, farthestZ + roadLength);
        road.transform.position = newPosition;
    }

    void IncreaseRoadSpeed()
    {
        // Gradually increase the road speed over time, without exceeding the max speed
        if (roadSpeed < maxSpeed)
        {
            roadSpeed += speedIncreaseRate * Time.deltaTime;
        }
    }

    public void StopRoad()
    {
        roadSpeed = 0f; // Stop the road movement
    }
}
