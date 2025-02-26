using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public enum Dir
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }
    public class Room
    {
        public int x;
        public int y;
        public bool ocupied;
        public GameObject prefab;
        public RoomTypes roomType;
        public Room(int x,int y, bool b)
        {
            this.x = x;
            this.y = y;
            ocupied = b;
        }


    }
    public Room[,] floorPlan = new Room[10, 8];
    [SerializeField]private List<GameObject> roomPrefabs;
    [SerializeField]private Queue<Room> roomQueue;
    int maxrooms = 15;
    int minrooms = 7;
    [SerializeField] int NumberOfRooms;
    int currentnumberOfRooms = 0;
    public int Level = 1;

    public List<GameObject> enemyPrefabs;

    private void Awake()
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                floorPlan[x, y] = new Room(x,y, false);
            }
        }
        NumberOfRooms = Mathf.Clamp((int)(UnityEngine.Random.Range(0, 2) + 5 + Level * 2.6f), minrooms, maxrooms);

        //Enqueue StartRoom on roomQueue
        Room startRoom = floorPlan[5, 4];
        roomQueue.Enqueue(startRoom);
        startRoom.ocupied = true;
        startRoom.roomType = RoomTypes.START;

        for (int i  = 0; i < NumberOfRooms; i++)
        {
            CheckAllNeighbours(roomQueue.Dequeue());
            currentnumberOfRooms++;
        }

    }
    public void CheckAllNeighbours(Room r)
    {
        if (CheckNeighbour(r,Dir.UP))
        {
            GetNeighbour(r, Dir.UP).ocupied = true;
            roomQueue.Enqueue(GetNeighbour(r, Dir.UP));
            //Enqueue Room
        }
        if (CheckNeighbour(r, Dir.DOWN))
        {
            GetNeighbour(r, Dir.DOWN).ocupied = true;
            roomQueue.Enqueue(GetNeighbour(r, Dir.DOWN));
            //Enqueue Room
        }
        if (CheckNeighbour(r, Dir.LEFT))
        {
            GetNeighbour(r, Dir.LEFT).ocupied = true;
            roomQueue.Enqueue(GetNeighbour(r, Dir.LEFT));
            //Enqueue Room
        }
        if (CheckNeighbour(r, Dir.RIGHT))
        {
            GetNeighbour(r, Dir.RIGHT).ocupied = true;
            roomQueue.Enqueue(GetNeighbour(r, Dir.RIGHT));
            //Enqueue Room
        }
    }
    public bool CheckNeighbour(Room r, Dir d)
    {
        Room neighbour = GetNeighbour(r, d);
        int ocupiedNeighboursNeighbours = 0;
        if (GetNeighbour(neighbour, Dir.UP).ocupied)
            ocupiedNeighboursNeighbours++;
        if (GetNeighbour(neighbour, Dir.DOWN).ocupied)
            ocupiedNeighboursNeighbours++;
        if (GetNeighbour(neighbour, Dir.LEFT).ocupied)
            ocupiedNeighboursNeighbours++;
        if (GetNeighbour(neighbour, Dir.RIGHT).ocupied)
            ocupiedNeighboursNeighbours++;

        System.Random random = new System.Random();
        bool randomBool = random.NextDouble() >= 0.5;

        return !neighbour.ocupied && ocupiedNeighboursNeighbours < 2 && currentnumberOfRooms <= NumberOfRooms && randomBool;
    }
    public Room GetNeighbour(Room r, Dir d)
    {
        
        Room neighbour;
        switch (d)
        {
            case Dir.UP:
                neighbour = floorPlan[r.x, r.y+1];
                break;
            case Dir.DOWN:
                neighbour = floorPlan[r.x, r.y-1];
                break;
            case Dir.LEFT:
                neighbour = floorPlan[r.x+1, r.y];
                break;
            case Dir.RIGHT:
                neighbour = floorPlan[r.x-1, r.y];
                break;
            default:
                neighbour = floorPlan[0, 0];
                Debug.LogError("No Neigbours");
                break;
        }
        return neighbour;
    }
}
