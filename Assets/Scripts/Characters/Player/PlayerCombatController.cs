using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatController : MonoBehaviour
{
	public LayerMask enemyLayer;
	public Transform attackPoint;
    public float additionalDamage;
	//Access variables like this: weapon.GetComponent<BaseWeapon>().attackDamage
	private GameObject weapon;
	int weaponIndex = 0;
	public GameObject[] allWeapons;
	public GameObject Arrow;

	private float timeSinceLastAttack;
	private AudioSource playerAudio;
	private WeaponWheel weaponSlots;

	[SerializeField]
	private AudioClip weaponSwap;

	[SerializeField]
	private AudioClip specialRangedSound;

	//booleans here to know what weapon is currently equipped for animations along with animator
	public bool lightAttack = true;
	public bool heavyAttack = false;
	public Animator animUp;
	public Animator animDown;
	public Animator animRight;
	public Animator animLeft;
	public GameObject spriteUp;
	public GameObject spriteDown;
	public GameObject spriteRight;
	public GameObject spriteLeft;
	public bool upAttack;
	public bool downAttack;
	public bool rightAttack;
	public bool leftAttack;
    public bool noBow;
    public bool noSword;
    public PlayerMovementController movementController;

	public GameObject upgradedStats;

	//Variables for special attack
	bool specialAttackIsQueued;
	float specialAttackTimer;
	public float specialAttackCooldown = 6.0f;
	public float lightAttackSpecialBonus = 5f;

	// Indicators for status of special attacks
	private Image lightSpecialCircle;
	private Image heavySpecialCircle;
	private Image rangeSpecialCircle;

	private Image CircleSlash;
	float circleSlashFillAmount = 0f;

	public GameObject lightEffect;

	// Start is called before the first frame update
	void Start()
	{
		GameObject[] results = GameObject.FindGameObjectsWithTag("Stats");
		for (int i = 0; i < results.Length; ++i)
		{
			if (results[i].GetComponent<Stats>() != null)
			{
				upgradedStats = results[i];
				break;
			}
		}

		weapon = allWeapons[weaponIndex];
		playerAudio = GameObject.Find("Player").GetComponent<AudioSource>();
		weaponSlots = GameObject.Find("Weapon Wheel").GetComponent<WeaponWheel>();

		lightAttack = true;
		heavyAttack = false;

		lightSpecialCircle = GameObject.Find("Light Special Circle").GetComponent<Image>();
		heavySpecialCircle = GameObject.Find("Heavy Special Circle").GetComponent<Image>();
		rangeSpecialCircle = GameObject.Find("Range Special Circle").GetComponent<Image>();

		CircleSlash = GameObject.Find("CircleSlash").GetComponent<Image>();
		CircleSlash.fillAmount = 0;

		lightSpecialCircle.fillAmount = 0;
		heavySpecialCircle.fillAmount = 0;
		rangeSpecialCircle.fillAmount = 0;

		specialAttackIsQueued = false;
		specialAttackTimer = Time.time;
		

	}

	// Update is called once per frame
	void Update()
	{
		GetInput();

		float timeLeftOnCooldown = specialAttackTimer - Time.time;
		if (timeLeftOnCooldown < 0)
		{
			timeLeftOnCooldown = 0;
		}
		timeLeftOnCooldown = 1 - (timeLeftOnCooldown / specialAttackCooldown);

		if(timeLeftOnCooldown != lightSpecialCircle.fillAmount)
		{
			lightSpecialCircle.fillAmount = Mathf.Lerp(lightSpecialCircle.fillAmount, timeLeftOnCooldown, Time.deltaTime * 5f);
			heavySpecialCircle.fillAmount = Mathf.Lerp(heavySpecialCircle.fillAmount, timeLeftOnCooldown, Time.deltaTime * 5f);
			rangeSpecialCircle.fillAmount = Mathf.Lerp(rangeSpecialCircle.fillAmount, timeLeftOnCooldown, Time.deltaTime * 5f);
		}

		if(circleSlashFillAmount != CircleSlash.fillAmount)
		{
			CircleSlash.fillAmount = Mathf.Lerp(CircleSlash.fillAmount, circleSlashFillAmount, Time.deltaTime * 5f);
		}

		if(CircleSlash.fillAmount >= 0.97)
		{
			circleSlashFillAmount = 0f;
			CircleSlash.fillAmount = 0f;
		}

	}

	private void GetInput()
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
				specialAttackIsQueued = true;
		}

        //Swap Weapon Input
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            weaponIndex += 1;

            if (weaponIndex == 1 && noSword)
            {
                --weaponIndex;
            }
            else if (weaponIndex == 2 && noBow)
            {
                --weaponIndex;
            }
            else
            {

                playerAudio.PlayOneShot(weaponSwap);

                if (weaponIndex >= allWeapons.Length)
                {
                    weaponIndex = 0;
                    weaponSlots.selectNext(allWeapons.Length - 1, 0);// light
                                                                     //for light attack animation
                    lightAttack = true;
                    heavyAttack = false;
                    weapon = allWeapons[weaponIndex];
                }
                else
                {
                    weapon = allWeapons[weaponIndex];
                    // This is hardcoded so I will fix this: swaps weapon icon on HUD
                    if (weaponIndex == 1)
                    {
                        weaponSlots.selectNext(0, 1);// heavy
                                                     //for heavy attack animation
                        lightAttack = false;
                        heavyAttack = true;
                    }
                    else if (weaponIndex == 0)
                    {
                        weaponSlots.selectNext(1, 0);// light
                                                     //for light attack animation
                        lightAttack = true;
                        heavyAttack = false;
                    }
                    else if (weaponIndex == 2)
                    {
                        weaponSlots.selectNext(1, 2);// range
                                                     //for range attack animation
                        lightAttack = false;
                        heavyAttack = false;
                    }
                }
                //Debug.Log("Swap weapon");
            }
        }
        if (weapon.GetComponent<BaseWeapon>().rangedAttack)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Vector3 arrowPosition = new Vector3(0.0f, 1.0f, 0.0f);
                arrowPosition += transform.position;
                upAttack = true;
                downAttack = false;
                rightAttack = false;
                leftAttack = false;
                RangedAttack(90f, arrowPosition);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Vector3 arrowPosition = new Vector3(0.0f, -1.0f, 0.0f);
                arrowPosition += transform.position;
                upAttack = false;
                downAttack = true;
                rightAttack = false;
                leftAttack = false;
                RangedAttack(270f, arrowPosition);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Vector3 arrowPosition = new Vector3(-1.0f, 0.0f, 0.0f);
                arrowPosition += transform.position;
                upAttack = false;
                downAttack = false;
                rightAttack = false;
                leftAttack = true;
                RangedAttack(0f, arrowPosition);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Vector3 arrowPosition = new Vector3(1.0f, 0.0f, 0.0f);
                arrowPosition += transform.position;
                upAttack = false;
                downAttack = false;
                rightAttack = true;
                leftAttack = false;
                RangedAttack(180f, arrowPosition);
            }
        }
        else
        {
            //Attack Input
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                attackPoint.localPosition = new Vector3(0.0f, 1.5f, 0f);
                Debug.Log(weapon.GetComponent<BaseWeapon>().attackDamage);
                upAttack = true;
                downAttack = false;
                rightAttack = false;
                leftAttack = false;
                Attack();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                attackPoint.localPosition = new Vector3(0.0f, -1.5f, 0f);
                upAttack = false;
                downAttack = true;
                rightAttack = false;
                leftAttack = false;
                Attack();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                attackPoint.localPosition = new Vector3(-1.0f, 0.5f, 0f);
                upAttack = false;
                downAttack = false;
                rightAttack = false;
                leftAttack = true;
                Attack();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                attackPoint.localPosition = new Vector3(1.0f, 0.5f, 0f);
                upAttack = false;
                downAttack = false;
                rightAttack = true;
                leftAttack = false;
                Attack();
            }
        }
		

	}

	void RangedAttack(float arrowRotation, Vector3 arrowPosition)
	{
		if (Time.time > timeSinceLastAttack)
		{
			timeSinceLastAttack = Time.time + weapon.GetComponent<BaseWeapon>().attackRate - upgradedStats.GetComponent<Stats>().upgradedAttackRate;

			//animation
			if(upAttack)
			{
				movementController.attacking = true;
				spriteUp.SetActive(true);
				spriteDown.SetActive(false);
				spriteRight.SetActive(false);
				spriteLeft.SetActive(false);
				animUp.Play("Ranged Attack");
				
				//Delay
				StartCoroutine(waitForRanged( 0.35f, arrowRotation, arrowPosition, false));
			}
			else if(downAttack)
			{
				movementController.attacking = true;
				spriteUp.SetActive(false);
				spriteDown.SetActive(true);
				spriteRight.SetActive(false);
				spriteLeft.SetActive(false);
				animDown.Play("Ranged Attack");
				
				//Delay
				StartCoroutine(waitForRanged( 0.35f, arrowRotation, arrowPosition, false));
			}
			else if(rightAttack)
			{
				movementController.attacking = true;
				spriteUp.SetActive(false);
				spriteDown.SetActive(false);
				spriteRight.SetActive(true);
				spriteLeft.SetActive(false);
				animRight.Play("Ranged Attack");
				
				//Delay
				StartCoroutine(waitForRanged( 0.35f, arrowRotation, arrowPosition, true));
			}
			else if(leftAttack)
			{
				movementController.attacking = true;
				spriteUp.SetActive(false);
				spriteDown.SetActive(false);
				spriteRight.SetActive(false);
				spriteLeft.SetActive(true);
				animLeft.Play("Ranged Attack");
				
				//Delay
				StartCoroutine(waitForRanged( 0.35f, arrowRotation, arrowPosition, true));
			}
		}
	}
	void Attack()
	{
		if (Time.time > timeSinceLastAttack)
		{
			timeSinceLastAttack = Time.time + weapon.GetComponent<BaseWeapon>().attackRate - upgradedStats.GetComponent<Stats>().upgradedAttackRate;
			//Debug.Log("PlayerCombatController Attack");
			//Animation for light attack here
			if(lightAttack && upAttack)
			{
				movementController.attacking = true;
				spriteUp.SetActive(true);
				spriteDown.SetActive(false);
				spriteRight.SetActive(false);
				spriteLeft.SetActive(false);
				animUp.Play("Light Attack");
				//Delay
				StartCoroutine("waitForMelee", 0.5f);
			}
			else if(lightAttack && downAttack)
			{
				movementController.attacking = true;
				spriteUp.SetActive(false);
				spriteDown.SetActive(true);
				spriteRight.SetActive(false);
				spriteLeft.SetActive(false);
				animDown.Play("Light Attack");
				//Delay
				StartCoroutine("waitForMelee", 0.5f);
			}
			else if(lightAttack && rightAttack)
			{
				movementController.attacking = true;
				spriteUp.SetActive(false);
				spriteDown.SetActive(false);
				spriteRight.SetActive(true);
				spriteLeft.SetActive(false);
				animRight.Play("Light Attack");

				//Delay
				StartCoroutine("waitForMelee", 0.5f);
			}
			else if(lightAttack && leftAttack)
			{
				movementController.attacking = true;				
				spriteUp.SetActive(false);
				spriteDown.SetActive(false);
				spriteRight.SetActive(false);
				spriteLeft.SetActive(true);
				animLeft.Play("Light Attack");


				//Delay
				StartCoroutine("waitForMelee", 0.5f);
			}

			//Animation for heavy attack
			else if (heavyAttack && upAttack)
			{
				movementController.attacking = true;
				spriteUp.SetActive(true);
				spriteDown.SetActive(false);
				spriteRight.SetActive(false);
				spriteLeft.SetActive(false);
				animUp.Play("Heavy Attack");

				//Delay
				StartCoroutine("waitForMelee", 0.6f);
			}
			else if (heavyAttack && downAttack)
			{
				movementController.attacking = true;
				spriteUp.SetActive(false);
				spriteDown.SetActive(true);
				spriteRight.SetActive(false);
				spriteLeft.SetActive(false);
				animDown.Play("Heavy Attack");

				//Delay
				StartCoroutine("waitForMelee", 0.6f);
			}
			else if (heavyAttack && rightAttack)
			{
				movementController.attacking = true;
				spriteUp.SetActive(false);
				spriteDown.SetActive(false);
				spriteRight.SetActive(true);
				spriteLeft.SetActive(false);
				animRight.Play("Heavy Attack");

				//Delay
				StartCoroutine("waitForMelee", 0.6f);
			}
			else if (heavyAttack && leftAttack)
			{
				movementController.attacking = true;
				spriteUp.SetActive(false);
				spriteDown.SetActive(false);
				spriteRight.SetActive(false);
				spriteLeft.SetActive(true);
				animLeft.Play("Heavy Attack");

				//Delay
				StartCoroutine("waitForMelee", 0.6f);
			}
		}
	}

	IEnumerator waitForMelee(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);

		if(specialAttackIsQueued & Time.time > specialAttackTimer)
		{
			specialAttackTimer = Time.time + specialAttackCooldown;
			if (weapon.GetComponent<BaseWeapon>().attackID == 0)
			{
				Instantiate(lightEffect, transform.position, transform.rotation);
				//Special attack for the light attack
				specialAttackIsQueued = false;
				Collider2D[] enemies = Physics2D.OverlapBoxAll(attackPoint.position, weapon.GetComponent<BaseWeapon>().attackBox, 0f, enemyLayer, -100f, 100f);
				foreach(Collider2D enemy in enemies)
				{
					if (enemy.gameObject != null)
					{
						if (enemy.gameObject.tag == "Enemy" || enemy.gameObject.tag == "Boss")
						{
							playerAudio.PlayOneShot(weapon.GetComponent<BaseWeapon>().attackSound);
							enemy.GetComponent<EnemyHealthManager>().LoseHealth((weapon.GetComponent<BaseWeapon>().attackDamage + additionalDamage + upgradedStats.GetComponent<Stats>().upgradedDamage) * lightAttackSpecialBonus, weapon.GetComponent<BaseWeapon>().attackID);
							break;
						}
					}
				}
			}
			else if (weapon.GetComponent<BaseWeapon>().attackID == 1)
			{
				//Special attack for the heavy attack
				//Still need to adjust the attack box
				Debug.Log("Performing special heavy");
				Vector2 specialAttackBox = new Vector2(3, 3);
				circleSlashFillAmount = 1f;
				specialAttackIsQueued = false;
				Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.position, specialAttackBox, 0f, enemyLayer, -100f, 100f);
				foreach (Collider2D enemy in enemies)
				{
					if (enemy.gameObject != null)
					{
						if (enemy.gameObject.tag == "Enemy" || enemy.gameObject.tag == "Boss")
						{
							playerAudio.PlayOneShot(weapon.GetComponent<BaseWeapon>().attackSound);
							enemy.GetComponent<EnemyHealthManager>().LoseHealth((weapon.GetComponent<BaseWeapon>().attackDamage + additionalDamage + upgradedStats.GetComponent<Stats>().upgradedDamage), weapon.GetComponent<BaseWeapon>().attackID);
						}
					}
				}
			}
		}
		else
		{
			//Normal melee attack
			Collider2D[] enemies = Physics2D.OverlapBoxAll(attackPoint.position, weapon.GetComponent<BaseWeapon>().attackBox, 0f, enemyLayer, -100f, 100f);
			foreach (Collider2D enemy in enemies)
			{
				//UnityEngine.Debug.Log("We hit " + enemy.name);
				if (enemy.gameObject != null)
				{
					if (enemy.gameObject.tag == "Enemy" || enemy.gameObject.tag == "Boss")
					{
						playerAudio.PlayOneShot(weapon.GetComponent<BaseWeapon>().attackSound);
						enemy.GetComponent<EnemyHealthManager>().LoseHealth(weapon.GetComponent<BaseWeapon>().attackDamage + additionalDamage + upgradedStats.GetComponent<Stats>().upgradedDamage, weapon.GetComponent<BaseWeapon>().attackID);
						if (weapon.GetComponent<BaseWeapon>().attackID == 0)
						{
							break;
						}
					}
				}
			}
		}
		yield return null;
	}

	IEnumerator waitForRanged(float waitTime, float arrowRotation, Vector3 arrowPosition, bool flipOnX)
	{
		yield return new WaitForSeconds(waitTime);

		if (specialAttackIsQueued & Time.time > specialAttackTimer)
		{
			specialAttackTimer = Time.time + specialAttackCooldown;
			float rotationArc = 45;
			float rotationIncrement = 22.5f;
			float currRotation = arrowRotation - rotationArc;
			float endingRotation = arrowRotation + rotationArc;
			int i = (int)(rotationArc / rotationIncrement);
			while (currRotation <= endingRotation)
			{
				GameObject newArrow;
				//Need to fix arrow rotation on the special attack
				if (i < 0)
				{
					newArrow = Instantiate(Arrow, arrowPosition, Quaternion.Euler(0, 0, currRotation + ((i - 1) * rotationIncrement)));
				}
				else if (i > 0)
				{
					newArrow = Instantiate(Arrow, arrowPosition, Quaternion.Euler(0, 0, currRotation + ((i + 1) * rotationIncrement)));
				}
				else
				{
					newArrow = Instantiate(Arrow, arrowPosition, Quaternion.Euler(0, 0, currRotation));
				}
				 
				newArrow.GetComponent<SpriteRenderer>().flipX = flipOnX;
				newArrow.GetComponent<Arrow>().SetRotation(currRotation - 90f);
				newArrow.GetComponent<Arrow>().additionalDamage = additionalDamage;
				newArrow.GetComponent<Arrow>().upgradedStats = upgradedStats;
				playerAudio.PlayOneShot(specialRangedSound);
				currRotation += rotationIncrement;
				i--;
			}

			specialAttackIsQueued = false;
			yield return null;
		}
		else
		{       
			//Normal ranged attack
			//Need to fix left and right arrow sprite
			GameObject tempArrow = Instantiate(Arrow, arrowPosition, Quaternion.Euler(0, 0, arrowRotation));
			tempArrow.GetComponent<SpriteRenderer>().flipX = flipOnX;
			tempArrow.GetComponent<Arrow>().SetRotation(arrowRotation - 90f);
			tempArrow.GetComponent<Arrow>().additionalDamage = additionalDamage;
			tempArrow.GetComponent<Arrow>().upgradedStats = upgradedStats;
			playerAudio.PlayOneShot(weapon.GetComponent<BaseWeapon>().attackSound);
		}
		yield return null;
	}
}
