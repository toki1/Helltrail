using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;

public class EnemyHealthManager : MonoBehaviour
{
    /* Health Boolean Array size should be 2
     * Only set 1 as true!
     * 0 = Normal health
     * 1 = Infinite health
     */
    public bool[] healthType;

    /* Health Boolean Array size should be 2
     * Only set 1 as true!
     * 0 = No weakness
     * 1 = Weak to dagger
     * 2 = Weak to sword
     * 3 = Weak to arrow
     */
    public bool[] weaknessType;

    public float startingHealth;
    private float currentHealth;

    public Image content;
    private float currentFill;
    public float lerpSpeed;
    public GameObject drop;
    // Damage particle effect
    public GameObject blood;
    public bool dropSoul;
    // Blood puddle left after death
    public GameObject puddle;

    public GameObject soul;

    //aniamtion
    public Animator anim;

    bool alreadyDead = false;
    public float damageModifier;

    [SerializeField]
    private GameObject bossExplosion;

    [SerializeField]
    private AudioClip bossDeathSound;

    // Start is called before the first frame update
    void Start()
    {
        int numTrue = 0;
        foreach (bool i in healthType)
        {
            if (i)
            {
                numTrue++;
            }
        }
        Assert.IsTrue(numTrue == 1);

        currentHealth = startingHealth;
        currentFill = currentHealth / startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void LoseHealth(float amount, int damageType)
    {
        Instantiate(blood, transform.position, Quaternion.identity);

        if (healthType[0])
        {

            if ((weaknessType[1] && damageType == 0) || (weaknessType[2] && damageType == 1) || (weaknessType[3] && damageType == 2))
            {
                currentHealth -= (amount * damageModifier);
            }
            else
            {
                currentHealth -= amount;
            }
        }

        if (!alreadyDead && currentHealth <= 0)
        {
            alreadyDead = true;
            Death();
        }
        Debug.Log(currentHealth);
        currentFill = currentHealth / startingHealth;
    }

    public void GainHealth(float amount)
    {
        currentHealth += amount;
        if (currentHealth <= 0)
            Death();
    }


    public void PlayParticle(GameObject particle)
    {
        Instantiate(particle, transform.position, Quaternion.identity);
    }


    void Death()
    {
        // Win Condition for Satan
        if (gameObject.tag.Equals("Boss"))
        {
            GameObject winEffect = Instantiate(bossExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (gameObject.tag == "Enemy")
            {
                anim.SetBool("Dead", true);
            }

            UnityEngine.Debug.Log(this.name + " died");
            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;

            if(!gameObject.tag.Equals("Boss"))
            {

                GameObject puddleObj = Instantiate(puddle, transform.position, Quaternion.identity);
                puddleObj.transform.localScale = new Vector3(Random.Range(0.2f, 0.25f), Random.Range(0.2f, 0.25f), 1.0f);

            }

            if (dropSoul)
            {
                Instantiate(soul, transform.position, transform.rotation);

            }
            if (drop != null)
            {
                Instantiate(drop, transform.position, transform.rotation);
                print("dropped");
            }
            Destroy(gameObject);
        
    }
}
