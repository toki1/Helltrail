using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionManager : MonoBehaviour
{
    public bool isNotGluttony;

    [SerializeField]
    private AudioSource potionUsed;

    public AudioClip healthSound;
    public AudioClip defenseSound;
    public AudioClip damageSound;
    public AudioClip rangeSound;

    // Health potion particle effect
    public GameObject healthUp;

    // Defense potion particle effect
    public GameObject defenseUp;

    // Damage potion particle effect
    public GameObject damageUp;

    // Range potion particle effect
    public GameObject rangeUp;

    // Bomb particle effect
    public GameObject bombExplosion;

    private GameObject playerStatus;
    private GameObject statusText;

    void Start()
    {
        potionUsed = GetComponent<AudioSource>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.name.Equals("Status Text"))
            {
                statusText = go;
                break;
            }
        }
        statusText.GetComponent<TextMeshProUGUI>().text = "";
        Debug.Log("potionman");
        //Debug.Log("changed to " + statusText.GetComponent<Text>().text);
        //Debug.Log(statusText != null);
    }

    void healthPotion()
    {
        print("healed");

        potionUsed.volume = 0.0f;
        potionUsed.clip = healthSound;
        potionUsed.Play();
        StartCoroutine(SoundManager.Fade(potionUsed, 0.75f, 1.0f));

        if(!isNotGluttony)
        {
            GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in temp)
            {
                enemy.GetComponent<EnemyHealthManager>().GainHealth(40);
                enemy.GetComponent<EnemyHealthManager>().PlayParticle(healthUp);
            }
        }

        playerStatus = GameObject.Find("Player Health Bar");
        playerStatus.GetComponent<PlayerHealthController>().GainHealth(20);
        playerStatus.GetComponent<PlayerHealthController>().PlayParticle(healthUp);
        statusText.GetComponent<TextMeshProUGUI>().text = "Health Up";
        //Debug.Log("changed to " + statusText.GetComponent<Text>().text);
        StartCoroutine(PotionExpire(1, 5));

    }
    void defensePotion()
    {
        print("defense");

        potionUsed.volume = 0.0f;
        potionUsed.clip = defenseSound;
        potionUsed.Play();
        StartCoroutine(SoundManager.Fade(potionUsed, 0.75f, 1.0f));

        if (!isNotGluttony)
        {
            GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in temp)
            {
                enemy.GetComponent<EnemyHealthManager>().GainHealth(40);
                enemy.GetComponent<EnemyHealthManager>().PlayParticle(defenseUp);
            }
        }

        playerStatus = GameObject.Find("Player Health Bar");
        playerStatus.GetComponent<PlayerHealthController>().GainHealth(20);
        playerStatus.GetComponent<PlayerHealthController>().PlayParticle(defenseUp);
        statusText.GetComponent<TextMeshProUGUI>().text = "Defense Up";
        //Debug.Log("changed to " + statusText.GetComponent<Text>().text);
        StartCoroutine(PotionExpire(3, 5));
    }

    /*
    void speedPotion()
    {
        print("speed");

        //potionUsed.PlayOneShot(speedSound);
        potionUsed.volume = 0.0f;
        potionUsed.clip = speedSound;
        potionUsed.Play();
        StartCoroutine(SoundManager.Fade(potionUsed, 0.75f, 1.0f));
    }

    */
    void damageUpPotion()
    {
        print("damage up");

        potionUsed.volume = 0.0f;
        potionUsed.clip = damageSound;
        potionUsed.Play();
        StartCoroutine(SoundManager.Fade(potionUsed, 0.75f, 1.0f));

        if (!isNotGluttony)
        {
            GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in temp)
            {
                enemy.GetComponent<EnemyAttackManager>().attackDamage += 20;
                enemy.GetComponent<EnemyHealthManager>().PlayParticle(damageUp);
            }
        }

        playerStatus = GameObject.Find("Player");
        playerStatus.GetComponent<PlayerCombatController>().additionalDamage += 20;
        playerStatus = GameObject.Find("Player Health Bar");
        playerStatus.GetComponent<PlayerHealthController>().PlayParticle(damageUp);
        statusText.GetComponent<TextMeshProUGUI>().text = "Damage Up";
        //Debug.Log("changed to " + statusText.GetComponent<Text>().text);
        StartCoroutine(PotionExpire(4 , 20));
    }

    void rangeUpPotion()
    {
        print("range up");

        potionUsed.volume = 0.0f;
        potionUsed.clip = rangeSound;
        potionUsed.Play();
        StartCoroutine(SoundManager.Fade(potionUsed, 0.75f, 1.0f));

        if (!isNotGluttony)
        {
            GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in temp)
            {
                // Add line that increases enemy range
                //enemy.GetComponent<EnemyAttackManager>().attackDamage += 20;
                enemy.GetComponent<EnemyHealthManager>().PlayParticle(rangeUp);
            }
        }

        playerStatus = GameObject.Find("Player");
        playerStatus.GetComponent<PlayerCombatController>().Arrow.GetComponent<Arrow>().speed += 7.0f;
        playerStatus = GameObject.Find("Player Health Bar");
        playerStatus.GetComponent<PlayerHealthController>().PlayParticle(rangeUp);
        statusText.GetComponent<TextMeshProUGUI>().text = "Range Up";
        //Debug.Log("changed to " + statusText.GetComponent<Text>().text);
        StartCoroutine(PotionExpire(5, 20));
    }
    IEnumerator PotionExpire(int type, int seconds)
    {

        yield return new WaitForSeconds(seconds);
        statusText.GetComponent<TextMeshProUGUI>().text = "";
        //Debug.Log("changed to " + statusText.GetComponent<Text>().text);
        switch (type)
        {
            case 1:
                print("health up done");
                break;
            case 2:
                print("notyet speed");
                break;
            case 3:
                print("defense up expired");
                break;
            case 4:
                print("damage up expired");
                GameObject.Find("Player").GetComponent<PlayerCombatController>().additionalDamage -= 20;
                break;
            case 5:
                print("range up expired");
                GameObject.Find("Player").GetComponent<PlayerCombatController>().Arrow.GetComponent<Arrow>().speed -= 7.0f;
                break;
            default:
                break;
        }


    }
    void allOfTheAbovePotion()
    {

    }

    void bomb()
    {
        /*
        print("bomb");

        potionUsed.volume = 0.0f;
        potionUsed.clip = bombSound;
        potionUsed.Play();
        StartCoroutine(SoundManager.Fade(potionUsed, 0.75f, 1.0f));

        if (!isNotGluttony)
        {
            GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in temp)
            {
                // Add line that makes enemies explode
                //enemy.GetComponent<EnemyAttackManager>().attackDamage += 20;
                enemy.GetComponent<EnemyHealthManager>().PlayParticle(bombExplosion);
            }
        }
        */
    }

    public void useItem(int id)
    {
        print(id);
        switch (id)
        {
            case 1:
                healthPotion();
                break;
            case 2:
                //speedPotion();
                break;
            case 3:
                defensePotion();
                break;
            case 4:
                damageUpPotion();
                break;
            case 5:
                rangeUpPotion();
                break;
            case 6:
                bomb();
                break;
            case 7:
                allOfTheAbovePotion();
                break;
            default:
                break;
        }
    }
}
