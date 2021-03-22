using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
	public Vector2 attackBox;
	public int attackID;
	public float attackDamage;
	public float attackRange;
	public float attackRate;
	public AudioClip attackSound;

	public bool rangedAttack;
	public float destroyDelay;
}
