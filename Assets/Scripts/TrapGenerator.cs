using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGenerator : MonoBehaviour
{
    public GameObject[] TrapPrefabs;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var trapPosition = transform.GetChild(i);
            var trap = Instantiate(TrapPrefabs[Random.Range(0, TrapPrefabs.Length)], trapPosition.position, Quaternion.identity);
            trap.transform.SetParent(trapPosition);
        }
    }
}
