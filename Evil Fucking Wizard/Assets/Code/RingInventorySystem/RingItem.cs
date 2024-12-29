using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RingItem", menuName = "Create New Ring", order = 0)]
public class RingItem : ScriptableObject
{
    //Cambiar para solo poder rotar arriba o abajo
    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down:     return Dir.Up;//left
            //case Dir.Left:     return Dir.Up;
            case Dir.Up:       return Dir.Down; //right
            //case Dir.Right:    return Dir.Down;

        }
    }
    public enum Dir
    {
        Up,
        Down
        //Left,
        //Right
    }

    public string nameString;
    public RectTransform itemPrefab;
    public RectTransform itemVisual;
    public Transform ringPrefab3D;
    public int width;
    public int height;
    public int GetRotationAngle (Dir dir)
    {
        switch (dir)
        {
            default :
            case Dir.Down:  return 0;
            //case Dir.Left:  return 90;
            case Dir.Up:    return 180;
            //case Dir.Right: return 270;
        }
    }
    
    public Vector2Int GetRotationOffset (Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down:      return new Vector2Int(0,0);
            //case Dir.Left:      return new Vector2Int(0, width);
            case Dir.Up:        return new Vector2Int(width, height);
            //case Dir.Right:     return new Vector2Int(height, 0);
        }
    }
     
    public List<Vector2Int> GetGridPositionList(Vector2Int offset,Dir dir)
    {
        //Hay que cambiarlo para que arriba y abajo cambie. Ya que son los unicos que vamos a permitir. Hay que buscar una manera de hacer que los anillos L y T se trasladen bien
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch(dir)
        {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        gridPositionList.Add(offset+new Vector2Int(x,y));
                    }
                }
                break;
            /*
            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
             */
        }
        return gridPositionList;
    }
}
