using UnityEngine;

public class DebugSubtitles : MonoBehaviour
{
    public Subtitles subtitles;

    private float nextTime;

    void Start()
    {
        nextTime = Time.time + 2.0f;
        subtitles.ShouldBeVisible = true;
    }

    void Update()
    {
        if (Time.time > nextTime)
        {
            subtitles.OnSubtitleStart("%w1 Hej Charlotte! Kan du läsa detta?\nDetta är andra raden... %w4 Detta är en enkelrad");
            nextTime = Time.time + 10.0f;
        }
    }
}
