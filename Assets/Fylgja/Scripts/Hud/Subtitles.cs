using UnityEngine;
using System.Collections;

public class Subtitles : MonoBehaviour
{
	public GUIText text;
	public int textSize;
	public float delayBetweenSubtitles = 0.2f;
	public float delayAfterSubtitle = 0.5f;
	float closedTextAtTime;
	string nextTitle = "";
	float nextTitleTime;
	bool titleEnabled = false;
	bool shouldBeVisible = false;
	
	void Start()
	{
		text.text = string.Empty;
		text.transform.parent.gameObject.SetActive(false);
	}
	
	void Update()
	{
		if (text.text == string.Empty && titleEnabled && Time.time > closedTextAtTime + delayAfterSubtitle)
		{
			Debug.Log("Subtitle timed out subtitle");
			titleEnabled = false;
			text.transform.parent.gameObject.SetActive(false);
		}
		
		if (nextTitle.Length != 0)
		{
			if ((text.text != string.Empty) && (Time.time >= nextTitleTime - delayBetweenSubtitles))
			{
				Debug.Log("nextTitle forced subtitle stop of line:" + text.text);
				CloseSubtitle();
			}
			if (Time.time >= nextTitleTime)
			{
				Debug.Log("nextTitle forced next subtitle:" + nextTitle);
				var nextTitleToShow = nextTitle;
				nextTitle = string.Empty;
				OnSubtitleStart(nextTitleToShow);
			}
		}
		text.fontSize = Screen.height / textSize;
	}

	public bool ShouldBeVisible
	{
		set
		{
			shouldBeVisible = value;
		}
	}
	
	public string AddLinebreaks(string title)
	{
		var escapeIndex = title.IndexOf('%');
		if (escapeIndex != -1)
		{
			var endEscapeIndex = title.IndexOf(' ', escapeIndex + 1);
			DebugUtilities.Assert(endEscapeIndex != -1, "Illegal formatting:" + title);
			var escapeCode = title.Substring(escapeIndex + 1, endEscapeIndex - escapeIndex);
			if (escapeCode[0] == 'w')
			{
				var waitTime = float.Parse(escapeCode.Substring(1));
				var textBefore = title.Substring(0, escapeIndex);
				nextTitle = title.Substring(endEscapeIndex + 1);
				nextTitleTime = Time.time + waitTime;
				
				Debug.Log("Waiting time:" + waitTime + " remaining:" + nextTitle + " showing:" + textBefore);
				   
				title = textBefore;
			}
		}
		
		if (title.Length > 80)
		{
			var breakChars = new char[]{'.', ',', ' ', ':', '-'};
			int index = title.IndexOfAny(breakChars, title.Length / 2);
			return title.Substring(0, index + 1) + "\n" + title.Substring(index + 1);
		}
		else
		{
			return title;
		}
	}
	
	public void OnSubtitleStart(string title)
	{
		Debug.Log("Subtitle start:" + title);
		if (title == string.Empty)
		{
			return;
		}
		text.text = AddLinebreaks(title);
		if (shouldBeVisible)
		{
			titleEnabled = true;
			text.transform.parent.gameObject.SetActive(true);
		}
	}

	void CloseSubtitle()
	{
		Debug.Log("Subtitle stop:");
		text.text = string.Empty;
		closedTextAtTime = Time.time;
	}
	
	public void OnSubtitleStop()
	{
		CloseSubtitle();
		nextTitle = string.Empty;
		nextTitleTime = 0;
	}
}
