using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Assertions;
public struct Room
{
    public int roomID;
    public Coords entrance;
    public Coords exit;
    public int difficulty;
    public int numEnemies;
    public int numTerrain;
    public int[,] grid;
    public int[,] visitedgrid;
    public Coords mapCoord;
    public List<Coords> connectedRooms;
    public bool enemySpawned;
}

public class CircleCreation : MonoBehaviour
{

    public int roomCount = 10;
    int[,] gameMap;
    public Room[] roomMap;
    public int roomWidth;
    public int roomHeight;
    public int[] numTerrain;
    public int[] numMonsters;
    public int[] roomDifficulty;
    public Tile[] tiles;
    public bool roomChangeTrigger = true;
    public GameObject[] enemyType;
    public GameObject[] terrainType;
    public Tile[] terrainTiles;

    public GameObject gridObject;
    public GameObject door;
    public GameObject player;
    public int currentRoom = 0;
    Grid grid;
    public Tilemap tileMap;
    public Tilemap tileMapCollision;
    public GameObject potion;
    public GameObject potion2;

    public GameObject potion3;

    void Start()
    {
        loadObjects();
        gameMap = new int[50, 50];
        roomMap = new Room[roomCount];
        setupGameMap();
        setupRooms();
        generateRooms();
        drawRooms();
        drawDoors();
        drawTerrain();
        drawEnemies(0);
        GameObject.Find("Main Camera").transform.position = new Vector3((roomWidth / 2) + roomWidth + 2, (roomHeight / 2) + roomHeight + 2, -10);
        GameObject p = Instantiate(player, new Vector3(roomWidth + roomWidth / 2.0f + 2.5f, roomHeight + 2.5f, 0), Quaternion.identity);
        p.name = "Player";
        print("player created");
        Door.doorLock = false;
    }

