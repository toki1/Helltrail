using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 3.0f;
    float arrowRotation;
    Vector3 movementDirection;
    public GameObject weapon;
    public float additionalDamage;
    public GameObject upgradedStats;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] results = GameObject.FindGameObjectsWithTag("Stats");
        for(int i = 0; i < results.Length; ++i)
		{
            if(results[i].GetComponent<Stats>() != null)
			{
                upgradedStats = results[i];
                break;
			}
		}
        //upgradedStats = GameObject.FindGameObjectsWithTag("Stats")[0];
        //Debug.Log(arrowRotation);
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(arrowRotation);
        //Debug.Log(transform.position);
        transform.position += movementDirection * Time.deltaTime * speed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag.Equals("Enemy") || col.gameObject.tag == "Boss")
        {
            Debug.Log(weapon.GetComponent<BaseWeapon>().attackDamage);
            Debug.Log(additionalDamage);
            Debug.Log(upgradedStats.GetComponent<Stats>().upgradedDamage);
            //Debug.Log("Enemy hit" + (weapon.GetComponent<BaseWeapon>().attackDamage + additionalDamage + upgradedStats.GetComponent<Stats>().upgradedDamage));
            col.GetComponent<EnemyHealthManager>().LoseHealth(weapon.GetComponent<BaseWeapon>().attackDamage + additionalDamage + upgradedStats.GetComponent<Stats>().upgradedDamage, weapon.GetComponent<BaseWeapon>().attackID);
        }

        if(!col.gameObject.tag.Equals("Player") && !col.gameObject.tag.Equals("Arrow") && !col.gameObject.tag.Equals("Soul") && !col.gameObject.tag.Equals("BossAttacks") && !col.gameObject.tag.Equals("Blood") && !col.gameObject.tag.Equals("Potion"))
		{
            Destroy(gameObject);
        }
    }

    public void SetRotation(float rot)
	{
        arrowRotation = rot;
        movementDirection = new Vector3(Mathf.Sin(rot * Mathf.Deg2Rad), Mathf.Cos(rot * Mathf.Deg2Rad), 0);
    }
}
