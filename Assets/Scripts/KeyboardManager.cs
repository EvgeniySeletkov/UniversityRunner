using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    public static KeyboardManager instance;

    private bool[] keys = new bool[4];

    #region --- Delegates ---

    public delegate void MoveDelegate(bool[] keys);
    public MoveDelegate MoveEvent;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    #region --- Unity methods ---

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

    #endregion

    #region --- Private methods ---

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

    #endregion

}
