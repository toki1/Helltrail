using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Portal : MonoBehaviour
{
    private GameObject stats;

    // Start is called before the first frame update
    void Start()
    {
        stats = GameObject.Find("PlayerStats");
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        print("triggered");
        if (col.gameObject.tag.Equals("Player"))
        {
            SceneManager.LoadScene("StatScreen");
        }

    }

}
