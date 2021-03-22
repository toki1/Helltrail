using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterStats : MonoBehaviour
{
    Stats pStats;
    void Start()
    {
        pStats = GameObject.Find("PlayerStats").GetComponent<Stats>();
    }

    public void nextScene()
    {
        if(pStats.gluttonyRan)
        {
            SceneLoader.GoToGluttonyBoss();
        }
        else if(pStats.limboRan)
        {
            SceneLoader.GoToGluttony();
        }
    }
}
