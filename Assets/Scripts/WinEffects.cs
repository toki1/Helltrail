using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinEffects : MonoBehaviour
{
    private Image fadePanel;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().Play();
        fadePanel = GameObject.Find("Fade Panel").GetComponent<Image>();
        StartCoroutine(ExplosionDelay(5.0f));
    }

    private IEnumerator ExplosionDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<ParticleSystem>().Stop();
        StartCoroutine(FadeDelay(3.0f));
    }

    private IEnumerator FadeDelay(float delay)
    {
        fadePanel.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Win Screen");
    }
}
