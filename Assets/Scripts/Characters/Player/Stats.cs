using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
	public static Stats playerStats;

	public float upgradedDamage = 0.0f;
	public float upgradedSpeed = 0.0f;
	public float upgradedAttackRate = 0.0f;
	public int souls = 0;
	private static int MAX_STAT_TOTAL = 30;

	public Text currentDamage;
	public Text currentSpeed;
	public Text currentRate;
	public Text total;

	Stat damageStatBar;
	Stat speedStatBar;
	Stat rateStatBar;

	public bool limboRan;
	public bool gluttonyRan;

	void Awake()
	{
		if(playerStats != null)
		{
			GameObject.Destroy(playerStats);
		}
		else
		{
			playerStats = this;
		}

		DontDestroyOnLoad(this);
		limboRan = false;
		gluttonyRan = false;
	}

	void OnApplicationQuit()
	{
		upgradedDamage = 0.0f;
		upgradedSpeed = 0.0f;
		upgradedAttackRate = 0.0f;
		souls = 0;
	}


	public void Start()
	{
		if (SceneManager.GetActiveScene().name == "StatScreen")
		{
			damageStatBar = GameObject.Find("Damage Bar").GetComponent<Stat>();
			speedStatBar = GameObject.Find("Speed Bar").GetComponent<Stat>();
			rateStatBar = GameObject.Find("Rate Bar").GetComponent<Stat>();

			currentDamage = GameObject.Find("Attack Damage Total Text").GetComponent<Text>();
			currentSpeed = GameObject.Find("Movement Speed Total Text").GetComponent<Text>();
			currentRate = GameObject.Find("Attack Rate Total Text").GetComponent<Text>();
			total = GameObject.Find("Total Text").GetComponent<Text>();

			damageStatBar.Initialize(upgradedDamage, (float)MAX_STAT_TOTAL);
			speedStatBar.Initialize(upgradedSpeed, (float)MAX_STAT_TOTAL);
			rateStatBar.Initialize(upgradedAttackRate, (float)MAX_STAT_TOTAL);
			updateSoulDisplay(souls);
			updateTextDisplay(upgradedDamage, upgradedSpeed, upgradedAttackRate);

		}
	}

	void Update()
	{
		if (SceneManager.GetActiveScene().name == "StatScreen")
		{
			updateSoulDisplay(souls);
		}

		if (SceneManager.GetActiveScene().name == "LimboV2")
		{
			limboRan = true;
		}

		if (SceneManager.GetActiveScene().name == "GluttonyV2")
		{
			gluttonyRan = true;
		}

	}

	public void updateSoulDisplay(int numToDisplay)
    {
		total.text = "" + numToDisplay;
	}

	public void updateTextDisplay(float damage, float speed, float rate)
	{
		currentDamage.text = (int)damage + "/" + MAX_STAT_TOTAL;
		currentSpeed.text = (int)speed + "/" + MAX_STAT_TOTAL;
		currentRate.text = (int)rate + "/" + MAX_STAT_TOTAL;
	}

	public void upgradeDamage()
	{
		if(souls > 0 && upgradedDamage < MAX_STAT_TOTAL)
		{
			//5.0
			upgradedDamage += 0.5f;
			
			// Puts cap on the highest damage you can get
			if (upgradedDamage >= MAX_STAT_TOTAL)
			{
				upgradedDamage = MAX_STAT_TOTAL;
			}
			souls--;
			damageStatBar.Initialize(upgradedDamage, (float) MAX_STAT_TOTAL);
		}
		else
		{
			//Maybe a sound effect or something visual here
		}

		currentDamage.text = upgradedDamage + "/" + MAX_STAT_TOTAL;
	}

	public void upgradeSpeed()
	{
		if (souls > 0 && upgradedSpeed < MAX_STAT_TOTAL)
		{
			upgradedSpeed += 1.0f;
			// Puts cap on the highest speed you can get
			if (upgradedSpeed >= MAX_STAT_TOTAL)
			{
				upgradedSpeed = MAX_STAT_TOTAL;
			}
			souls--;
			speedStatBar.Initialize(upgradedSpeed, (float)MAX_STAT_TOTAL);
		}
		else
		{
			//Maybe a sound effect or something visual here
		}
		currentSpeed.text = upgradedSpeed + "/" + MAX_STAT_TOTAL;
	}

	public void upgradeAttackRate()
	{
		if (souls > 0 && upgradedAttackRate < MAX_STAT_TOTAL)
		{
			//0.25
			upgradedAttackRate += 0.25f;
			// Puts cap on the highest rate you can get
			if (upgradedAttackRate >= MAX_STAT_TOTAL)
			{
				upgradedAttackRate = MAX_STAT_TOTAL;
			}
			souls--;
			rateStatBar.Initialize(upgradedAttackRate, (float)MAX_STAT_TOTAL);
		}
		else
		{
			//Maybe a sound effect or something visual here
		}

		currentRate.text = upgradedAttackRate + "/" + MAX_STAT_TOTAL;
	}

	public void giveSouls(int num)
	{
		souls += num;
	}

	public void clearSouls()
    {
		souls = 0;
    }
}
