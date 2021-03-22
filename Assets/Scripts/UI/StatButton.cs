using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatButton : MonoBehaviour
{
    private GameObject playerStats;
    private bool initStats = false;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.Find("PlayerStats");
        if (!initStats)
        {
            playerStats.GetComponent<Stats>().Start();
            initStats = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void upgradeStat()
    {
        if(gameObject.name.Equals("udamage"))
        {
            playerStats.GetComponent<Stats>().upgradeDamage();
        }
        else if(gameObject.name.Equals("uspeed"))
        {
            playerStats.GetComponent<Stats>().upgradeSpeed();
        }
        else
        {
            playerStats.GetComponent<Stats>().upgradeAttackRate();
        }
    }
}
