using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public GameObject[] roadPrefabs; // Array of road prefabs for variety
    public int roadCount = 5; // Number of road segments to load initially
    public float roadLength = 20f; // Length of each road segment
    public float roadSpeed = 5f; // Speed at which the road moves

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
        MoveRoads();
    }

    void MoveRoads()
    {
        for (int i = 0; i < roads.Length; i++)
        {
            // Move each road backward
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
        // Find the farthest road segment
        float farthestZ = float.MinValue;
        foreach (GameObject r in roads)
        {
            if (r.transform.position.z > farthestZ)
            {
                farthestZ = r.transform.position.z;
            }
        }

        // Randomly select a new road prefab
        int randomIndex = Random.Range(0, roadPrefabs.Length);

        // Replace the road's content by destroying and instantiating a new prefab
        Vector3 newPosition = new Vector3(0, 0, farthestZ + roadLength);
        GameObject newRoad = Instantiate(roadPrefabs[randomIndex], newPosition, Quaternion.identity);

        // Replace the old road in the array
        int roadIndex = System.Array.IndexOf(roads, road);
        Destroy(road); // Destroy the old road
        roads[roadIndex] = newRoad; // Replace it with the new one
    }
}
