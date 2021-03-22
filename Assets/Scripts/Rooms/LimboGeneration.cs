using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Linq;



public class LimboGeneration : MonoBehaviour
{

    public int roomCount = 10;
    int[,] gameMap;
    Room[] roomMap;
    public int roomWidth;
    public int roomHeight;
    public Tile[] tiles;
    public bool roomChangeTrigger = true;


    public GameObject gridObject;
    public GameObject door;
    public GameObject player;
    public int currentRoom = 0;
    Grid grid;
    public Tilemap tileMap;
    public Tilemap tileMapCollision;
    void Start()
    {
        loadObjects();
        gameMap = new int[50, 50];
        roomMap = new Room[roomCount];
        setupGameMap();
        setupRooms();
        drawRooms();
        drawDoors();

        GameObject.Find("Main Camera").transform.position = new Vector3((roomWidth / 2) + roomWidth + 2, (roomHeight / 2) + roomHeight + 2, -10);
        GameObject p = Instantiate(player, new Vector3(roomWidth + roomWidth / 2.0f + 2.5f, roomHeight + 2.5f, -0.1f), Quaternion.identity);
        p.name = "Player";
        p.GetComponent<PlayerCombatController>().noBow = true;
        p.GetComponent<PlayerCombatController>().noSword = true;

        print("player created");
        Door.doorLock = true;
    }
    


