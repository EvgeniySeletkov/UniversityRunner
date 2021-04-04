using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManager : MonoBehaviour
{
    public static SwipeManager instance;
    private const float SWIPE_THRESHOLD = 50;

    #region --- Private fields ---

    private Vector2 startTouch;
    private bool touchMoved;
    private Vector2 swipeDelta;

    private bool[] swipes = new bool[4];

    #endregion

    #region --- Delegates ---

    public delegate void MoveDelegate(bool[] swipes);
    public MoveDelegate MoveEvent;

    public delegate void ClickDelegate(Vector2 position);
    public ClickDelegate ClickEvent;

    #endregion

    #region --- Unity methods ---

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // Начало и конец свайпа.
        if (GetStartTouch())
        {
            startTouch = GetTouchPosition();
            touchMoved = true;
        }
        else if (GetEndTouch() && touchMoved == true)
        {
            SendSwipe();
            touchMoved = false;
        }

        // Расстояние свайпа.
        swipeDelta = Vector2.zero;
        if (touchMoved && GetTouch())
        {
            swipeDelta = GetTouchPosition() - startTouch;
        }

        // Проверка свайпа.
        if (swipeDelta.magnitude > SWIPE_THRESHOLD)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                swipes[(int)Direction.Left] = swipeDelta.x < 0;
                swipes[(int)Direction.Right] = swipeDelta.x > 0;
            }
            else
            {
                swipes[(int)Direction.Down] = swipeDelta.y < 0;
                swipes[(int)Direction.Up] = swipeDelta.y > 0;
            }
            SendSwipe();
        }
    }

    #endregion

    #region --- Private methods ---

    private bool GetStartTouch()
    {
        return Input.GetMouseButtonDown(0);
    }

    private Vector2 GetTouchPosition()
    {
        return Input.mousePosition;
    }

    private bool GetEndTouch()
    {
        return Input.GetMouseButtonUp(0);
    }

    private bool GetTouch()
    {
        return Input.GetMouseButton(0);
    }

    private void SendSwipe()
    {
        if (swipes[0] || swipes[1] || swipes[2] || swipes[3])
        {
            Debug.Log("Swipe");
            MoveEvent?.Invoke(swipes);
        }
        else
        {
            Debug.Log("Click");
            ClickEvent?.Invoke(GetTouchPosition());
        }

        ResetSwipe();
    }

    private void ResetSwipe()
    {
        startTouch = swipeDelta = Vector2.zero;
        touchMoved = false;
        for (int i = 0; i < swipes.Length; i++)
        {
            swipes[i] = false;
        }
    }

    #endregion
}