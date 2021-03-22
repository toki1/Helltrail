using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public bool[] isFull;
    public int[] slots;
    public int currentSlot;
    public int currentTotal;
    public Image SelectedItemIcon;
    public Sprite slotImage;
    public AudioClip itemAdded;
    private AudioSource noises;


    public void Start()
    {
        for(int i=0; i<slots.Length; ++i)
        {
            slots[i] = -1;
        }

        noises = gameObject.GetComponent<AudioSource>();
    }

    public void addImage(Sprite image)
    {
        slotImage = image;
    }
    public bool addItem(int itemID, Sprite itemImage)
    {
        print(itemID);
        int nextIndex = getNextFreeSlot();
        int temp = getNextFreeSlot();
        if (temp==-1)
        {
            return false;
        }
        else
        {
            noises.volume = 0.0f;
            noises.clip = itemAdded;
            noises.Play();
            StartCoroutine(SoundManager.Fade(noises, 0.75f, 1.0f));

            slots[nextIndex] = itemID;
            GameObject.Find("SlotImage" + (temp + 1)).GetComponent<Image>().sprite = itemImage;
            GameObject.Find("SlotImage" + (temp + 1)).GetComponent<Image>().color = new Color (255, 255, 255,100);
        }
        return true;
    }
    void Update()
    {
        //change to switch statement later
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(useItem(1))
            {
                GameObject.Find("SlotImage1").GetComponent<Image>().sprite = null;
                GameObject.Find("SlotImage1").GetComponent<Image>().color = new Color(255, 255, 255, 0);

                print("used item");
            }
            else
            {
                print("you can't use that item");
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (useItem(2))
            {
                GameObject.Find("SlotImage2").GetComponent<Image>().sprite = null;
                GameObject.Find("SlotImage2").GetComponent<Image>().color = new Color(255, 255, 255, 0);
                print("used item");
            }
            else
            {
                print("you can't use that item");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (useItem(3))
            {
                GameObject.Find("SlotImage3").GetComponent<Image>().sprite = null;
                GameObject.Find("SlotImage3").GetComponent<Image>().color = new Color(255, 255, 255, 0);

                print("used item");
            }
            else
            {
                print("you can't use that item");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (useItem(4))
            {
                GameObject.Find("SlotImage4").GetComponent<Image>().sprite = null;
                GameObject.Find("SlotImage4").GetComponent<Image>().color = new Color(255, 255, 255, 0);

                print("used item");
            }
            else
            {
                print("you can't use that item");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (useItem(5))
            {
                GameObject.Find("SlotImage5").GetComponent<Image>().sprite = null;
                GameObject.Find("SlotImage5").GetComponent<Image>().color = new Color(255, 255, 255, 0);

                print("used item");
            }
            else
            {
                print("you can't use that item");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (useItem(6))
            {
                GameObject.Find("SlotImage6").GetComponent<Image>().sprite = null;
                GameObject.Find("SlotImage6").GetComponent<Image>().color = new Color(255, 255, 255, 0);

                print("used item");
            }
            else
            {
                print("you can't use that item");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (useItem(7))
            {
                GameObject.Find("SlotImage7").GetComponent<Image>().sprite = null;
                GameObject.Find("SlotImage7").GetComponent<Image>().color = new Color(255, 255, 255, 0);

                print("used item");
            }
            else
            {
                print("you can't use that item");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (useItem(8))
            {
                GameObject.Find("SlotImage8").GetComponent<Image>().sprite = null;
                GameObject.Find("SlotImage8").GetComponent<Image>().color = new Color(255, 255, 255, 0);

                print("used item");
            }
            else
            {
                print("you can't use that item");
            }
        }
    }
    public bool useItem(int spot)
    {
        if (slots[spot-1] == -1)
        {
            return false;
        }
        else
        {
            GameObject.Find("Manager").GetComponent<PotionManager>().useItem(slots[spot-1]);
            slots[spot - 1] = -1;

            return true;
        }
    }
    int getNextFreeSlot()
    {
        for(int i=0; i<slots.Length; ++i)
        {
            if(slots[i] == -1)
            {
                return i;
            }
        }
        return -1;
    }
    public void selectNextItem()
    {
        if (currentTotal > 1)
        {
            currentSlot += 1;

            // Set new index for current slot
            if (currentSlot == currentTotal)
            {
                currentSlot = 0;
            }

        }
    }
}
