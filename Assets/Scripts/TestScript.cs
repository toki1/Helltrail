using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    RoomGenerator temp;
    public GameObject player;
    void Start()
    {
        GameObject p = Instantiate(player, new Vector3(0,0,0), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
