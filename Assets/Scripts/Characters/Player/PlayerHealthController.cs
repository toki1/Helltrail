using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public float lerpSpeed;
    public float startingHealth;
    public float maximumHealth;
    public int defense;
    private Image content;
    private float currentFill;
    public float currentValue;

    [SerializeField]
    private AudioClip playerHurt;

    [SerializeField]
    private AudioClip playerHealed;

    private AudioSource playerAudio;
    
    //Animator here
    public Animator anim;

    DamageOverlay overlay;


    // Start is called before the first frame update
    void Start()
    {
        content = GetComponent<Image>();
        currentValue = startingHealth;
        currentFill = currentValue / maximumHealth;
        playerAudio = GameObject.Find("Player").GetComponent<AudioSource>();
        overlay = GameObject.FindObjectOfType(typeof(DamageOverlay)) as DamageOverlay;
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    void GetInput()
	{
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("O");
            LoseHealth(10);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P");
            GainHealth(10);
        }
    }

    public void LoseHealth(float amount)
    {
        // Show damage overlay for 1 second
        StartCoroutine(overlay.ShowOverlay());

        //playerAudio.PlayOneShot(playerHurt);
        playerAudio.volume = 0.0f;
        playerAudio.clip = playerHurt;
        playerAudio.Play();
        StartCoroutine(SoundManager.Fade(playerAudio, 0.75f, 0.8f));

        print(amount);
        print(defense);
        currentValue -= amount - defense;
        print(currentValue);
        if(currentValue < 0)
            currentValue = 0;
        if (currentValue == 0)
        { 
            //This line prevents the Death Screen from loading for some reason
            //anim.Play("Death");
            SceneManager.LoadScene("Death Screen");
        }
        currentFill = currentValue / maximumHealth;
    }

    public void GainHealth(float amount)
	{
        //playerAudio.PlayOneShot(playerHealed);
        playerAudio.volume = 0.0f;
        playerAudio.clip = playerHealed;
        playerAudio.Play();
        StartCoroutine(SoundManager.Fade(playerAudio, 0.75f, 0.7f));

        currentValue += amount;
        if(currentValue > maximumHealth)
            currentValue = maximumHealth;
        currentFill = currentValue / maximumHealth;
    }

    public void PlayParticle(GameObject particle)
    {
        Instantiate(particle, transform.position, Quaternion.identity);
    }
}
