using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using Unity.Mathematics;
using UnityEngine;

public class Grid<GridObject>
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private GridObject[,] gridArray;
    private TextMeshPro[,] debugTextArray;
    public Grid(int w, int h, float cSize, Vector3 oPosition)
    {
        this.width = w;
        this.height = h;
        this.cellSize = cSize;
        this.originPosition = oPosition;

        gridArray = new GridObject[w, h];
        debugTextArray = new TextMeshPro[w, h];
        for (int x = 0; x< gridArray.GetLength(0); x++)
        {
            for (int y = 0; y< gridArray.GetLength(1); y++)
            {
                debugTextArray[x,y] = CreateWorldText(null,gridArray[x,y].ToString(), GetWorldPosition(x,y) + new Vector3(cellSize,cellSize) * 0.5f, 10, Color.white, TMPro.TextAlignmentOptions.Midline);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        //SetValue(2, 1, 56);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition-originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition-originPosition).y / cellSize);
    }

    public void SetValue(int x, int y, GridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x,y] = value;
            debugTextArray[x,y].text = gridArray[x, y].ToString();
        }
    }
    public void SetValue(Vector3 worldPosition, GridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public GridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(GridObject);
        }
    }
    public GridObject GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    private TextMeshPro CreateWorldText(Transform parent, string text, Vector3 localPos, int fontSize, Color color, TMPro.TextAlignmentOptions textAlignment)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMeshPro));
        RectTransform transform = gameObject.GetComponent<RectTransform>();
        transform.SetParent(parent, false);
        transform.localPosition = localPos;
        TextMeshPro textMesh = gameObject.GetComponent<TextMeshPro>();
        textMesh.text = text;  
        textMesh.fontSize = fontSize;
        textMesh.alignment = textAlignment;
        textMesh.color = color;
        return textMesh;

    }
}
