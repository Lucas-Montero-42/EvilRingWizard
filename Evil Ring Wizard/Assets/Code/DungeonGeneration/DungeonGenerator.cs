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

    public GameObject ExitDoor;
    public Room[,] floorPlan = new Room[20, 20];
    [SerializeField]private List<GameObject> roomPrefabs;
    [SerializeField]private List<GameObject> oneDoorRooms;
    [SerializeField]private List<GameObject> twoDoorOpositeRooms;
    [SerializeField]private List<GameObject> twoDoorCloseRooms;
    [SerializeField]private List<GameObject> threeDoorRooms;
    [SerializeField]private List<GameObject> fourDoorRooms;
    public GameObject treasureChest;
    private Queue<Room> roomQueue = new Queue<Room>();
    private Queue<Room> deadEnds = new Queue<Room>();
    int maxrooms = 15;
    int minrooms = 7;
    [SerializeField] int NumberOfRooms;
    int currentnumberOfRooms = 1;
    public int Level = 1;
    Room startRoom;
    public List<GameObject> enemyPrefabs;

    public int roomSize = 12;
    float adjustingToGidDistance;

    private void Start()
    {
        if (roomQueue == null) roomQueue = new Queue<Room>();
        if (deadEnds == null) deadEnds = new Queue<Room>();
        floorPlan = new Room[20, 20];
        //Instantiate(DebugCube, new Vector3(roomQueue.Peek().x * roomSize + adjustingToGidDistance, 0, roomQueue.Peek().y * roomSize + adjustingToGidDistance), Quaternion.identity);
        int attemptCount = 0;
        while (!GenerateDungeon() && attemptCount < 100)  // Máximo 1000 intentos
        {
            attemptCount++;
            Debug.Log("Número de intentos: " + attemptCount);
            if (attemptCount >= 100)
            {
                Debug.LogError("Se alcanzó el máximo de intentos sin generar una mazmorras válida. Tienes muy mala suerte colega");
                return;
            }
        }
        //Debug.Log(attemptCount);
    }
    private bool GenerateDungeon()
    {
        Debug.Log("Generando mazmorras...");
        roomQueue = new Queue<Room>();
        deadEnds = new Queue<Room>();
        floorPlan = new Room[20, 20];
        if (floorPlan == null)
        {
            Debug.LogError("floorPlan no se inicializó correctamente.");
            return false;
        }
        adjustingToGidDistance = roomSize * -10;
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                floorPlan[x, y] = new Room(x,y, false);
            }
        }
        NumberOfRooms = Mathf.Clamp((int)(UnityEngine.Random.Range(0, 2) + 5 + Level * 2.6f), minrooms, maxrooms);
        currentnumberOfRooms = 1;
        startRoom = floorPlan[10, 10];
        if (startRoom == null)
        {
            Debug.LogError("startRoom no se inicializó correctamente.");
            return false; // redundante pero a estas alturas ya no se que mas hacer
        }
        roomQueue.Enqueue(startRoom);
        startRoom.ocupied = true;
        startRoom.roomType = RoomTypes.START;
        //Instantiate(DebugCube, new Vector3(roomQueue.Peek().x * roomSize + adjustingToGidDistance, 0, roomQueue.Peek().y * roomSize + adjustingToGidDistance), Quaternion.identity);
        for (int i = 1; i < NumberOfRooms; i++)
        {
            if (roomQueue.Count == 0)
            {
                Debug.LogError("roomQueue está vacío antes de generar todas las habitaciones.");
                return false; // Salir del método si no hay más habitaciones para procesar
            }
            Room currentRoom = roomQueue.Dequeue(); 
            CheckAllNeighbours(currentRoom);
            if (roomQueue.Count > 0 && roomQueue.Peek().roomType == RoomTypes.START)
            {
                i--;
            }
            if (deadEndUsed)
            {
                i--;
                deadEndUsed = false;
            }
        }

        if (!AssignBossRoom())
        {
            return false;
        }

        AssignSpecialRooms();

        //Debug.Log(currentnumberOfRooms);
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                if (floorPlan[x, y].ocupied)
                {
                    AsignRoomPrefab(floorPlan[x, y]);
                }
            }
        }
        return true;
    }
    public void CheckAllNeighbours(Room r)
    {
        bool addedNeighbour = false;

        // Primero intentamos explorar todos los vecinos
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

        // Si no se encontraron vecinos y hay más habitaciones que generar
        if (!addedNeighbour && currentnumberOfRooms < NumberOfRooms)
        {
            // Intentamos reusar una habitación de deadEnd
            if (attemptCounter < maxAttempts)
            {
                // Usamos la StartRoom si no hemos intentado demasiado
                roomQueue.Enqueue(startRoom);
                attemptCounter++;
            }
            // Si no se puede continuar desde StartRoom, tratamos con deadEnds
            else if (deadEnds.Count > 0)
            {
                deadEndUsed = true;  // Marcamos que se está usando un deadEnd
                roomQueue.Enqueue(deadEnds.Dequeue());  // Retomamos un deadEnd
                attemptCounter = 0;  // Reiniciamos los intentos
            }
            else
            {
                Debug.LogError("No hay más habitaciones disponibles para continuar.");
                return; // Salir del método si no hay más habitaciones disponibles
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
            //Instantiate(DebugCube, new Vector3(neighbour.x * roomSize + spaceBtwRooms, 0, neighbour.y * roomSize + spaceBtwRooms), Quaternion.identity);
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

        bool randomBool = UnityEngine.Random.value >= 0.5f;
        bool ocupied = !neighbour.ocupied;
        bool neighbourneighbour = ocupiedNeighboursNeighbours < 3;
        bool roomsLeft = currentnumberOfRooms < NumberOfRooms;

        if (!(ocupied && neighbourneighbour && roomsLeft) && r.roomType != RoomTypes.START)
        {
            if (deadEnds == null)
            {
                deadEnds = new Queue<Room>(); // Inicializar deadEnds si es null
            }

            if (!deadEnds.Contains(r))
            {
                deadEnds.Enqueue(r);
            }
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

        if (x >= 0 && x < 20 && y >= 0 && y < 20)
        {
            return floorPlan[x, y];
        }
        else
        {
            return null;
        }
    }
    public bool AssignBossRoom()
    {
        Room bossRoom = null;
        double distanceToBoss = -1;

        // Iteramos a través de todo el plano
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                // Comprobar si la habitación está ocupada
                if (floorPlan[x, y].ocupied)
                {
                    // Calcular la distancia de la habitación ocupada a la StartRoom
                    double distance = Math.Sqrt(Math.Pow(x - startRoom.x, 2) + Math.Pow(y - startRoom.y, 2));

                    // Si la distancia es mayor que la máxima registrada, actualizamos la habitación más alejada
                    if (distance > distanceToBoss)
                    {
                        distanceToBoss = distance;
                        bossRoom = floorPlan[x, y]; // Actualizamos la habitación más alejada
                    }
                }
            }
        }
        if (bossRoom == null || GetNumberOffNeighbours(bossRoom) !=1 )
        {
            return false;
        }
        bossRoom.roomType = RoomTypes.BOSS;
        Instantiate(ExitDoor, new Vector3(bossRoom.x * roomSize + adjustingToGidDistance, ExitDoor.transform.localScale.y - (roomSize / 2), bossRoom.y * roomSize + adjustingToGidDistance), Quaternion.identity);
        return true;
    }
    public void AssignSpecialRooms()
    {
        List<Room> ocupiedRooms = new List<Room>();
        Room treasureRoom;
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                if (floorPlan[x, y].ocupied && floorPlan[x, y].roomType != RoomTypes.START && floorPlan[x,y].roomType != RoomTypes.BOSS)
                {
                    ocupiedRooms.Add(floorPlan[x,y]);
                }
            }
        }
        if (ocupiedRooms.Count > 0)
        {
            treasureRoom = ocupiedRooms[UnityEngine.Random.Range(0, ocupiedRooms.Count)];
            treasureRoom.roomType = RoomTypes.TREASURE;
            Instantiate(treasureChest, new Vector3(treasureRoom.x * roomSize + adjustingToGidDistance, -roomSize / 2 + treasureChest.transform.localScale.y, treasureRoom.y * roomSize + adjustingToGidDistance), Quaternion.identity);
        }
        

    }
    public void AsignRoomPrefab(Room r)
    {
        bool[] disposition = new bool[4];
        for (int i = 0; i< disposition.Length;i++)
        {
            disposition[i] = false;
        }
        int numberOfNeighbours = 0;
        if (GetNeighbour(r, Dir.UP) != null && GetNeighbour(r, Dir.UP).ocupied)
        {
            disposition[0] = true;
            numberOfNeighbours++;
        }
        if (GetNeighbour(r, Dir.DOWN) != null && GetNeighbour(r, Dir.DOWN).ocupied)
        {
            disposition[1] = true;
            numberOfNeighbours++;
        }
        if (GetNeighbour(r, Dir.LEFT) != null && GetNeighbour(r, Dir.LEFT).ocupied)
        {
            disposition[2] = true;
            numberOfNeighbours++;
        }
        if (GetNeighbour(r, Dir.RIGHT) != null && GetNeighbour(r, Dir.RIGHT).ocupied)
        {
            disposition[3] = true;
            numberOfNeighbours++;
        }

        if (numberOfNeighbours == 1)
        {
            for (int i = 0; i < disposition.Length; i++)
            {
                if (disposition[i] == true)
                {
                    int rand = (int)UnityEngine.Random.Range(0, oneDoorRooms.Count - 1);

                    switch (i)
                    {
                        case 0:
                            //direction is up
                            Instantiate(oneDoorRooms[rand], new Vector3(r.x * roomSize + adjustingToGidDistance, 0, r.y * roomSize + adjustingToGidDistance), Quaternion.Euler(0, -90, 0)); // Cambiar rotación
                            break;
                        case 1:
                            //direction is down
                            Instantiate(oneDoorRooms[rand], new Vector3(r.x * roomSize + adjustingToGidDistance, 0, r.y * roomSize + adjustingToGidDistance), Quaternion.Euler(0, 90, 0)); // Cambiar rotación
                            break;
                        case 2:
                            //direction is left
                            Instantiate(oneDoorRooms[rand], new Vector3(r.x * roomSize + adjustingToGidDistance, 0, r.y * roomSize + adjustingToGidDistance), Quaternion.Euler(0,180,0)); // Cambiar rotación
                            break;
                        case 3:
                            //direction is right
                            Instantiate(oneDoorRooms[rand], new Vector3(r.x * roomSize + adjustingToGidDistance, 0, r.y * roomSize + adjustingToGidDistance), Quaternion.identity); // Cambiar rotación
                            break;
                        default:
                            break;
                    }
                    return;
                }
            }
        }
        else if (numberOfNeighbours == 2)
        {
            int rand = (int)UnityEngine.Random.Range(0, twoDoorOpositeRooms.Count - 1);
            int rand2 = (int)UnityEngine.Random.Range(0, twoDoorCloseRooms.Count - 1);

            if (disposition[0]==true && disposition[1]==true)
            {
                Instantiate(twoDoorOpositeRooms[rand], new Vector3(r.x * roomSize + adjustingToGidDistance, 0, r.y * roomSize + adjustingToGidDistance), Quaternion.Euler(0, 90, 0)); // Cambiar rotación
                // Direction is |
            }
            else if (disposition[2] == true && disposition[3] == true)
            {
                Instantiate(twoDoorOpositeRooms[rand], new Vector3(r.x * roomSize + adjustingToGidDistance, 0, r.y * roomSize + adjustingToGidDistance), Quaternion.identity); // Cambiar rotación
                // Direction is -
            }
            else if (disposition[0] == true && disposition[2] == true)
            {
                Instantiate(twoDoorOpositeRooms[rand], new Vector3(r.x * roomSize + adjustingToGidDistance, 0, r.y * roomSize + adjustingToGidDistance), Quaternion.Euler(0, -90, 0)); // Cambiar rotación
                // Direction is _|
            }
            else if (disposition[0] == true && disposition[3] == true) 
            {
                Instantiate(twoDoorCloseRooms[rand2], new Vector3(r.x * roomSize + adjustingToGidDistance, 0, r.y * roomSize + adjustingToGidDistance), Quaternion.identity); // Cambiar rotación
                // Direction is L
            }
            else if (disposition[1] == true && disposition[2] == true)
            {
                Instantiate(twoDoorCloseRooms[rand2], new Vector3(r.x * roomSize + adjustingToGidDistance, 0, r.y * roomSize + adjustingToGidDistance), Quaternion.Euler(0, 180, 0)); // Cambiar rotación
                // Direction is -|
            }
            else if (disposition[1] == true && disposition[3] == true)
            {
                Instantiate(twoDoorCloseRooms[rand2], new Vector3(r.x * roomSize + adjustingToGidDistance, 0, r.y * roomSize + adjustingToGidDistance), Quaternion.Euler(0, 90, 0)); // Cambiar rotación
                // Direction is |-
            }
            else
            {
                Debug.LogError("HELL NAH");
            }
        }
        else if (numberOfNeighbours == 3)
        {
            int rand = (int)UnityEngine.Random.Range(0, threeDoorRooms.Count - 1);

            if (disposition[0] == true && disposition[2] == true && disposition[3] == true)
            {
                Instantiate(threeDoorRooms[rand], new Vector3(r.x * roomSize + adjustingToGidDistance, 0, r.y * roomSize + adjustingToGidDistance), Quaternion.Euler(0, 180, 0)); // Cambiar rotación
                // Direction is _|_
            }
            else if (disposition[0] == true && disposition[1] == true && disposition[3] == true)
            {
                Instantiate(threeDoorRooms[rand], new Vector3(r.x * roomSize + adjustingToGidDistance, 0, r.y * roomSize + adjustingToGidDistance), Quaternion.Euler(0, -90, 0)); // Cambiar rotación
                // Direction is |-
            }

            else if (disposition[0] == true && disposition[1] == true && disposition[2] == true)
            {
                Instantiate(threeDoorRooms[rand], new Vector3(r.x * roomSize + adjustingToGidDistance, 0, r.y * roomSize + adjustingToGidDistance), Quaternion.Euler(0, 90, 0)); // Cambiar rotación
                // Direction is -|
            }
            else if (disposition[1] == true && disposition[2] == true && disposition[3] == true)
            {
                Instantiate(threeDoorRooms[rand], new Vector3(r.x * roomSize + adjustingToGidDistance, 0, r.y * roomSize + adjustingToGidDistance), Quaternion.identity); // Cambiar rotación
                // Direction is T
            }
            else
            {
                Debug.LogError("HELL NAH");
            }
        }
        else if (numberOfNeighbours == 4)
        {
            int rand = (int)UnityEngine.Random.Range(0, fourDoorRooms.Count - 1);
            Instantiate(fourDoorRooms[rand], new Vector3(r.x * roomSize + adjustingToGidDistance, 0, r.y * roomSize + adjustingToGidDistance), Quaternion.identity);
            // Direction is all
        }
    }
    public int GetNumberOffNeighbours(Room r)
    {
        int numberOfNeighbours = 0;
        if (GetNeighbour(r, Dir.UP) != null && GetNeighbour(r, Dir.UP).ocupied)
        {
            numberOfNeighbours++;
        }
        if (GetNeighbour(r, Dir.DOWN) != null && GetNeighbour(r, Dir.DOWN).ocupied)
        {
            numberOfNeighbours++;
        }
        if (GetNeighbour(r, Dir.LEFT) != null && GetNeighbour(r, Dir.LEFT).ocupied)
        {
            numberOfNeighbours++;
        }
        if (GetNeighbour(r, Dir.RIGHT) != null && GetNeighbour(r, Dir.RIGHT).ocupied)
        {
            numberOfNeighbours++;
        }
        return numberOfNeighbours;
    }
}
