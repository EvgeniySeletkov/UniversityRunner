using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public static RoadGenerator instance;
    
    public GameObject RoadPrefab;

    private List<GameObject> roads = new List<GameObject>();
    private float maxSpeed = 10;
    private float speed = 0;
    private int maxRoadCount = 5;

    #region --- Unity methods ---

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ResetLevel();
        // StartLevel();
    }

    private void Update()
    {
        if (speed == 0)
        {
            return;
        }

        // �������� �����.
        foreach (var road in roads)
        {
            road.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }

        if (roads[0].transform.position.z < -15)
        {
            DestroyRoads();
            CreateRoad();
        }
    }

    #endregion

    #region --- Public methods ---

    public void StartLevel()
    {
        speed = maxSpeed;
        SwipeManager.instance.enabled = true;
        KeyboardManager.instance.enabled = true;
    }

    public void ResetLevel()
    {
        speed = 0;
        while (roads.Count > 0)
        {
            DestroyRoads();
        }
        for (int road = 0; road < maxRoadCount; road++)
        {
            CreateRoad();
        }
        SwipeManager.instance.enabled = false;
        KeyboardManager.instance.enabled = false;
    }

    #endregion

    #region --- Private methods ---

    private void CreateRoad()
    {
        Vector3 roadPosition = Vector3.zero;

        if (roads.Count > 0)
        {
            roadPosition = roads[roads.Count - 1].transform.position + new Vector3(0, 0, 15);
        }

        var road = Instantiate(RoadPrefab, roadPosition, Quaternion.identity);
        road.transform.SetParent(transform);
        roads.Add(road);
    }

    private void DestroyRoads()
    {
        Destroy(roads[0]);
        roads.RemoveAt(0);
    }

    #endregion

}
