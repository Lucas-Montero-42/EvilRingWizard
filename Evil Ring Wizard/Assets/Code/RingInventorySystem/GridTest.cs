using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    private Grid<Ring> grid;
    private Grid<Ring> grid2;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<Ring>(5, 2, 100f, new Vector3(100,100,0),(Grid<Ring> r, int x, int y)=>new Ring(0,r,x,y));
        grid2 = new Grid<Ring>(5, 2, 100f, new Vector3(700, 100, 0), (Grid<Ring> r, int x, int y) => new Ring(0, r, x, y));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ring ring = grid.GetGridObject(GetMousePosition());
            if (ring != null)
            {
                ring.IncreaseSize(1);
                //grid.SetGridObject(GetMousePosition(),new Ring(1));
                Debug.Log("HERE Mouse 1");
            }

            ring = grid2.GetGridObject(GetMousePosition());
            if (ring != null)
            {
                ring.IncreaseSize(1);
                //grid2.SetGridObject(GetMousePosition(), new Ring(1));
                Debug.Log("HERE2 Mouse 1");
            }

        }
        if (Input.GetMouseButtonDown(0))
        {
            Ring ring = grid.GetGridObject(GetMousePosition());
            if (ring != null)
            {
                Debug.Log(grid.GetGridObject(GetMousePosition()).size);
                Debug.Log("HERE Mouse 0");
            }

            ring = grid2.GetGridObject(GetMousePosition());
            if (ring != null)
            {
                Debug.Log(grid2.GetGridObject(GetMousePosition()).size);
                Debug.Log("HERE2 Mouse 0");
            }

        }
    }

    private Vector2 GetMousePosition()
    {
        return Input.mousePosition;
    }
}
public class Ring
{
    public int size = 0;
    public Grid<Ring> grid;
    int X, Y;
    public Ring(int s, Grid<Ring> grid, int x, int y)
    {
        size = s;
        this.grid = grid;
        X = x;
        Y = y;
    }
    public void IncreaseSize(int value)
    {
        size += value;
        grid.TriggerGirdObjectChanged(X, Y);
    }
    public override string ToString()
    {
        
        return base.ToString() + "\n" + size.ToString();
    }
}
