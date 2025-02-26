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
    public GameObject DebugCube;
    public Room[,] floorPlan = new Room[20, 20];
    [SerializeField]private List<GameObject> roomPrefabs;
    [SerializeField]private Queue<Room> roomQueue = new Queue<Room>();
    int maxrooms = 15;
    int minrooms = 7;
    [SerializeField] int NumberOfRooms;
    int currentnumberOfRooms = 1;
    public int Level = 1;
    Room startRoom;
    public List<GameObject> enemyPrefabs;

    private void Awake()
    {
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                floorPlan[x, y] = new Room(x,y, false);
            }
        }
        NumberOfRooms = Mathf.Clamp((int)(UnityEngine.Random.Range(0, 2) + 5 + Level * 2.6f), minrooms, maxrooms);

        startRoom = floorPlan[10, 10];
        roomQueue.Enqueue(startRoom);
        startRoom.ocupied = true;
        startRoom.roomType = RoomTypes.START;
        Instantiate(DebugCube, new Vector3(roomQueue.Peek().x * 10, 0, roomQueue.Peek().y * 10), Quaternion.identity);
        for (int i = 1; i < NumberOfRooms; i++)
        {
            Debug.Log(roomQueue.Peek().roomType);
            CheckAllNeighbours(roomQueue.Dequeue());
            if (roomQueue.Peek().roomType == RoomTypes.START)
            {
                i--;
            }
        }
        Debug.Log(currentnumberOfRooms);
    }

    public void CheckAllNeighbours(Room r)
    {
        if (CheckNeighbour(r,Dir.UP))
        {
            GetNeighbour(r, Dir.UP).ocupied = true;
            GetNeighbour(r, Dir.UP).roomType = RoomTypes.EMPTY;
            roomQueue.Enqueue(GetNeighbour(r, Dir.UP));
            currentnumberOfRooms++;
            Instantiate(DebugCube, new Vector3(roomQueue.Peek().x * 10, 0, roomQueue.Peek().y * 10), Quaternion.identity);
        }
        else if (CheckNeighbour(r, Dir.DOWN))
        {
            GetNeighbour(r, Dir.DOWN).ocupied = true;
            GetNeighbour(r, Dir.DOWN).roomType = RoomTypes.EMPTY;
            roomQueue.Enqueue(GetNeighbour(r, Dir.DOWN));
            currentnumberOfRooms++;
            Instantiate(DebugCube, new Vector3(roomQueue.Peek().x * 10, 0, roomQueue.Peek().y * 10), Quaternion.identity);
        }
        else if (CheckNeighbour(r, Dir.LEFT))
        {
            GetNeighbour(r, Dir.LEFT).ocupied = true;
            GetNeighbour(r, Dir.LEFT).roomType = RoomTypes.EMPTY;
            roomQueue.Enqueue(GetNeighbour(r, Dir.LEFT));
            currentnumberOfRooms++;
            Instantiate(DebugCube, new Vector3(roomQueue.Peek().x * 10, 0, roomQueue.Peek().y * 10), Quaternion.identity);
        }
        else if (CheckNeighbour(r, Dir.RIGHT))
        {
            GetNeighbour(r, Dir.RIGHT).ocupied = true;
            GetNeighbour(r, Dir.RIGHT).roomType = RoomTypes.EMPTY;
            roomQueue.Enqueue(GetNeighbour(r, Dir.RIGHT));
            currentnumberOfRooms++;
            Instantiate(DebugCube, new Vector3(roomQueue.Peek().x * 10, 0, roomQueue.Peek().y * 10), Quaternion.identity);
        }
        else
        {
            roomQueue.Enqueue(startRoom);
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
        bool ocupied = !neighbour.ocupied;
        bool neighbourneighbour = ocupiedNeighboursNeighbours < 3;
        bool roomsLeft = currentnumberOfRooms <= NumberOfRooms;

        if (r.roomType == RoomTypes.START)
        {
            return ocupied && neighbourneighbour && roomsLeft;
        }
        return ocupied && neighbourneighbour && roomsLeft && randomBool;
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
                neighbour = floorPlan[r.x-1, r.y];
                break;
            case Dir.RIGHT:
                neighbour = floorPlan[r.x+1, r.y];
                break;
            default:
                neighbour = floorPlan[0, 0];
                Debug.LogError("No Neigbours");
                break;
        }
        return neighbour;
    }
}
