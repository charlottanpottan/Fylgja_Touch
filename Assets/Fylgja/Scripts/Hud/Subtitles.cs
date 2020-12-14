using System;
using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityEngine.UI;



public class Subtitles : MonoBehaviour
{
    public Text text;
	public int textSize = 30;
	public float delayBetweenSubtitles = 0.2f;
	public float delayAfterSubtitle = 0.5f;
	float closedTextAtTime;
	string nextTitle = "";
	float nextTitleTime;
	private float showedTitleAtTime;
	enum Command
	{
		WaitTime,
		VideoTime
	}

	private Command nextCommand;
	bool titleEnabled = false;
	bool shouldBeVisible = false;
	private MoviePlayerToCamera moviePlayerToCamera;

	
	void Start()
	{
		text.text = string.Empty;
		ShowBorder(false);
	}

	bool DoesLogicallyHaveText()
	{
		return text.text.Length != 0;
	}

	bool IsLongTimeSinceWeLogicallyClosedLine()
	{
		Debug.Log($"IsLongTimeSinceWeLogicallyClosedLine: {Time.time} > {closedTextAtTime}");
		return Time.time > closedTextAtTime + delayAfterSubtitle;
	}

	bool IsShowingBorderButNoLogicalText()
	{
		Debug.Log($"IsShowingBorderButNoLogicalText: {(text.text == string.Empty)} && {titleEnabled}");
		return (text.text == string.Empty) && titleEnabled;
	}


	bool IsThereUpcomingText()
	{
		return nextTitle.Length != 0;
	}

	bool IsNextLineComingUpRealSoon(float referenceTime)
	{
		return referenceTime >= nextTitleTime - delayBetweenSubtitles;
	}

	bool IsTimeForNextLine(float referenceTime)
	{
		return referenceTime >= nextTitleTime;
	}

	void CheckUpcomingText(float referenceTime)
	{
		if (DoesLogicallyHaveText() && IsNextLineComingUpRealSoon(referenceTime))
		{
			Debug.Log($"nextTitle forced subtitle stop of line:{text.text}");
			CloseSubtitle();
		}

		if (IsThereUpcomingText() && IsTimeForNextLine(referenceTime))
		{
			Debug.Log($"nextTitle moved on to next subtitle {nextTitle} because reference time {referenceTime} > title time: {nextTitleTime}");
			var nextTitleToShow = nextTitle;
		
			nextTitle = string.Empty;
			OnSubtitleStart(nextTitleToShow);
		}
	}

	void ShowBorder(bool on)
	{
		Debug.Log($"Show border {on}");
		titleEnabled = on;
		text.transform.parent.gameObject.SetActive(on);
	}
	
	void Update()
	{
		var referenceTime = Time.time;
		if (nextCommand == Command.VideoTime)
		{
			moviePlayerToCamera = FindObjectOfType<MoviePlayerToCamera>();
			if (moviePlayerToCamera == null)
			{
				return;
			}
			var movieTimeInSeconds = moviePlayerToCamera.videoPlayer.time;
			referenceTime = (float) movieTimeInSeconds;
		}
		
		if (IsShowingBorderButNoLogicalText() && IsLongTimeSinceWeLogicallyClosedLine())
		{
			Debug.Log("Subtitle timed out subtitle");
			ShowBorder(false);
		}
		
		if (IsThereUpcomingText())
		{
			CheckUpcomingText(referenceTime);
		}
	}

	public bool ShouldBeVisible
	{
		set
		{
			shouldBeVisible = value;
		}
	}

	bool ParseCommandIfFound(string title, out string nextText)
	{
		var escapeIndex = title.IndexOf('%');

		nextText = title;

		if (escapeIndex != -1)
		{
			var endEscapeIndex = title.IndexOf(' ', escapeIndex + 1);
			DebugUtilities.Assert(endEscapeIndex != -1, "Illegal formatting:" + title);
			var escapeCode = title.Substring(escapeIndex + 1, endEscapeIndex - escapeIndex);
			var textBefore = title.Substring(0, escapeIndex).TrimEnd();
			var textAfter = title.Substring(endEscapeIndex + 1);
			var parameters = escapeCode.Substring(1);

			nextText = textBefore;
			
			switch (escapeCode[0])
			{
				case 'w':
				{
					var waitTime = float.Parse(parameters);
					nextTitle = textAfter;
					nextTitleTime = Time.time + waitTime;
					nextCommand = Command.WaitTime;
				
					Debug.Log("Waiting time:" + waitTime + " remaining:" + nextTitle + " showing:" + textBefore);
				   
					nextText = textAfter;
					break;
				}
				case 't':
				{
					// Couldn't get TimeSpan.ParseExact to work, so added custom parsing.
					var separator = parameters.IndexOf(':');
					if (separator == -1)
					{
						Debug.LogError($"wrong time format '{parameters}'");
					}

					var minutesString = parameters.Substring(0, separator);
					var secondsString = parameters.Substring(separator + 1);
					var minutes = int.Parse(minutesString);
					var seconds = int.Parse(secondsString);
					
					nextTitleTime = minutes * 60 + seconds;
					nextTitle = textAfter;
					nextCommand = Command.VideoTime;
					break;
				}
			}
		}

		return escapeIndex != -1;
	}
	
	public string ParseUpcomingCommandsAndReturnStringToDisplay(string title)
	{
		ParseCommandIfFound(title, out title);
		
		if (title.Length > 80)
		{
			var breakChars = new[]{'.', ',', ' ', ':', '-'};
			var index = title.IndexOfAny(breakChars, title.Length / 2);
			return title.Substring(0, index + 1) + "\n" + title.Substring(index + 1);
		}
		else
		{
			return title;
		}
	}
	
	public void OnSubtitleStart(string title)
	{
		Debug.Log("Subtitle start: (before)" + title);
		if (title == string.Empty)
		{
			return;
		}
		var textToDisplay = ParseUpcomingCommandsAndReturnStringToDisplay(title);
		if (textToDisplay.Trim() == "|")
		{
			Debug.Log("Intentionally empty line. Do not show anything.");
			textToDisplay = string.Empty;
			ShowBorder(false);
		}

		text.text = textToDisplay;
		
		if (text.text.Length == 0)
		{
			return;
		}
		
		Debug.Log($"Subtitle start: (after) {{text.text}} next subtitle is at {nextTitleTime} with '{nextTitle}'");
		if (shouldBeVisible)
		{
			ShowBorder(true);
			showedTitleAtTime = Time.time;
		}
	}

	/// <summary>
	/// Clears the text and it will eventually time out and close the border around the subtitles.
	/// </summary>
	void CloseSubtitle()
	{
		Debug.Log("Subtitle close");
		text.text = string.Empty;
		closedTextAtTime = Time.time;
	}
	
	/// <summary>
	/// Forces the subtitles to not show any text or border. Usually since a movie or cutscene has ended.
	/// </summary>
	public void OnSubtitleStop()
	{
		Debug.Log("Subtitle stop");
		CloseSubtitle();
		nextTitle = string.Empty;
		nextTitleTime = 0;
		ShowBorder(false);
	}
}
