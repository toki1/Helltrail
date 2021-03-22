using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheel : MonoBehaviour
{
    public Image[] selectedWeaponIcons;
    int selected = 0;

    private AudioSource wheelNoises;
    public AudioClip weaponSelected;
    public GameObject bow;
       public GameObject sword;
    // Start is called before the first frame update
    void Start()
    {
        selectedWeaponIcons[0].enabled = true;

        for(int currentImage = 1; currentImage < selectedWeaponIcons.Length; currentImage++)
        {
            selectedWeaponIcons[currentImage].enabled = false;
        }

        wheelNoises = gameObject.GetComponent<AudioSource>();
       
    }
    public void Update()
    {
        if(GameObject.Find("Player").GetComponent<PlayerCombatController>().noBow)
        {
            bow.SetActive(false);

        }
        else
        {
            bow.SetActive(true);
        }
        if (GameObject.Find("Player").GetComponent<PlayerCombatController>().noSword)
        {
            sword.SetActive(false);

        }
        else
        {
            sword.SetActive(true);
        }
    }

    public void selectNext(int currentSelection, int newSelection)
    {
        selectedWeaponIcons[currentSelection].enabled = false;
        selectedWeaponIcons[newSelection].enabled = true;
        selected = newSelection;

        wheelNoises.PlayOneShot(weaponSelected, 1.0f);
    }
}
