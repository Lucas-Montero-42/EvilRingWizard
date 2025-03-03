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
    const int maxAttempts = 4;
    int attemptCounter = 0;
    bool deadEndUsed = false;

    public GameObject DebugCube;
    public Room[,] floorPlan = new Room[20, 20];
    [SerializeField]private List<GameObject> roomPrefabs;
    private Queue<Room> roomQueue = new Queue<Room>();
    private Queue<Room> deadEnds = new Queue<Room>();
    [SerializeField] int maxrooms = 15;
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
            CheckAllNeighbours(roomQueue.Dequeue());
            if (roomQueue.Peek().roomType == RoomTypes.START)
            {
                i--;
            }
            if (deadEndUsed)
            {
                i--;
                deadEndUsed = false;
            }
        }
        Debug.Log(currentnumberOfRooms);
    }

    public void CheckAllNeighbours(Room r)
    {
        bool addedNeighbour = false;

        if (CheckNeighbour(r, Dir.UP))
        {
            AddNeighbour(r, Dir.UP);
            addedNeighbour = true;
            attemptCounter = 0;
        }
        else if (CheckNeighbour(r, Dir.DOWN))
        {
            AddNeighbour(r, Dir.DOWN);
            addedNeighbour = true;
            attemptCounter = 0;
        }
        else if (CheckNeighbour(r, Dir.LEFT))
        {
            AddNeighbour(r, Dir.LEFT);
            addedNeighbour = true;
            attemptCounter = 0;
        }
        else if (CheckNeighbour(r, Dir.RIGHT))
        {
            AddNeighbour(r, Dir.RIGHT);
            addedNeighbour = true;
            attemptCounter = 0;
        }
       

        if (!addedNeighbour && currentnumberOfRooms < NumberOfRooms)
        {
            if (attemptCounter < maxAttempts)
            {
                roomQueue.Enqueue(startRoom);
                attemptCounter++;
            }
            else
            {
                deadEndUsed = true;
                roomQueue.Enqueue(deadEnds.Peek());
            }
        }
    }

    public void AddNeighbour(Room r, Dir direction)
    {
        Room neighbour = GetNeighbour(r, direction);

        // Verificar si el vecino es válido (no null)
        if (neighbour != null)
        {
            neighbour.ocupied = true;
            neighbour.roomType = RoomTypes.EMPTY;
            roomQueue.Enqueue(neighbour);
            currentnumberOfRooms++;
            Instantiate(DebugCube, new Vector3(neighbour.x * 10, 0, neighbour.y * 10), Quaternion.identity);
        }
    }
    public bool CheckNeighbour(Room r, Dir d)
    {
        Room neighbour = GetNeighbour(r, d);

        // Si el vecino es null (fuera de los límites), no es válido
        if (neighbour == null)
        {
            return false;
        }

        int ocupiedNeighboursNeighbours = 0;

        // Verificar los vecinos del vecino
        if (GetNeighbour(neighbour, Dir.UP) != null && GetNeighbour(neighbour, Dir.UP).ocupied)
            ocupiedNeighboursNeighbours++;
        if (GetNeighbour(neighbour, Dir.DOWN) != null && GetNeighbour(neighbour, Dir.DOWN).ocupied)
            ocupiedNeighboursNeighbours++;
        if (GetNeighbour(neighbour, Dir.LEFT) != null && GetNeighbour(neighbour, Dir.LEFT).ocupied)
            ocupiedNeighboursNeighbours++;
        if (GetNeighbour(neighbour, Dir.RIGHT) != null && GetNeighbour(neighbour, Dir.RIGHT).ocupied)
            ocupiedNeighboursNeighbours++;

        System.Random random = new System.Random();
        bool randomBool = random.NextDouble() >= 0.5;
        bool ocupied = !neighbour.ocupied;
        bool neighbourneighbour = ocupiedNeighboursNeighbours < 3;
        bool roomsLeft = currentnumberOfRooms < NumberOfRooms;

        if (!(ocupied && neighbourneighbour && roomsLeft) && r.roomType != RoomTypes.START)
        {
            deadEnds.Enqueue(r);
        }

        return ocupied && neighbourneighbour && roomsLeft && randomBool;
    }
    public Room GetNeighbour(Room r, Dir d)
    {
        int x = r.x;
        int y = r.y;

        switch (d)
        {
            case Dir.UP:
                y += 1;
                break;
            case Dir.DOWN:
                y -= 1;
                break;
            case Dir.LEFT:
                x -= 1;
                break;
            case Dir.RIGHT:
                x += 1;
                break;
            default:
                Debug.LogError("Dirección no válida");
                return null;
        }

        // Verificar si las coordenadas están dentro de los límites de la matriz
        if (x >= 0 && x < 20 && y >= 0 && y < 20)
        {
            return floorPlan[x, y];
        }
        else
        {
            // Si está fuera de los límites, devolver null o manejar el caso según sea necesario
            return null;
        }
    }
}
