using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTrapDestroyer : MonoBehaviour
{
    [Range(0, 1)]
    public float ChanceOfStaying = 0.5f;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var trapPosition = transform.GetChild(i);
            if (Random.value > ChanceOfStaying)
            {
                DestroyImmediate(trapPosition.gameObject);
            }
        }
    }
}
