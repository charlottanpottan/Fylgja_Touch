using UnityEngine;
using System.Collections;

public class BlacksmithSword : MonoBehaviour
{
	public GameObject dentRoot;
	public float dentRadius;
	public float normalizedAnnealedTemperature = 1.0f;
	public float normalizedAnnealedTemperatureChangePerSecond = -0.01f;
	public float normalizedAnnealedTemperatureHeatChangePerSecond = 3.0f;
	public float perfectHitStrength = 5f;
	private Interactable annealInteractable;
	private CharacterAvatar avatar;
	float annealTimeLeft;
	bool isPaused;
	float effects;
	bool isAnnealed = true;
	bool swordIsCompleted;

	void Start()
	{
		annealInteractable = GameObject.FindGameObjectWithTag("AnnealPoint").GetComponent<Interactable>();
		avatar = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CharacterAvatar>();
	}

	void Update()
	{
		if( isPaused == false)
		{
			UpdateAnnealed();
		}
	}

	void UpdateAnnealed()
	{
		normalizedAnnealedTemperature += normalizedAnnealedTemperatureChangePerSecond * Time.deltaTime;
		normalizedAnnealedTemperature = Mathf.Clamp(normalizedAnnealedTemperature, 0.0f, 1.0f);
		if (annealTimeLeft > 0)
		{
			normalizedAnnealedTemperature += normalizedAnnealedTemperatureHeatChangePerSecond * Time.deltaTime;
			annealTimeLeft -= Time.deltaTime;
		}

		if (isAnnealed)
		{
			if (normalizedAnnealedTemperature <= 0.0f)
			{
				TriggerOnBlacksmithSwordCold();
				isAnnealed = false;
			}
		}
		else
		{
			isAnnealed = normalizedAnnealedTemperature > 0.0f;
		}

		TriggerOnBlacksmithSwordAnnealedChanged(normalizedAnnealedTemperature);
	}
	
	public void IsPaused(bool pauseState)
	{
		isPaused = pauseState;	
	}

	public void AnnealSword()
	{
		annealTimeLeft = 4.0f;
	}

	public bool IsSwordDone()
	{
		return swordIsCompleted;
	}

	bool CheckIfSwordIsDone()
	{
		var dents = dentRoot.GetComponentsInChildren<BlacksmithSwordBump>();

		foreach (BlacksmithSwordBump dent in dents)
		{
			if (!dent.IsBumpDone())
			{
				return false;
			}
		}
		return true;
	}

	public bool HitWithSledgeHammer(Vector3 hitLocation)
	{
		DebugUtilities.Assert(!swordIsCompleted, "You can not hit a sword that is completed!");
		if (isAnnealed)
		{
			CheckForDentHit(hitLocation);
			swordIsCompleted = CheckIfSwordIsDone();
			return swordIsCompleted;
		}
		else
		{
			TriggerOnSledgeHammerHitColdSword();
		}

		return false;
	}

	BlacksmithSwordBump FindClosestDent(Vector3 point)
	{
		point.y = 0;
		float closestDelta = 0;
		BlacksmithSwordBump closestDent = null;

		var dents = dentRoot.GetComponentsInChildren<BlacksmithSwordBump>();
		foreach (BlacksmithSwordBump dent in dents)
		{
			if(!dent.IsBumpDone())
			{
				Vector3 dentPosition = dent.transform.position;
				dentPosition.y = 0;
				float delta = (point - dentPosition).magnitude;
				if (closestDent == null || delta < closestDelta)
				{
					closestDent = dent;
					closestDelta = delta;
				}
			}
		}


		return closestDent;
	}

	void CheckForDentHit(Vector3 sledgeHitPositionInWorld)
	{
		var dent = FindClosestDent(sledgeHitPositionInWorld);

		var delta = (dent.transform.position - sledgeHitPositionInWorld);

		delta.y = 0;

		var missedBy = delta.magnitude;
		if (missedBy > dentRadius)
		{
			TriggerOnSledgeHammerMissedDent();
		}
		else
		{
			float perfectHitFactor = perfectHitStrength - (missedBy / dentRadius);
			dent.HitBump(perfectHitFactor);
			TriggerOnSledgeHammerHitDent(perfectHitFactor);
		}
	}

	void TriggerOnSledgeHammerMissedDent()
	{
		BroadcastMessage("OnSledgeHammerMissedDent");
	}

	void TriggerOnSledgeHammerHitDent(float perfectHitFactor)
	{
		BroadcastMessage("OnSledgeHammerHitDent", perfectHitFactor);
	}

	void TriggerOnSledgeHammerHitColdSword()
	{
		Debug.Log("Sword is cold");
		BroadcastMessage("OnSledgeHammerHitColdSword");
	}

	void TriggerOnBlacksmithSwordCold()
	{
		BroadcastMessage("OnBlacksmithSwordCold");
		DebugUtilities.Assert(avatar != null, "Please set collision masks. Nothing other than CharacterAvatars should trigger this");
		DebugUtilities.Assert(annealInteractable != null, "You must set up interactable for InteractOnTrigger:" + name);
		avatar.PerformPrimaryAction(annealInteractable.gameObject);
	}

	void TriggerOnBlacksmithSwordAnnealedChanged(float normalizedAnnealedTemperature)
	{
		BroadcastMessage("OnBlacksmithSwordAnnealedChanged", normalizedAnnealedTemperature);
	}
}

