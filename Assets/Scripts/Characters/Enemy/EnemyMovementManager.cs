using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Collections;
public class EnemyMovementManager : MonoBehaviour
{
    /* Movement Boolean Array size should be 4
     * Only set 1 as true!
     * 0 = towards the player
     * 1 = away from the player
     * 2 = randomly 
     * 3 = no movement
     */
    public bool[] movementType;

    public Transform player;
    float time = 0.0f;

    public float speed;
    public GameObject manager;
    public GameObject pathtest;
    public Transform movePoint;
    public LayerMask collideables;
    public int powerLevel;
    private float nextMovement = 0.0f;
    public float movementInterval = 2.0f;
    void Start()
    {
        /*
        int numTrue = 0;
        foreach (bool i in movementType)
        {
            if (i)
            {
                numTrue++;
            }
        }
        Assert.IsTrue(numTrue == 1);

        movePoint.parent = null;
        nextMovement = Time.time + Random.Range(0.0f, 3.0f);*/
        manager = GameObject.Find("Manager");
        time = 3;
    }

    /*
    // Changed to FixedUpdate
    void Update()
    {
        player = GameObject.Find("Player").transform;
        if(Time.time > nextMovement)
        {
            nextMovement += movementInterval;
            setMovement();
        }
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);
    } */
    bool lockk = false;
    Vector3 temp;
    List<Vector3Int> tempppp;
    bool first = true;
    void drawPath()
    {
        Vector3 start = transform.position;
        for(int i=1; i<tempppp.Count; ++i)
        {
            //Instantiate(pathtest, start, Quaternion.identity);
            start.x = start.x + (tempppp[i].x - tempppp[i - 1].x);
            start.y = start.y + (tempppp[i].y - tempppp[i - 1].y);
            print(start.x + " a " + start.y);
            //print(tempppp[i].x + " a " + tempppp[i].y);

        }
    }
    Vector2 target;
    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").transform;

        time += Time.deltaTime;

        if(time > 0.75)
        {
            tempppp = manager.GetComponent<CircleCreation>().pathFind(this.transform.position);
            if(tempppp.Count>=2)
            {
                //Coords reference = tempppp[0];
                Vector3Int next = tempppp[1];
                temp = setMovement();
                //print(temp);
                //rb.velocity = (new Vector2(, ) * speed;
                print(next.x + "   ||||  " + next.y);
                target = new Vector2(next.x+0.5f, next.y+0.5f);
                first = false;
            }

           
            // move sprite towards the target location
            time = 0;
        }
        else if(!first)
        {
            float step = 2 * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target, step);

            //rb.velocity = new Vector2(0, 0);
        }/*
        if (time > 12)
        {
           //tempppp = manager.GetComponent<CircleCreation>().pathFind(this.transform.position);
            //foreach(Coords t in tempppp)
            {
                //print(t.x + "  " + t.y);
            }
            lockk = false;
            time = 0;
        }*/
        /*
        player = GameObject.Find("Player").transform;
        if (Time.time > nextMovement)
        {
            nextMovement += movementInterval;
            setMovement();
        }
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);*/
    }

    private Vector3 setMovement()
    {

        if(lockk)
        {
            return temp;
        }
        lockk = true;
        if (manager == null)
        {
            return getMovement();
        }
        else
        {
            return getMovement();
        }
    }
    private void getMovementPF()
    {

    }
    bool testDirection(int x, int y,int step, int direction)
    {
        int temp = manager.GetComponent<CircleCreation>().currentRoom;
        Room[] r =  manager.GetComponent<CircleCreation>().roomMap;

        switch (direction)
        {
            case 1:
                if(y+1 < 6 && r[temp].grid[x, y+1]>0 && r[temp].visitedgrid[x, y + 1]==step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 3:
                if (y - 1 > -1 && r[temp].grid[x, y - 1] > 0 && r[temp].visitedgrid[x, y - 1] == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 2:
                if (x + 1 < 10 && r[temp].grid[x+1, y + 1] > 0 && r[temp].visitedgrid[x+1, y] == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 4:
                if (x - 1 > -1 && r[temp].grid[x -1, y ] > 0 && r[temp].visitedgrid[x-1, y] == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }

        }
        return false;
    }
    void initialSetup()
    {

    }
    void setVisited(int x, int y, int step)
    {
        int temp = manager.GetComponent<CircleCreation>().currentRoom;
        Room[] r = manager.GetComponent<CircleCreation>().roomMap;
        Room current = r[temp];
        if(current.grid[x,y] >=0)
        {
            current.visitedgrid[x,y] = step;
        }
    }
    void setDistance()
    {
        int temp = manager.GetComponent<CircleCreation>().currentRoom;
        Room[] r = manager.GetComponent<CircleCreation>().roomMap;
        Room current = r[temp];
        initialSetup();
        int x = 0;
        int y = 0;
        for(int step = 1; step<60; ++step)
        {
            for(int i=0; i<10; ++i)
            {
                for(int j=0; j<6; ++j )
                {

                    if(current.visitedgrid[i,j] == step -1)
                    {
                        test(i, j, step);

                    }
                }
            }
        }
    }
    void SetPath()
    {
        int step;
        int x = 5;
        int y = 5;
    }
    void test(int x, int y, int step)
    {
        if(testDirection(x,y,-1,1))
        {
            setVisited(x, y + 1, step);
        }
        if (testDirection(x, y, -1, 2))
        {
            setVisited(x, y + 1, step);
        }
        if (testDirection(x, y, -1, 3))
        {
            setVisited(x, y + 1, step);
        }
        if (testDirection(x, y, -1, 4))
        {
            setVisited(x, y + 1, step);
        }
    }
    private Vector3 getMovement()
	{
        if(movementType[0])
		{
            return directionTowardsPlayer();
        }
        else if (movementType[1])
        {
            return directionAwayFromPlayer();
        }
        else if (movementType[2])
        {
            return directionRandom();
        }

        return new Vector3(0f, 0f, 0f);
    }

    private Vector3 directionTowardsPlayer()
    {
        Vector3 dir = player.position - transform.position;
        //Debug.Log(dir.x + " " + dir.y);
        if (Mathf.Abs(dir.x) < 1.0f && Mathf.Abs(dir.y) < 1.0f)
        {
            dir.x = 0.0f;
            dir.y = 0.0f;
        }
        else
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    dir.x = 1.0f;
                }
                else
                {
                    dir.x = -1.0f;
                }

                dir.y = 0.0f;
            }
            else
            {
                if (dir.y > 0)
                {
                    dir.y = 1.0f;
                }
                else
                {
                    dir.y = -1.0f;
                }

                dir.x = 0.0f;
            }


        }

        return dir;
    }

    private Vector3 directionAwayFromPlayer()
    {
        
        return directionTowardsPlayer() * -1.0f;
    }

    private Vector3 directionRandom()
    {
        return new Vector3(Mathf.Round(Random.Range(-1f, 1f)) * Mathf.Round(Random.Range(0, speed)), Mathf.Round(Random.Range(-1f, 1f)) * Mathf.Round(Random.Range(0, speed)), 0f);
    }
}


