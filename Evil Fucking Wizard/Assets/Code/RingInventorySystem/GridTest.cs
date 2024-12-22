using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    private Grid<int> grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<int>(5, 2, 10f, new Vector3(10,0,0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            grid.SetValue(GetMousePosition(),10);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(grid.GetValue(GetMousePosition()));
        }
    }

    private Vector2 GetMousePosition()
    {
        return Input.mousePosition;
    }
}
