using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Grid<GridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private GridObject[,] gridArray;
    private TextMeshProUGUI[,] debugTextArray;
    public Grid(int w, int h, float cSize, Vector3 oPosition, Func<Grid<GridObject>,int, int, GridObject> createGridObject)
    {
        this.width = w;
        this.height = h;
        this.cellSize = cSize;
        this.originPosition = oPosition;

        gridArray = new GridObject[w, h];

        for (int x= 0; x < w; x++)
        {
            for (int y= 0; y < h; y++)
            {
                gridArray[x, y] = createGridObject(this, x,y);
            }
        }

        bool showDebug = false;
        if (showDebug)
        {
            debugTextArray = new TextMeshProUGUI[w, h];
            for (int x = 0; x< gridArray.GetLength(0); x++)
            {
                for (int y = 0; y< gridArray.GetLength(1); y++)
                {
                    debugTextArray[x,y] = CreateUIText(null,gridArray[x,y]?.ToString(), GetWorldPosition(x,y) + new Vector3(cellSize,cellSize) * 0.5f, 40, Color.white, TMPro.TextAlignmentOptions.Midline);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
            
        }
    }
    public float GetCellSize()
    {
        return cellSize;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition-originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition-originPosition).y / cellSize);
    }

    public void SetGridObject(int x, int y, GridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x,y] = value;
            //debugTextArray[x,y].text = gridArray[x, y].ToString();
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }
    public void TriggerGirdObjectChanged(int x, int y)
    {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        
    }
    public void SetGridObject(Vector3 worldPosition, GridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public GridObject GetGridObject(int x, int y)
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
    public GridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    private TextMeshProUGUI CreateUIText(Transform parent, string text, Vector3 localPos, int fontSize, Color color, TMPro.TextAlignmentOptions textAlignment)
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject gameObject = new GameObject("World_Text", typeof(TextMeshProUGUI));
        RectTransform transform = gameObject.GetComponent<RectTransform>();
        transform.SetParent(canvas.transform, true);
        transform.position = localPos;
        TextMeshProUGUI textMesh = gameObject.GetComponent<TextMeshProUGUI>();
        textMesh.text = text;  
        textMesh.fontSize = fontSize;
        textMesh.alignment = textAlignment;
        textMesh.color = color;
        return textMesh;

    }
}
