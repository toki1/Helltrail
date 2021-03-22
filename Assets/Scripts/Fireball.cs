using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");

        target = player.transform.position;
        print(target);
    }
    Vector3 target;
    bool explode = false;
    // Update is called once per frame
    void Update()
    {
        if (transform.position == target)
        {
            explode = true;
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(20, 20);
            StartCoroutine(ExampleCoroutine());

        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        }


    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        print("triggered");
        if(col.gameObject.tag.Equals("Player"))
        {
            if (explode)
            {
                //GameObject.Find("Health Bar").GetComponent<PlayerHealthController>().LoseHealth(10);

                print("took explosion damage");
            }
            else
            {
                GameObject.Find("Player Health Bar").GetComponent<PlayerHealthController>().LoseHealth(10);
                Destroy(gameObject);
                print("took direct damage");
            }

        }
        else if(!col.gameObject.tag.Equals("Boss") && !col.gameObject.tag.Equals("Wall") && !col.gameObject.tag.Equals("BossAttacks") && !col.gameObject.tag.Equals("Blood"))
		{
            Destroy(gameObject);
		}

    }
    IEnumerator ExampleCoroutine()
    {

        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}