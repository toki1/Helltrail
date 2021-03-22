using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public GameObject Virgil;
    public float MoveSpeed;
    int currentNode;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void moveNext()
    {
        ++currentNode;
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * MoveSpeed;
        if(currentNode < transform.childCount)
        {
            Virgil.transform.position = Vector3.Lerp(Virgil.transform.position, transform.GetChild(currentNode).transform.position, time);
            //transform.GetChild(currentNode).transform
        }
        
    }
}
