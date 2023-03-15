using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursor : MonoBehaviour
{
    public Texture2D cursor;

    Vector2 hotSpot;

    private void Start()
    {
        hotSpot = new Vector2(cursor.width / 2f, cursor.height / 2f);
        Cursor.SetCursor(cursor, hotSpot, CursorMode.Auto);
    }
}
