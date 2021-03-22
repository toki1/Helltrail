using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
struct CharacterDialogue
{
    public string text;
    public int speaker;
    public CharacterDialogue(string t, int s)
    {
        text = t;
        speaker = s;
    }
}
public class Virgil : MonoBehaviour
{
    public int stage =  -2;
    public Vector3 target;
    bool move = false;
    bool teleport = false;
    bool dialogue = false;
    bool arrivedAtTarget = false;
    bool changeRoom = false;
    bool inDialogue = false;
    bool triggerDialogue = false;
    public GameObject dialogueUI;
    public GameObject VirildialogueUI;
    public GameObject AidendialogueUI;
    public GameObject textUI;
    bool doorTrigger = false;
    public int dialogueStage = 0;
    public GameObject[] movePoints;
    CharacterDialogue[] dialogues;
    int dialogueEndPoint;
    bool disabledialogue = false;
    bool pause = false;
    public int enemycount;
    bool killingPart = false;
    public GameObject[] enemies;
    private GameObject weaponWheel;

    // Icons of weapons that Virgil will display during tutorial
    public GameObject daggerIcon;
    public GameObject swordIcon;
    public GameObject bowIcon;
    private IEnumerator coroutine;
    int a = 0;
    int b = 1;
    int c = 2;
    // Start is called before the first frame update
    void Start()
    {
        weaponWheel = GameObject.Find("Weapon Wheel");
        daggerIcon = weaponWheel.transform.GetChild(0).GetChild(0).gameObject;
        swordIcon = weaponWheel.transform.GetChild(1).GetChild(0).gameObject;
        bowIcon = weaponWheel.transform.GetChild(2).GetChild(0).gameObject;

        // Dagger icon initial position and scale
        daggerIcon.transform.position = new Vector3(-250.0f, 400.0f, 1);
        daggerIcon.transform.localScale = new Vector3(14.0f, 14.0f, 0);
        daggerIcon.GetComponent<Animator>().enabled = false;

        // Sword icon initial position and scale
        swordIcon.transform.position = new Vector3(-357.0f, 470.0f, 1);
        swordIcon.transform.localScale = new Vector3(8.0f, 8.0f, 0);
        swordIcon.GetComponent<Animator>().enabled = false;

        // Bow icon initial position and scale
        bowIcon.transform.position = new Vector3(-460.0f, 450.0f, 1);
        bowIcon.transform.localScale = new Vector3(8.0f, 8.0f, 0);
        bowIcon.GetComponent<Animator>().enabled = false;


        transform.position = new Vector3(17.5f, 11.5f, -2);
        target = transform.position;
        dialogueUI.SetActive(false);
        dialogues = new CharacterDialogue[17];
        dialogues[0] = new CharacterDialogue("Huh? Where am I?", 0);
        dialogues[1] = new CharacterDialogue("You’re in Limbo. You’ve been sent to help us.", 1);
        dialogues[2] = new CharacterDialogue("Help you? How?", 0);
        dialogues[3] = new CharacterDialogue("Satan has illegally taken pure souls from the mortal realm. Recently the border between the afterlife and the mortal realm has begun to weaken. So, we must take this opportunity to rescue them. ", 1);
        dialogues[4] = new CharacterDialogue("But why me?", 0);
        dialogues[5] = new CharacterDialogue("Satan has captured and corrupted many of the world’s most powerful warriors and saints. However, only a mortal can traverse both Heaven and Hell. That’s why you were chosen, a pure soul with a strong resistance to the corruption and temptation of Hell, to reclaim the lost souls.", 1);
        dialogues[6] = new CharacterDialogue("If we hurry, we'll be able to get you to Gluttony, the third circle of Hell.", 1);
        dialogues[7] = new CharacterDialogue("First, you’ll need some weapons. Here’s a small dagger. Use it by pressing the arrows keys in the direction of your attack. This will deal a small amount of damage, but you can use it quickly. When you’re ready, let’s head to the next room, I heard something coming from there. Use the WASD keys to move around.", 1);
        dialogues[8] = new CharacterDialogue("Press E to prepare your special attack for your next strike", 1);
        dialogues[9] = new CharacterDialogue("A wretch! These enemies are common in all circles of Hell. They are quick but pretty easy to kill! Use your dagger to attack.", 1);
        dialogues[10] = new CharacterDialogue("It looks like that enemy had a sword from a fallen soldier. You can use that on your journey, it will be stronger but slower than your dagger.", 1);
        dialogues[11] = new CharacterDialogue("To rotate between your weapons use the key ‘Q’. There are a lot of enemies in here, if you see your health bar dropping, look for a health potion for strength. ", 1);
        dialogues[12] = new CharacterDialogue("That was close. If you find yourself needing help, always keep your eyes open for potions, they can help you in lots of ways. Also, it looks like you now have a bow and arrow! That will be useful for ranged attacks. ", 1);
        dialogues[13] = new CharacterDialogue("It looks like one of the Gluttony monsters has wandered into Limbo!", 1);
        dialogues[14] = new CharacterDialogue("A soul! One of the monsters must’ve trapped it inside them! You can use these freed souls to help fuel your journey.", 1);
        dialogues[15] = new CharacterDialogue("It looks like we’ve made it to the entry of the Gluttony circle of Hell. I think you’re ready to head on your journey. Remember, every circle of Hell will contain the sin it embodies. Giving in to these sins, will only make these enemies stronger.", 1);
        dialogues[16] = new CharacterDialogue("We wish you luck on your journey. You are our only hope in returning these pure souls to the world.", 1);
        coroutine = TextAnimation("");

        next();
    }
    void Update()
    {
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 3 * Time.deltaTime);
            if (Vector3.Distance(transform.position, target) < 0.1||true)
            {
                move = false;
                arrivedAtTarget = true;
                if (changeRoom)
                {
                    changeRoom = false;
                    print("asdf2");
                    next();

                }

            }
        }
        else if (teleport)
        {
            transform.position = target;
            arrivedAtTarget = true;
            teleport = false;
            print("asdf3");

            next();
        }
        if (triggerDialogue && arrivedAtTarget && doorTrigger)
        {
            //next();
            //triggerDialogue = false;
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            print(stage);
            if (inDialogue)
            {
                nextDialogue();



            }
            else
            {
                dialogueUI.SetActive(false);
                GameObject.Find("Player").GetComponent<PlayerMovementController>().enabled = true;

                // Time.timeScale = 1;
                if (!killingPart)
                {
                    Door.doorLock = false;

                }
                //
                //next();
            }
            if (pause && !killingPart)
            {
                pause = false;
                next();
            }
            if (killingPart)
            {
                enemies[a].GetComponent<EnemyMovementManager>().enabled = true;
                enemies[a+1].GetComponent<EnemyMovementManager>().enabled = true;
                enemies[a+2].GetComponent<EnemyMovementManager>().enabled = true;
            }
            print("enter pressed");
        }
        int temp = GameObject.FindGameObjectsWithTag("Enemy").Length;
        //print(temp);
        if (temp == enemycount - 3 && killingPart)
        {
            Door.doorLock = false;

            enemycount -= 3;
            pause = false;
            killingPart = false;
            next();
            print("this ran");
        }
    }
    public void fix()
    {
        //dialogueEndPoint++;
    }

    void nextDialogue()
    {
        if (dialogueStage >= dialogues.Length)
        {
            inDialogue = false;

            return;
        }
        CharacterDialogue d = dialogues[dialogueStage];
        if (d.speaker == 0)
        {
            VirildialogueUI.SetActive(false);
            AidendialogueUI.SetActive(true);
            textUI.GetComponent<Text>().text = "";
            StopCoroutine(coroutine);
            coroutine = TextAnimation(d.text);
            StartCoroutine(coroutine);

            //textUI.GetComponent<Text>().text = d.text;
        }
        else
        {
            VirildialogueUI.SetActive(true);
            AidendialogueUI.SetActive(false);
            textUI.GetComponent<Text>().text = "";
            StopCoroutine(coroutine);
            coroutine = TextAnimation(d.text);

            StartCoroutine(coroutine);

            //textUI.GetComponent<Text>().text = d.text;
        }

        // Play weapon animations when Virgil introduces a weapon
        if (dialogueStage == 7)
        {
            daggerIcon.GetComponent<Animator>().enabled = true;

        }
        /*
        else if (dialogueStage == 9)
        {
            swordIcon.GetComponent<Animator>().enabled = true;

        }
        else if (dialogueStage == 11)
        {
            bowIcon.GetComponent<Animator>().enabled = true;
        }*/
        else if (dialogueStage == 15)
        {
            daggerIcon.GetComponent<Animator>().enabled = false;
            swordIcon.GetComponent<Animator>().enabled = false;
            bowIcon.GetComponent<Animator>().enabled = false;
        }

        print(dialogueStage);
        print(dialogueEndPoint);

        dialogueStage++;


        if (dialogueStage == dialogueEndPoint)
        {
            inDialogue = false;
            if (pause)
            {
                print("a");
            }
            else
            {
                print("b");

                next();

            }
            print("c");


        }

    }
    private IEnumerator TextAnimation(string text)
    {
        string[] t = text.Split(null);
        for(int i=0; i<t.Length; ++i)
        {
            yield return new WaitForSeconds(0.05f);

            textUI.GetComponent<Text>().text = textUI.GetComponent<Text>().text +" "+ t[i];
        }

        

    }
    void doDialogue()
    {
        dialogueUI.SetActive(true);
        inDialogue = true;
        print("doing dialogue");
        GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameObject.Find("Player").GetComponent<PlayerMovementController>().animUp.SetBool("Walking", false);
        GameObject.Find("Player").GetComponent<PlayerMovementController>().animDown.SetBool("Walking", false);
        GameObject.Find("Player").GetComponent<PlayerMovementController>().animRight.SetBool("Walking", false);
        GameObject.Find("Player").GetComponent<PlayerMovementController>().animLeft.SetBool("Walking", false);
        GameObject.Find("Player").GetComponent<PlayerMovementController>().enabled = false;

        //Time.timeScale = 0;
        nextDialogue();
    }
    public void next()
    {

        switch (stage)
        {
            case -1:
                {
                    doDialogue();
                    dialogueEndPoint = 9;
                    break;
                }
            case 0:

                {
                    move = true;
                    target = new Vector3(22.5f, 14.5f, -2);
                    arrivedAtTarget = false;
                    changeRoom = true;
                    //do Dialogue
                    break;
                }
            case 1:
                {

                    teleport = true;
                    target = new Vector3(37.5f, 14.5f, -2);
                    break;
                }

            case 2:
                {
                    move = true;
                    target = new Vector3(44.5f, 14.5f, -2);
                    arrivedAtTarget = false;
                    triggerDialogue = true;


                    break;
                }
            case 3:
                {

                    dialogueEndPoint = 10;
                    pause = true;
                    killingPart = true;

                    doDialogue();

                    break;
                }
            case 222:
                {

                    break;
                }
            case 4:
                {
                    GameObject.Find("Player").GetComponent<PlayerCombatController>().noSword = false;

                    dialogueEndPoint = 11;
                    pause = true;
                    doDialogue();
                    Door.doorLock = true;

                    break;
                }

            case 5:
                {
                    changeRoom = true;
                    move = true;
                    target = new Vector3(50.5f, 14.5f, -2);
                    arrivedAtTarget = false;
                    break;
                }
            case 6:
                {
                    teleport = true;
                    target = new Vector3(61.5f, 14.5f, -2);
                    break;
                }
            case 7:
                {
                    move = true;
                    target = new Vector3(68.5f, 14.5f, -2);
                    arrivedAtTarget = false;

                    break;
                }

            case 8:
                {
                    a += 3;
                    enemies[3].GetComponent<EnemyMovementManager>().enabled = true;
                    enemies[4].GetComponent<EnemyMovementManager>().enabled = true;
                    enemies[5].GetComponent<EnemyMovementManager>().enabled = true;
                    dialogueEndPoint = 12;
                    pause = true;
                    killingPart = true;
                    //stage += 1;

                    doDialogue();
                    break;
                }

            case 9:
                {
                    GameObject.Find("Player").GetComponent<PlayerCombatController>().noBow = false;

                    dialogueEndPoint = 13;
                    pause = true;
                    doDialogue();
                    break;
                }

            case 10:
                {
                    changeRoom = true;
                    move = true;
                    target = new Vector3(74.5f, 14.5f, -2);
                    arrivedAtTarget = false;
                    break;
                }
            case 11:
                {
                    teleport = true;
                    target = new Vector3(85.5f, 14.5f, -2);
                    break;
                }
            case 12:
                {
                    move = true;
                    target = new Vector3(92.5f, 14.5f, -2);
                    arrivedAtTarget = false;

                    break;
                }


            case 13:
                {
                    dialogueEndPoint = 14;
                    pause = true;
                    killingPart = true;
                    a += 3;

                    doDialogue();
                    break;
                }


            case 14:
                {

                    dialogueEndPoint = 15;
                    pause = true;

                    doDialogue();
                    break;
                }




            case 15:
                {
                    changeRoom = true;
                    move = true;
                    target = new Vector3(98.5f, 14.5f, -2);
                    arrivedAtTarget = false;
                    break;
                }
            case 16:
                {
                    teleport = true;
                    target = new Vector3(109.5f, 14.5f, -2);
                    break;
                }
            case 17:
                {
                    changeRoom = true;

                    move = true;
                    target = new Vector3(112.5f, 14.5f, -2);
                    arrivedAtTarget = false;
                    break;
                }
            case 18:
                {

                    break;
                }

            /*

                        case 22:
                            {
                                changeRoom = true;
                                move = true;
                                target = new Vector3(122.5f, 14.5f, -2);
                                arrivedAtTarget = false;
                                break;
                            }
                        case 23:
                            {
                                teleport = true;
                                target = new Vector3(133.5f, 14.5f, -2);
                                break;
                            }
                        case 24:
                            {
                                changeRoom = true;

                                move = true;
                                target = new Vector3(136.5f, 14.5f, -2);
                                arrivedAtTarget = false;
                                break;
                            }
                            */



            case 19:
                {
                    dialogueEndPoint = 18;

                    doDialogue();
                    break;
                }







        }
        ++stage;

    }
}
