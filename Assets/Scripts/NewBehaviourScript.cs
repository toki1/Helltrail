using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask m_LayerMask;
    void Start()
    {
        StartCoroutine(ExampleCoroutine());

    }

    // Update is called once per frame
    void Update()
    {
        /*
        Collider2D hitColliders = Physics2D.OverlapBox(gameObject.transform.position, transform.localScale/2, 0, m_LayerMask);
        if(hitColliders)
        {
            print(hitColliders.name);
        }*/

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        //print("triggered");

        if (col.gameObject.tag.Equals("Player"))
        {
            print(col.gameObject.name);
            GameObject.Find("Player Health Bar").GetComponent<PlayerHealthController>().LoseHealth(5);
            //col.gameObject.GetComponent<Player>().TakeDamage(10);
            print("took damage");
        }
    }
    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
