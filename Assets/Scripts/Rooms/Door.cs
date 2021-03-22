using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    public int direction;
    public bool isExit;
    GameObject roomManager;
    public static bool switchh = true;
    public static bool doorLock = false;
    void Start()
    {
        roomManager = GameObject.Find("Manager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag.Equals("Player"))
        {
            if(switchh && !doorLock)
            {
                if (roomManager.GetComponent<LimboGeneration>() == null)
                {
                    roomManager.GetComponent<CircleCreation>().changeRooms(isExit);
                    switchh = true;

                }
                else if (isExit)
                {
                    roomManager.GetComponent<LimboGeneration>().changeRooms(isExit);
                    doorLock = true;
                    switchh = true;

                }

            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (roomManager.GetComponent<LimboGeneration>() == null)
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                if (switchh)
                {
                    switchh = false;
                }
                else
                {
                    switchh = true;
                }
            }
        }
        else
        {
            if (collision.gameObject.tag.Equals("Player") && !isExit)
            {
                if (switchh)
                {
                    switchh = false;
                }
                else
                {
                    switchh = true;
                }
                switchh = true;

            }
        }

    }
}
