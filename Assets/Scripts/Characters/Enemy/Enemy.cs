using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int defense;
    Coords currentPosition;
    bool playerDetected = false;
    GameObject player;
    Coords playerPosition;
    GameObject manager;
    Grid grid;
    private AudioSource enemyNoises;
    public AudioClip enemyHurt;
    public AudioClip enemyAttack;
    public AudioClip enemyDeath;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("grid").GetComponent<Grid>();
        manager = GameObject.Find("Manager");
        player = GameObject.Find("Player");
        enemyNoises = gameObject.GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
	{
        //Enemy damage animation here
        enemyNoises.PlayOneShot(enemyHurt, 1.0f);
        currentHealth -= damage-defense;
        if(currentHealth <= 0)
            Death();
	}

    int counter = 0;
    void Update()
    {
        Vector3Int temp = grid.WorldToCell(player.transform.position);
        playerPosition = new Coords(temp.x, temp.y);
        temp = grid.WorldToCell(transform.position);
        currentPosition = new Coords(temp.x, temp.y);
        if (detection() || playerDetected)
        {
            counter++;
            if (getSquareDistance(currentPosition, playerPosition) < 2 && counter > 1000)
            {
                counter = 0;
                attack();
            }
            else if (counter > 1000)
            {
                moveTowardsPlayer();
                counter = 0;
            }
            playerDetected = true;

        }
    }
    public void moveTowardsPlayer()
    {
        //Enemy movement animation here
        getSquareDistance(new Coords(currentPosition.x, currentPosition.y), playerPosition);

        int distance = getSquareDistance(currentPosition, playerPosition);
        anim.SetBool("Imp_Attack", false);
        if (distance >= 2)
        {

            if (getSquareDistance(new Coords(currentPosition.x + 1, currentPosition.y), playerPosition) < distance)
            {
                this.transform.position = grid.GetCellCenterWorld(new Vector3Int(currentPosition.x + 1, currentPosition.y, 0));
                currentPosition = new Coords(currentPosition.x + 1, currentPosition.y);
            }
            else if (getSquareDistance(new Coords(currentPosition.x - 1, currentPosition.y), playerPosition) < distance)
            {
                this.transform.position = grid.GetCellCenterWorld(new Vector3Int(currentPosition.x - 1, currentPosition.y, 0));
                currentPosition = new Coords(currentPosition.x - 1, currentPosition.y);

            }
            else if (getSquareDistance(new Coords(currentPosition.x, currentPosition.y + 1), playerPosition) < distance)
            {
                this.transform.position = grid.GetCellCenterWorld(new Vector3Int(currentPosition.x, currentPosition.y - 1, 0));
                currentPosition = new Coords(currentPosition.x, currentPosition.y + 1);

            }
            else if (getSquareDistance(new Coords(currentPosition.x, currentPosition.y - 1), playerPosition) < distance)
            {
                this.transform.position = grid.GetCellCenterWorld(new Vector3Int(currentPosition.x, currentPosition.y - 1, 0));
                currentPosition = new Coords(currentPosition.x, currentPosition.y - 1);

            }
        }
    }
    public bool detection()
    {
        return getSquareDistance(currentPosition, playerPosition) < 4;
    }
    public void attack()
    {
        anim.SetBool("Imp_Attack", true);
        enemyNoises.PlayOneShot(enemyAttack, 1.0f);
        player.GetComponent<Player>().TakeDamage(10);
        print("attacking");
    }

    void Death()
	{

        anim.Play("Imp_Death");
        enemyNoises.PlayOneShot(enemyDeath, 1.0f);
        UnityEngine.Debug.Log(this.name + " died");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject);
	}
    int getSquareDistance(Coords coord1, Coords coord2)
    {
        return Mathf.Abs(coord1.x - coord2.x) + Mathf.Abs(coord1.y - coord2.y);
    }
}