    void drawEnemies(int room)
    {
        //for (int i = 0; i < roomCount; ++i)
        {
            Room currentRoom = roomMap[room];
            if (roomMap[room].enemySpawned)
            {

                return;
            }
            roomMap[room].enemySpawned = true;
            for (int x = 0; x < roomWidth; ++x)
            {
                for (int y = 0; y < roomHeight; ++y)
                {
                    int c = currentRoom.grid[x, y];
                    //print(c);
                    if (c > 0)
                    {
                        Vector3 temp = grid.CellToWorld(new Vector3Int((currentRoom.roomID * roomWidth * 2) + x + roomWidth + 2, roomHeight + 2 + y, 0));
                        temp.x += 0.5f;
                        temp.y += 0.5f;
                        int tempp = Random.Range(0, 10);
                        GameObject enemy = Instantiate(enemyType[c - 1], temp, Quaternion.identity);

                        if (tempp < 2)
                        {
                            int tempp2 = Random.Range(0, 10);
                            if (tempp2 < 4)
                            {
                                enemy.GetComponent<EnemyHealthManager>().drop = potion;

                            }
                            else if (tempp2 < 7)
                            {
                                enemy.GetComponent<EnemyHealthManager>().drop = potion2;

                            }
                            else
                            {
                                enemy.GetComponent<EnemyHealthManager>().drop = potion3;

                            }
                        }
                        else if (tempp < 6)
                        {
                            enemy.GetComponent<EnemyHealthManager>().dropSoul = true;

                        }

                    }
                }
            }

        }
    }
    void drawTerrain()
    {
        for (int i = 0; i < roomCount; ++i)
        {
            Room currentRoom = roomMap[i];
            for (int x = 0; x < roomWidth; ++x)
            {
                for (int y = 0; y < roomHeight; ++y)
                {
                    int c = currentRoom.grid[x, y];
                    if (c < 0)
                    {
                        //Instantiate(terrainType[0], grid.CellToWorld(new Vector3Int((currentRoom.roomID * roomWidth * 2)+x+ 12, 12+y, 0)), Quaternion.identity);
                        tileMapCollision.SetTile(new Vector3Int((currentRoom.roomID * roomWidth * 2) + x + roomWidth + 2, roomHeight + 2 + y, 0), terrainTiles[Random.Range(0, 8)]);

                    }
                }
            }

        }
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

            GameObject.Find("Player").transform.position = newPos;
            GameObject.Find("Move Point").transform.position = newPos;

            GameObject cam = GameObject.Find("Main Camera");
            Vector3 newCamPos = new Vector3(cam.transform.position.x + (roomWidth * 2), cam.transform.position.y, cam.transform.position.z);
            cam.transform.position = newCamPos;
            drawEnemies(currentRoom);
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
            for (int a = -2; a <= roomWidth + 1; ++a)
            {
                for (int b = -2; b <= roomHeight + 1; ++b)
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
        roomMap[0].numEnemies = numMonsters[0];
        roomMap[0].numTerrain = numTerrain[0];
        roomMap[0].difficulty = roomDifficulty[0];
        for (int i = 1; i < roomCount; ++i)
        {
            //print("wtf");   
            roomMap[i].roomID = i;
            roomMap[i].numEnemies = numMonsters[i];
            roomMap[i].numTerrain = numTerrain[i];
            roomMap[i].difficulty = roomDifficulty[i];
        }
    }
    void generateRooms()
    {
        RoomGenerator roomGen = new RoomGenerator(enemyType, terrainType);
        for (int i = 0; i < roomCount; ++i)
        {
            roomGen.generateRoom(roomMap[i]);
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
            int chooseDirection = Random.Range(0, 4);
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

    public List<Vector3Int> pathFind(Vector3 currentPos)
    {
        Node[,] nodes = new Node[roomWidth, roomHeight];
        print(currentRoom);
        int tileXOffset = (roomWidth + 2 )+  ((roomWidth*2)*(currentRoom)) ;
        int tileYOffset = roomHeight + 2;
        for (int i = 0; i < roomWidth; ++i)
        {
            for (int j = 0; j < roomHeight; ++j)
            {
                nodes[i, j] = new Node(roomMap[currentRoom].grid[i, j] >= 0, i, j);

            }
        }

        Vector3Int target = grid.WorldToCell(GameObject.Find("Player").transform.position);
        Vector3Int start = grid.WorldToCell(currentPos);

        print(target.x + "        " + target.y);
        print(tileXOffset + "      sdfasdfsa  ");

        Coords to = new Coords(target.x - tileXOffset, target.y -tileYOffset);
        Coords from = new Coords(start.x - tileXOffset, start.y - tileYOffset);
        print(to.x + "  " + to.y);
        print(from.x + "   " + from.y);

        List<Coords> path = FindPath(nodes, from, to);
        print(path.Count);
        path.Insert(0, from);
        path.RemoveAt(path.Count - 1);

        List<Vector3Int> positions = new List<Vector3Int>();


        foreach(Coords c in path)
        {
            int cx = c.x + tileXOffset;
            int cy = c.y + tileYOffset;

            positions.Add(new Vector3Int(cx, cy, 0));
        }



        return positions;

    }

    public List<Coords> FindPath(Node[,] nodes, Coords startPos, Coords targetPos)
    {
        List<Node> nodes_path = helper(nodes, startPos, targetPos);

        List<Coords> ret = new List<Coords>();
        if (nodes_path != null)
        {
            foreach (Node node in nodes_path)
            {
                ret.Add(new Coords(node.gridX, node.gridY));
            }
        }
        return ret;
    }

    private List<Node> helper(Node[,] nodes, Coords startPos, Coords targetPos)
    {
        print(startPos.x);

        print(targetPos.x + "         " + targetPos.y);


        if (startPos.x >= roomWidth|| startPos.x < 0)
        {
            print(targetPos.x + "      1   " + targetPos.y);

            return null;
        }
        if (targetPos.x >= roomWidth || targetPos.x < 0)
        {
            print(targetPos.x + "     2    " + targetPos.y);

            return null;
        }
        if (targetPos.y >= roomHeight || targetPos.y < 0)
        {
            print(targetPos.x + "     3    " + targetPos.y);

            return null;
        }
        Node startNode = nodes[startPos.x, startPos.y];

        Node targetNode = nodes[targetPos.x, targetPos.y];

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost() <= currentNode.fCost()  && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                List<Node> path = new List<Node>();
                Node temp = targetNode;

                while (temp != startNode)
                {
                    path.Add(temp);
                    temp = temp.parent;
                }
                path.Reverse();
                return path;
            }

            foreach (Node neighbour in GetNeighbours(nodes, currentNode))
            {
                if (!neighbour.walkable || closedList.Contains(neighbour))
                {
                    closedList.Add(neighbour);
                    continue;
                }

                int newCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newCost < neighbour.gCost|| !openList.Contains(neighbour))
                {
                    neighbour.gCost = newCost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openList.Contains(neighbour))
                        openList.Add(neighbour);
                }
            }
        }

        return null;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int x = System.Math.Abs(nodeA.gridX - nodeB.gridX);
        int y = System.Math.Abs(nodeA.gridY - nodeB.gridY);
        if (x > y)
        {
            return  y +  (x - y);
        }
        else
        {
            return  x +  (y - x);
        }
    }
    public List<Node> GetNeighbours(Node[,] nodes, Node node)
    {
        List<Node> list = new List<Node>();
        var temp = check(-1, 0, node, nodes);
        if (temp != null)
            list.Add(temp);
        temp = check(1, 0, node, nodes);
        if (temp != null)
            list.Add(temp);
        temp = check(0, 1, node, nodes);
        if (temp != null)
            list.Add(temp);
        temp = check(0, -1, node, nodes);
        if (temp != null)
            list.Add(temp);
        return list;

    }
    Node check(int x, int y, Node node, Node[,] nodes)
    {
        int checkX = node.gridX + x;
        int checkY = node.gridY + y;

        if (checkX >= 0 && checkX < roomWidth && checkY >= 0 && checkY < roomHeight)
        {
            return nodes[checkX, checkY];
        }
        return null;
    }
}
public class Node
{
    public bool walkable;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;


    public Node(bool obstacle, int x, int y)
    {
        walkable = obstacle;
        gridX = x;
        gridY = y;
    }


    public int fCost()
    {
        return gCost + hCost;    
    }

}