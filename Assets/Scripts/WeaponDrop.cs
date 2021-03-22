using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    public GameObject weapon;
    Transform temp;
    public int weaponID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag =="Player")
        {
            // Weapon UI animation starts when player acquires weapon
            Virgil currentVirgil = GameObject.FindObjectOfType<Virgil>();
            Animator swordAnim = currentVirgil.swordIcon.GetComponent<Animator>();
            Animator bowAnim = currentVirgil.bowIcon.GetComponent<Animator>();

            if (weaponID == 1)
            {
                swordAnim.enabled = true;
                collision.gameObject.GetComponent<PlayerCombatController>().noSword = false;
            }
            else
            {
                bowAnim.enabled = true;
                collision.gameObject.GetComponent<PlayerCombatController>().noBow = false;
            }
            Destroy(gameObject);
        }

    }
}