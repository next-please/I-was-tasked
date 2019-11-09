﻿using UnityEngine;
using UnityEngine.EventSystems;

public class CustomCursor : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D clickCursor;
    public Texture2D dragCursor;

    private float offset = 30.0f;
    private bool mouseDown = false;

    private void Start()
    {
        Cursor.SetCursor(defaultCursor, Vector2.right * offset, CursorMode.Auto);
    }

    private void Update()
    {
        if (mouseDown == Input.GetMouseButton(0))
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);
            mouseDown = true;
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.right * offset, CursorMode.Auto); ;
            mouseDown = false;
        }
    }
}