    public void changeRooms(bool isForward)
    {
        if (isForward)
        {
            currentRoom += 1;

            int tempX = roomMap[currentRoom].entrance.x;
            int tempY = roomMap[currentRoom].entrance.y;
            Vector3 newPos = grid.CellToWorld(new Vector3Int(tempX, tempY, 0));
            newPos.x += 0.5f;
            newPos.y += 0.5f;
            newPos.z = -0.1f;
            GameObject.Find("Player").transform.position = newPos;
            GameObject.Find("Move Point").transform.position = newPos;

            GameObject cam = GameObject.Find("Main Camera");
            Vector3 newCamPos = new Vector3(cam.transform.position.x + (roomWidth * 2), cam.transform.position.y, cam.transform.position.z);
            cam.transform.position = newCamPos;
            print("asdf");
            GameObject.Find("Virgil").GetComponent<Virgil>().fix();

            GameObject.Find("Virgil").GetComponent<Virgil>().next();

        }
        else
        {
            currentRoom--;
            print(currentRoom);

            int tempX = roomMap[currentRoom].exit.x;
            int tempY = roomMap[currentRoom].exit.y;
            Vector3 newPos = grid.CellToWorld(new Vector3Int(tempX, tempY, 0));
            newPos.x += 0.5f;
            newPos.y += 0.5f;
            GameObject.Find("Player").transform.position = newPos;
            GameObject.Find("Move Point").transform.position = newPos;
            GameObject cam = GameObject.Find("Main Camera");
            Vector3 newCamPos = new Vector3(cam.transform.position.x - (roomWidth * 2), cam.transform.position.y, cam.transform.position.z);
            cam.transform.position = newCamPos;
        }
    }
    void drawDoors()
    {
        for (int i = 0; i < roomCount; ++i)
        {
            Room currentRoom = roomMap[i];
            bool isExit = true;
            if (i == 0)
            {
                isExit = false;
            }


            for (int j = 0; j < currentRoom.connectedRooms.Count; j++)
            {
                int tempX = currentRoom.mapCoord.x - currentRoom.connectedRooms[j].x;
                int tempY = currentRoom.mapCoord.y - currentRoom.connectedRooms[j].y;


                if (tempX == 1 && tempY == 0)
                {
                    if (isExit)
                    {
                        isExit = false;
                        roomMap[i].entrance = new Coords((currentRoom.roomID * roomWidth * 2) + roomWidth + 1, (roomHeight / 2) + roomHeight + 2);

                        Vector3 temp = grid.CellToWorld(new Vector3Int((currentRoom.roomID * roomWidth * 2) + roomWidth + 1, (roomHeight / 2) + roomHeight + 2, 0));
                        tileMapCollision.SetTile(new Vector3Int((currentRoom.roomID * roomWidth * 2) + roomWidth + 1, (roomHeight / 2) + roomHeight + 2, 0), null);
                        temp.x += 0.5f;
                        temp.y += 0.5f;
                        Quaternion rotation = Quaternion.Euler(0, 0, 0);
                        GameObject d = Instantiate(door, temp, rotation);
                        d.GetComponent<Door>().isExit = false;

                    }
                    else
                    {
                        roomMap[i].exit = new Coords((currentRoom.roomID * roomWidth * 2) + roomWidth + 1, (roomHeight / 2) + roomHeight + 2);

                        Vector3 temp = grid.CellToWorld(new Vector3Int((currentRoom.roomID * roomWidth * 2) + roomWidth + 1, (roomHeight / 2) + roomHeight + 2, 0));
                        tileMapCollision.SetTile(new Vector3Int((currentRoom.roomID * roomWidth * 2) + roomWidth + 1, (roomHeight / 2) + roomHeight + 2, 0), null);

                        temp.x += 0.5f;
                        temp.y += 0.5f;
                        Quaternion rotation = Quaternion.Euler(0, 0, 0);
                        GameObject d = Instantiate(door, temp, rotation);
                        d.GetComponent<Door>().isExit = true;

                    }

                }
                else if (tempX == 0 && tempY == 1)
                {
                    if (isExit)
                    {
                        isExit = false;
                        roomMap[i].entrance = new Coords((currentRoom.roomID * roomWidth * 2) + (roomWidth / 2) + roomWidth + 2, (roomHeight) + roomHeight + 2);
                        Vector3 temp = grid.CellToWorld(new Vector3Int((currentRoom.roomID * roomWidth * 2) + (roomWidth / 2) + roomWidth + 2, (roomHeight) + roomHeight + 2, 0));
                        tileMapCollision.SetTile(new Vector3Int((currentRoom.roomID * roomWidth * 2) + (roomWidth / 2) + roomWidth + 2, (roomHeight) + roomHeight + 2, 0), null);

                        temp.x += 0.5f;
                        temp.y += 0.5f;
                        Quaternion rotation = Quaternion.Euler(0, 0, 270);
                        GameObject d = Instantiate(door, temp, rotation);
                        d.GetComponent<Door>().isExit = false;

                    }
                    else
                    {
                        roomMap[i].exit = new Coords((currentRoom.roomID * roomWidth * 2) + (roomWidth / 2) + roomWidth + 2, (roomHeight) + roomHeight + 2);
                        Vector3 temp = grid.CellToWorld(new Vector3Int((currentRoom.roomID * roomWidth * 2) + (roomWidth / 2) + roomWidth + 2, (roomHeight) + roomHeight + 2, 0));
                        tileMapCollision.SetTile(new Vector3Int((currentRoom.roomID * roomWidth * 2) + (roomWidth / 2) + roomWidth + 2, (roomHeight) + roomHeight + 2, 0), null);

                        temp.x += 0.5f;
                        temp.y += 0.5f;
                        Quaternion rotation = Quaternion.Euler(0, 0, 270);
                        GameObject d = Instantiate(door, temp, rotation);
                        d.GetComponent<Door>().isExit = true;

                    }

                }
                else if (tempX == -1 && tempY == 0)
                {
                    if (isExit)
                    {
                        isExit = false;
                        roomMap[i].entrance = new Coords((currentRoom.roomID * roomWidth * 2) + roomWidth + roomWidth + 2, (roomHeight / 2) + roomHeight + 2);
                        Vector3 temp = grid.CellToWorld(new Vector3Int((currentRoom.roomID * roomWidth * 2) + roomWidth + roomWidth + 2, (roomHeight / 2) + roomHeight + 2, 0));
                        tileMapCollision.SetTile(new Vector3Int((currentRoom.roomID * roomWidth * 2) + roomWidth + roomWidth + 2, (roomHeight / 2) + roomHeight + 2, 0), null);

                        temp.x += 0.5f;
                        temp.y += 0.5f;
                        Quaternion rotation = Quaternion.Euler(0, 0, 180);
                        GameObject d = Instantiate(door, temp, rotation);
                        d.GetComponent<Door>().isExit = false;

                    }
                    else
                    {
                        roomMap[i].exit = new Coords((currentRoom.roomID * roomWidth * 2) + roomWidth + roomWidth + 2, (roomHeight / 2) + roomHeight + 2);
                        Vector3 temp = grid.CellToWorld(new Vector3Int((currentRoom.roomID * roomWidth * 2) + roomWidth + roomWidth + 2, (roomHeight / 2) + roomHeight + 2, 0));
                        tileMapCollision.SetTile(new Vector3Int((currentRoom.roomID * roomWidth * 2) + roomWidth + roomWidth + 2, (roomHeight / 2) + roomHeight + 2, 0), null);

                        temp.x += 0.5f;
                        temp.y += 0.5f;
                        Quaternion rotation = Quaternion.Euler(0, 0, 180);
                        GameObject d = Instantiate(door, temp, rotation);
                        d.GetComponent<Door>().isExit = true;

                    }

                }
                else if (tempX == 0 && tempY == -1)
                {
                    if (isExit)
                    {
                        isExit = false;
                        roomMap[i].entrance = new Coords((currentRoom.roomID * roomWidth * 2) + roomWidth + 2 + (roomWidth / 2), 0 + roomHeight + 1);
                        Vector3 temp = grid.CellToWorld(new Vector3Int((currentRoom.roomID * roomWidth * 2) + roomWidth + 2 + (roomWidth / 2), 0 + roomHeight + 1, 0));
                        tileMapCollision.SetTile(new Vector3Int((currentRoom.roomID * roomWidth * 2) + roomWidth + 2 + (roomWidth / 2), 0 + roomHeight + 1, 0), null);

                        temp.x += 0.5f;
                        temp.y += 0.5f;
                        Quaternion rotation = Quaternion.Euler(0, 0, 90);
                        GameObject d = Instantiate(door, temp, rotation);
                        d.GetComponent<Door>().isExit = false;
                    }
                    else
                    {
                        roomMap[i].exit = new Coords((currentRoom.roomID * roomWidth * 2) + roomWidth + 2 + (roomWidth / 2), 0 + roomHeight + 1);
                        Vector3 temp = grid.CellToWorld(new Vector3Int((currentRoom.roomID * roomWidth * 2) + roomWidth + 2 + (roomWidth / 2), 0 + roomHeight + 1, 0));
                        tileMapCollision.SetTile(new Vector3Int((currentRoom.roomID * roomWidth * 2) + roomWidth + 2 + (roomWidth / 2), 0 + roomHeight + 1, 0), null);

                        temp.x += 0.5f;
                        temp.y += 0.5f;
                        Quaternion rotation = Quaternion.Euler(0, 0, 90);
                        GameObject d = Instantiate(door, temp, rotation);
                        d.GetComponent<Door>().isExit = true;

                    }

                }
            }
        }
    }
    void loadObjects()
    {
        grid = gridObject.GetComponent<Grid>();
    }
    void drawRooms()
    {
        int tileXOffset = roomWidth + 2;
        int tileYOffset = roomHeight + 2;
        for (int i = 0; i < roomCount; ++i)
        {
            Room currentRoom = roomMap[i];
            for (int a = -2; a <= roomWidth+1; ++a)
            {
                for (int b = -2; b <= roomHeight+1; ++b)
                {
                    if (a <= -1)
                    {
                        tileMapCollision.SetTile(new Vector3Int(a + tileXOffset, b + tileYOffset, 0), tiles[1]);
                    }
                    else if (a >= roomWidth)
                    {
                        tileMapCollision.SetTile(new Vector3Int(a + tileXOffset, b + tileYOffset, 0), tiles[1]);
                    }
                    else if (b <= -1)
                    {
                        tileMapCollision.SetTile(new Vector3Int(a + tileXOffset, b + tileYOffset, 0), tiles[1]);
                    }
                    else if (b >= roomHeight)
                    {
                        tileMapCollision.SetTile(new Vector3Int(a + tileXOffset, b + tileYOffset, 0), tiles[1]);
                    }
                    else
                    {
                        tileMap.SetTile(new Vector3Int(a + tileXOffset, b + tileYOffset, 0), tiles[0]);

                        if (currentRoom.grid[a, b] == 0)
                        {
                            tileMap.SetTile(new Vector3Int(a + tileXOffset, b + tileYOffset, 0), tiles[0]);
                        }
                        else if (currentRoom.exit.x - 1 == a && currentRoom.exit.y - 1 == b)
                        {
                            print("this ran2");
                            //tileMap.SetTile(new Vector3Int(a + tileXOffset, b + tileYOffset, 0), tiles[5]);
                        }
                        else
                        {
                            //tileMap.SetTile(new Vector3Int(a + tileXOffset, b + tileYOffset, 0), tiles[5]);

                            //Instantiate(enemyType[currentRoom.grid[a, b] - 1], grid.CellToWorld(new Vector3Int(a + tileXOffset, b + tileYOffset, 0)), Quaternion.identity);
                        }
                    }
                }
            }
            tileXOffset += roomWidth * 2;
        }
    }

