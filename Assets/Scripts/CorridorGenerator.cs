using System.Collections.Generic;
using UnityEngine;

public class CorridorGenerator : MonoBehaviour
{
    public static CorridorGenerator instance;
    
    public GameObject CorridorPrefab;
    public GameObject StartCorridorPrefab;

    private List<GameObject> corridorList = new List<GameObject>();
    private float maxSpeed = 10;
    private float speed = 0;
    private int maxCorridorCount = 5;

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

        // Движение дорог.
        foreach (var corridor in corridorList)
        {
            corridor.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }

        if (corridorList[0].transform.position.z < -15)
        {
            DestroyCorridor();
            CreateCorridor();
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
        while (corridorList.Count > 0)
        {
            DestroyCorridor();
        }
        for (int corridor = 0; corridor < maxCorridorCount; corridor++)
        {
            CreateCorridor();
        }
        SwipeManager.instance.enabled = false;
        KeyboardManager.instance.enabled = false;
    }

    #endregion

    #region --- Private methods ---

    private void CreateCorridor()
    {
        Vector3 corridorPosition = Vector3.zero;
        GameObject corridorPrefab;

        if (corridorList.Count > 0)
        {
            corridorPrefab = CorridorPrefab;
            corridorPosition = corridorList[corridorList.Count - 1].transform.position + new Vector3(0, 0, 15);
        }
        else
        {
            corridorPrefab = StartCorridorPrefab;
        }

        var corridor = Instantiate(corridorPrefab, corridorPosition, Quaternion.identity);
        corridor.transform.SetParent(transform);
        corridorList.Add(corridor);
    }

    private void DestroyCorridor()
    {
        Destroy(corridorList[0]);
        corridorList.RemoveAt(0);
    }

    #endregion

}
