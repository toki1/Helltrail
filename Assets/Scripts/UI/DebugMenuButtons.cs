using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMenuButtons : MonoBehaviour
{
    // Make sure scene names and scenes are added to build settings
    public void GoToScene()
    {
        SceneManager.LoadScene(gameObject.name);
    }
}
