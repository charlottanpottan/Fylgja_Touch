using UnityEngine;

public class QuestParchment : ActionArbitration
{
	public Renderer progressRenderer;
	public Texture[] questTextures;
	public Texture noQuestTexture;
	public AudioHandler parchmentCloseHandler;
	float targetProgression;
	float progression = -9.0f;

	void Start()
	{
		SetProgression(0.0f);
	}


	void OnQuestParchmentClose()
	{
		parchmentCloseHandler.CreateAndPlay(GetComponent<AudioSource>().volume);
		Destroy(gameObject);
	}

	void Update()
	{
		if (Mathf.Abs(progression - targetProgression) > 0.01f)
		{
			progression = Mathf.Lerp(progression, targetProgression, 0.04f);
			SetShaderValue(progression);
		}
	}

	public override void ExecuteAction(IAvatar avatar)
	{
		avatar.CloseQuestParchment();
	}

	public void SetProgression(float f)
	{
		targetProgression = f;
	}

	public void SetQuest(Quest quest)
	{
		if (quest == null)
		{
			return;
		}
		DebugUtilities.Assert(quest != null, "Null quest passed to Quest Parchment");
		var textureName = quest.questName + "Parchment_DIFFUSE";
		foreach (var texture in questTextures)
		{
			if (texture.name == textureName)
			{
				Debug.Log("Texture " + textureName + " found!");
				SetTexture(texture);
				return;
			}
		}
		Debug.Log("Texture not found");
		//Debug.LogWarning("Couldn't find texture for quest:" + quest.questName + " texture:" + textureName);

		// DebugUtilities.Assert(false, "Couldn't find texture for quest:" + quest.questName + " texture:" + textureName);
	}

	public void SetQuestCompleted()
	{
		Debug.Log("Setting no texture on parchment!");
		SetTexture(noQuestTexture);
	}

	void SetTexture(Texture texture)
	{
		Debug.Log("Setting texture:" + texture.name);
		progressRenderer.material.SetTexture("_QuestTex", texture);
	}

	void SetShaderValue(float f)
	{
		progressRenderer.material.SetFloat("_QuestProgression", f);
	}
}
