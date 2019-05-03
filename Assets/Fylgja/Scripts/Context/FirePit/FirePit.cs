using UnityEngine;
using System.Collections;

public class FirePit : ActionArbitration
{
	private float timePassed;
	bool debug = false;
	int debugHealthChangeEachSecond = 15;
	bool isIgniting = false;

	public int flameDecayPerSecond = 5;
	private int health = 0;
	public int maxHealth = 150;

	public IFirePitNotifications firePitNotification;

	void Start()
	{
		health = maxHealth;
		StrengthChanged();
		enabled = false;
	}
	
	void Awake()
	{
		
	}

	void Update()
	{
		timePassed += Time.deltaTime;
		int secondsPassed = (int) timePassed;
		for (int i = 0; i < secondsPassed; ++i)
		{
			SecondPassed();
		}
		timePassed -= secondsPassed;
	}

	void SecondPassed()
	{
		int oldHealth = health;

		if (debug)
		{
			health += debugHealthChangeEachSecond;
			if (health > maxHealth)
			{
				health = maxHealth;
				debugHealthChangeEachSecond = -debugHealthChangeEachSecond;
			}
			else if (health < 0)
			{
				health = 0;
				debugHealthChangeEachSecond = -debugHealthChangeEachSecond;
			}
		}
		else
		{
			health -= flameDecayPerSecond;
			health = Mathf.Max(0, health);
		}

		CheckHealth(oldHealth);
	}

	void CheckHealth(int oldHealth)
	{
		const int fullFlameThreshold = 100;
		const int fullFlameNotificationThreshold = 100;
		const int flameDeadThreshold = 0;

		if (oldHealth > flameDeadThreshold && health == flameDeadThreshold)
		{
			BroadcastMessage("OnFirePitDied");
		}
		else if (oldHealth >= fullFlameThreshold && health < fullFlameThreshold)
		{
			Debug.Log("LostFullFrame");
			BroadcastMessage("OnFirePitLostFullFlame");
		}
		else if (oldHealth < fullFlameNotificationThreshold && health >= fullFlameNotificationThreshold)
		{
			health = maxHealth;
			Debug.Log("We have FULL FLAME!");
			if (firePitNotification != null)
			{
				Debug.Log("Reporting full flame to minigame");
				firePitNotification.OnFirePitFullFlame();
			}
			BroadcastMessage("OnFirePitFullFlame");
		}

		StrengthChanged();
	}

	void StrengthChanged()
	{
		// Debug.Log("Strength:" + FireStrengthFactor());
		BroadcastMessage("OnFirePitStrength", FireStrengthFactor());
	}

	public float FireStrengthFactor()
	{
		return Mathf.Min(100, health) / 100.0f;
	}

	public void StartIgniting()
	{
		isIgniting = true;
	}

	public void AbortIgnite()
	{
		isIgniting = false;
	}

	public void Ignited()
	{
		BroadcastMessage("OnFirePitIgnited");
		DebugUtilities.Assert(health == 0, "You can only ignite when the fire is dead");
		int oldHealth = health;
		health = 20;
		CheckHealth(oldHealth);
		isIgniting = false;
	}

	public bool IsIgniting()
	{
		return isIgniting;
	}

	public bool IsIgnited()
	{
		return(health > 0);
	}

	public bool IsDead()
	{
		return(health == 0);
	}

	public bool IsFullFlame()
	{
		return(health >= 100);
	}

	public override bool IsActionPossible(IAvatar avatar)
	{
		var igniter = avatar.GetComponentInChildren<CharacterFirePitMinigame>();
		return !IsFullFlame() && !IsIgniting() && igniter.HasFirePitTools();
	}

	public override void ExecuteAction(IAvatar avatar)
	{
		var igniter = avatar.GetComponentInChildren<CharacterFirePitMinigame>();
		if (!IsIgniting() && igniter.HasFirePitTools())
		{
			if (IsDead())
			{
				avatar.IgniteFirePit(this);
			}
			else if (!IsFullFlame())
			{
				avatar.FanFirePit(this);
			}
		}
	}

	public void Fan(int addedHealth)
	{
		int oldHealth = health;

		DebugUtilities.Assert(addedHealth >= 0 && addedHealth <= 10, "You can not add that amount to health");
		health += addedHealth;
		health = Mathf.Min(maxHealth, health);
		if (addedHealth < 4)
		{
			BroadcastMessage("OnFirePitFanFailed");
		}
		else if (addedHealth < 10)
		{
			BroadcastMessage("OnFirePitSmallFan");
		}
		else
		{
			BroadcastMessage("OnFirePitBigFan");
		} CheckHealth(oldHealth);
	}

	public void BurnForever()
	{
		enabled = false;
	}
	
	public void ResetFire()
	{
		health = 0;	
		BroadcastMessage("ResetEffects");
		enabled = true;
	}
}
