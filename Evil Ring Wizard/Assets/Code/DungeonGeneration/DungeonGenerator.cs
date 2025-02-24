using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class DungeonGenerator : MonoBehaviour
{
    List<GameObject> roomPrefabs;
    List<DungeonRoom> _dungeonRooms;
    int maxRooms;
    private enum ROOM_DIRECTIONS
    {
        UP = 0,
        RIGHT,
        DOWN,
        LEFT
    }
    private class DungeonRoom
    {
        public int xPosition;
        public int zPosition;

        public int NeighboursCount
        {
            get
            {
                return _neighbours.Count;
            }
        }

        private List<Tuple<ROOM_DIRECTIONS, DungeonRoom>> _neighbours;
        public List<Tuple<ROOM_DIRECTIONS, DungeonRoom>> Neighbours{ get{ return _neighbours; } }

        public RoomTypes type = RoomTypes.INVALID;

        public DungeonRoom(int x, int z)
        {
            this.xPosition = x;
            this.zPosition = z;
        }

        public bool HasNeighbourInDirection(ROOM_DIRECTIONS directions)
        {

        }
        public void AddNeighbourInDirection(DungeonRoom room, ROOM_DIRECTIONS direction) 
        {

        }
    }

    private void Awake()
    {
        LoadRoomPrefabs();
    }

    private void LoadRoomPrefabs()
    {
        string roomsPath = "Prefabs/Rooms/";
        string[] roomPrefabNames = { "Room_Door_1", "Room_Door_2_Close", "Room_Door_2_Opposite", "Room_Door_3", "Room_Door_4" };
        roomPrefabs = new List<GameObject>();

        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < roomPrefabNames.Length; i++)
        {
            sb.Append(roomsPath).Append(roomPrefabNames[i]);
            GameObject room = Resources.Load<GameObject>(sb.ToString());
            if (!ReferenceEquals(room,null))
            {
                roomPrefabs.Add(room);
            }
            else
            {
                Debug.LogError("Room prefab " + sb.ToString() + "could not be found in" + roomsPath);
            }
            sb.Clear();
        }
    }
    void Start()
    {
        GenerateDungeon();
        GenerateSpecialRooms();

        InstantiateDungeon();

        SpawnEnemies();
        SpawnSpecialRooms();
    }

    private void GenerateDungeon()
    {
        _dungeonRooms = new List<DungeonRoom>();
        maxRooms = GetDungeonMaxRoomCount();
        nCu
    }
    private void GenerateSpecialRooms()
    {

    }
    private void InstantiateDungeon()
    {

    }
    private void SpawnEnemies()
    {

    }
    private void SpawnSpecialRooms()
    {

    }
}
