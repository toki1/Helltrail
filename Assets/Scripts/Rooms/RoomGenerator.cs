using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct Coords
{
    public Coords(int X, int Y)
    {
        x = X;
        y = Y;
    }

    public int x { get; }
    public int y { get; }
}
public class RoomGenerator : MonoBehaviour
{
    GameObject[] enemies;
    GameObject[] terrain;
    Stack<int> enemyInRoom;
    Stack<int> terrainInRoom;


    public RoomGenerator(GameObject[] enemyList, GameObject[] terrainList)
    {
        enemies = enemyList;
        terrain = terrainList;
        enemyInRoom = new Stack<int>();
        terrainInRoom = new Stack<int>();
    }
    public void generateRoom(Room room )
    {
        enemyInRoom.Clear();
        terrainInRoom.Clear();
        determineEnemies( room.difficulty);
        determineTerrain( room.numTerrain);
        placeEnemies(room.grid);
        placeTerrain(room.grid);
    }
    //determines where to place enemy.
    void placeEnemies(int[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);
        while(enemyInRoom.Count > 0)
        {
            int x = Random.Range(2, width - 2);
            int y = Random.Range(2, height - 2);
            if(grid[x,y] == 0)
            {

                grid[x, y] = enemyInRoom.Pop() + 1;
            }
        }
    }
    //determines where to place terrain
    void placeTerrain(int[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);
        while (terrainInRoom.Count > 0)
        {
            int x = Random.Range(1, width - 1);
            int y = Random.Range(1, height - 1);
            if (grid[x, y] == 0)
            {
                grid[x, y] = -1;
                terrainInRoom.Pop();
            }
        }
    }
    //determines how many terrain.
    void determineTerrain( int numTerrain)
    {

        int assigned = 0;
        while (assigned < numTerrain)
        {
            int terrainIndex = -1;

            assigned += 1; //terrain.area
            terrainInRoom.Push(terrainIndex);
        }
    }
    //determines what and how many enemies
    void determineEnemies( int difficulty)
    {
        int index = enemies.Length;

        int assigned = 0;
        while(assigned<difficulty)
        {
            int enemyIndex = Random.Range(0, index);

            assigned += enemies[enemyIndex].GetComponent<EnemyMovementManager>().powerLevel;
            enemyInRoom.Push(enemyIndex);
        }
    }
    int getSquareDistance(Coords coord1, Coords coord2)
    {
        return Mathf.Abs(coord1.x - coord2.x) + Mathf.Abs(coord1.y - coord2.y);
    }
}