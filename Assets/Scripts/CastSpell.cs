using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CastSpell : MonoBehaviour
{
    public GameObject iceSpikes;
    public GameObject oozeIndicator;
    public GameObject fireBall;
    public GameObject enemySummon;
    public GameObject enemyIndicator;
    public float enemyChance;

    public GameObject gridObject;
    public GameObject tileMapObject;

    public AudioClip iceSound;
    public AudioClip fireSound;

    private AudioSource satanSpellAudio;
    public void iceSpell()
    {
        satanSpellAudio.PlayOneShot(iceSound);
        Grid grid = gridObject.GetComponent<Grid>();
        Tilemap tilemap = tileMapObject.GetComponent<Tilemap>();
        //Instantiate(iceSpikes, grid.GetCellCenterWorld(new Vector3Int(0, 0, 0)), Quaternion.identity);

        for (int i=0; i<2; ++i)
        {
            Vector3 temp = grid.GetCellCenterWorld(new Vector3Int(Random.Range(0, 8) - 5, Random.Range(0, 6) - 5, 0));
            Instantiate(oozeIndicator, new Vector3(temp.x, temp.y, 0f), Quaternion.identity);
            StartCoroutine(WaitToSummonSlime(temp));
        }

    }

    public void fireBallSpell()
    {
        satanSpellAudio.PlayOneShot(fireSound);
        Grid grid = gridObject.GetComponent<Grid>();
        Tilemap tilemap = tileMapObject.GetComponent<Tilemap>();
        //Instantiate(fireBall, grid.GetCellCenterWorld(new Vector3Int(0, 0, 0)), Quaternion.Euler(0, 0, 90));
        Instantiate(fireBall, GameObject.Find("Hoggish").transform.position + new Vector3(0f, 1f, 0f), Quaternion.Euler(0, 0, 90));
    }

    public void summonSpell()
    {
        float randomChance = Random.Range(0f, enemyChance);
        if(randomChance <= 1f)
        {
            //Summon sound here
            //satanSpellAudio.PlayOneShot(iceSound);
            Grid grid = gridObject.GetComponent<Grid>();
            Tilemap tilemap = tileMapObject.GetComponent<Tilemap>();
            //Instantiate(enemySummon, grid.GetCellCenterWorld(new Vector3Int(0, 0, 0)), Quaternion.identity);
            Vector3 temp = grid.GetCellCenterWorld(new Vector3Int(Random.Range(0, 8) - 5, Random.Range(0, 6) - 5, 0));
            Instantiate(enemyIndicator, new Vector3(temp.x, temp.y, 0f), Quaternion.identity);
            StartCoroutine(WaitToSummonEnemy(temp));
        }
    }

    void Start()
    {
        satanSpellAudio = gameObject.GetComponent<AudioSource>();
        //iceSpell();
    }

    IEnumerator WaitToSummonSlime(Vector3 temp)
    {
        yield return new WaitForSeconds(1f);
        Instantiate(iceSpikes, new Vector3(temp.x, temp.y, 0f), Quaternion.identity);
    }

    IEnumerator WaitToSummonEnemy(Vector3 temp)
    {
        yield return new WaitForSeconds(1f);
        Instantiate(enemySummon, new Vector3(temp.x, temp.y, 0f), Quaternion.identity);
    }
}
