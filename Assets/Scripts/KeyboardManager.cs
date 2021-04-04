using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    public static KeyboardManager instance;

    private bool[] keys = new bool[4];

    public delegate void MoveDelegate(bool[] keys);
    public MoveDelegate MoveEvent;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            keys[(int)Direction.Left] = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            keys[(int)Direction.Right] = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            keys[(int)Direction.Up] = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            keys[(int)Direction.Down] = true;
        }

        SendPress();
    }

    private void SendPress()
    {
        if (keys[0] || keys[1] || keys[2] || keys[3])
        {
            Debug.Log("Pressed");
            MoveEvent?.Invoke(keys);
        }
        ResetPressed();
    }

    private void ResetPressed()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i] = false;
        }
    }
}