    void setupRooms()
    {
        roomMap[0].roomID = 0;
        roomMap[0].entrance = new Coords(roomWidth / 2, 0);

        for (int i = 1; i < roomCount; ++i)
        {
            //print("wtf");   
            roomMap[i].roomID = i;

        }
    }


    void setupGameMap()
    {
        int currentxpos = 25;
        int currentypos = 25;
        int roomIndex = 1;
        gameMap[currentxpos, currentypos] = roomIndex;
        roomMap[roomIndex - 1] = new Room();
        roomMap[roomIndex - 1].connectedRooms = new List<Coords>();
        roomMap[roomIndex - 1].grid = new int[roomWidth, roomHeight];
        roomMap[roomIndex - 1].mapCoord = new Coords(currentxpos, currentypos);
        int roomsLeft = roomCount - 1;
        while (roomsLeft > 0)
        {
            int chooseDirection = 0;
            switch (chooseDirection)
            {
                case 0:
                    ++currentxpos;
                    break;
                case 1:
                    --currentxpos;
                    break;
                case 2:
                    ++currentypos;
                    break;
                case 3:
                    --currentypos;
                    break;
                default:
                    print("Default case");
                    break;
            }
            if (gameMap[currentxpos, currentypos] == 0 && !(roomsLeft == roomCount - 1 && chooseDirection == 2))
            {
                roomIndex++;
                gameMap[currentxpos, currentypos] = roomIndex;
                roomMap[roomIndex - 1] = new Room();
                roomMap[roomIndex - 1].enemySpawned = false;
                roomMap[roomIndex - 1].grid = new int[roomWidth, roomHeight];
                roomMap[roomIndex - 1].mapCoord = new Coords(currentxpos, currentypos);
                roomMap[roomIndex - 1].connectedRooms = new List<Coords>();
                roomMap[roomIndex - 2].connectedRooms.Add(roomMap[roomIndex - 1].mapCoord);
                roomMap[roomIndex - 1].connectedRooms.Add(roomMap[roomIndex - 2].mapCoord);

                --roomsLeft;
            }
            else
            {
                switch (chooseDirection)
                {
                    case 0:
                        --currentxpos;
                        break;
                    case 1:
                        ++currentxpos;
                        break;
                    case 2:
                        --currentypos;
                        break;
                    case 3:
                        ++currentypos;
                        break;
                    default:
                        print("Default case");
                        break;
                }
            }
        }
    }
    Coords createDoor(int direction)
    {
        /*
        switch (direction)
        {
            case 0:
                return new Coords(Random.Range(1, roomWidth - 1), 0);
            case 1:
                return new Coords(Random.Range(1, roomWidth - 1), roomHeight);
            case 2:
                return new Coords(0, Random.Range(1, roomHeight - 1));
            case 3:
                return new Coords(roomWidth, Random.Range(1, roomHeight - 1));
            default:
                print("Default case");
                break;
        }*/
        switch (direction)
        {
            case 0:
                return new Coords(roomWidth, roomHeight / 2);
            case 1:
                return new Coords(roomWidth / 2, 0);
            case 2:
                return new Coords(roomWidth / 2, roomHeight);
            case 3:
                return new Coords(0, roomHeight / 2);
            default:
                print("Default case");
                break;
        }
        return new Coords();

    }
    Coords getDoor(Coords prevRoomDoor)
    {
        if (prevRoomDoor.x == roomWidth)
        {
            return new Coords(0, prevRoomDoor.y);
        }
        else if (prevRoomDoor.x == 0)
        {
            return new Coords(roomWidth, prevRoomDoor.y);
        }
        else if (prevRoomDoor.y == roomHeight)
        {
            return new Coords(prevRoomDoor.x, roomHeight);
        }
        else if (prevRoomDoor.y == 0)
        {
            return new Coords(prevRoomDoor.x, 0);
        }
        else
        {
            print("panic");
            return new Coords();
        }
    }
}