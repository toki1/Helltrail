using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static SoundManager soundManager;
    public static AudioClip limboSong;
    public static AudioClip gluttonySong;

    public static GameObject pause;
    public static GameObject controls;

    public static bool limboStarted;
    public static bool gluttonyStarted;
    public static bool bossStarted;

    public static void Start()
    {
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if(go.name.Equals("SoundManager"))
            {
                soundManager = go.GetComponent<SoundManager>();
            }
            else if(go.name.Equals("Menus"))
            {
                pause = go.transform.Find("Pause Screen").gameObject;
                controls = go.transform.Find("Controls Screen").gameObject;
            }
        }

        DisableActiveScreens();
        limboStarted = false;
        gluttonyStarted = false;
        bossStarted = false;

    }

    public static void GoToMenu()
    {
        GameObject[] results = GameObject.FindGameObjectsWithTag("Stats");
        for (int i = 0; i < results.Length; ++i)
        {
            if (results[i].GetComponent<Stats>() != null)
            {
                GameObject.Destroy(results[i]);
            }
        }
        // Clear stats if retrying
        GameObject.Find("PlayerStats").GetComponent<Stats>().clearSouls();
        DisableActiveScreens();
        soundManager.SwitchTrackCaller(0);
        SceneManager.LoadScene("Main Menu");
    }

    public static void GoToLimbo()
    {
        limboStarted = true;
        SceneManager.LoadScene("LimboV2");
        soundManager.SwitchTrackCaller(1);
    }

    public static void GoToGluttony()
    {
        gluttonyStarted = true;
        SceneManager.LoadScene("GluttonyV2");
        soundManager.SwitchTrackCaller(2);
    }

    public static void GoToGluttonyBoss()
    {
        bossStarted = true;
        SceneManager.LoadScene("FinalBoss 1");
        soundManager.SwitchTrackCaller(2);
    }

    public static void GoToWinScreen()
    {
        SceneManager.LoadScene("Win Screen");
        soundManager.SwitchTrackCaller(0);
    }

    public static void GoToDeathScreen()
    {
        SceneManager.LoadScene("Death Screen");
        soundManager.SwitchTrackCaller(0);
    }

    public static void GoToCredits()
    {
        SceneManager.LoadScene("Credits Screen");
        soundManager.SwitchTrackCaller(3);
    }

    public static void GoToDebug()
    {
        SceneManager.LoadScene("Debug Menu");
        soundManager.SwitchTrackCaller(0);
    }

    public static void GoToPeytonAnimation()
    {
        SceneManager.LoadScene("Peyton Animation Scene");
    }

    public static void GoToStatScreen()
    {
        SceneManager.LoadScene("StatScreen");
    }

    public static void GoToPause()
    {
        Debug.Log("go to pause");
        DisableActiveScreens();
        GameObject.Find("Menus").transform.Find("Pause Screen").gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public static void Resume()
    {
        DisableActiveScreens();
        Time.timeScale = 1;
    }

    public static void GoToControls()
    {
        Debug.Log("here");
        DisableActiveScreens();
        GameObject.Find("Menus").transform.Find("Controls Screen").gameObject.SetActive(true);
    }

    public static void exitGame()
    {
        Application.Quit();
    }

    public static void DisableActiveScreens()
    {
      
        if (controls != null && GameObject.Find("Menus").transform.Find("Controls Screen").gameObject.activeInHierarchy)
        {
            Debug.Log("controls if");
            GameObject.Find("Menus").transform.Find("Controls Screen").gameObject.SetActive(false);

        }

        if (pause != null && GameObject.Find("Menus").transform.Find("Pause Screen").gameObject.activeInHierarchy)
        {
            Debug.Log("pause if");
            GameObject.Find("Menus").transform.Find("Pause Screen").gameObject.SetActive(false);
        }
    }

    
}
