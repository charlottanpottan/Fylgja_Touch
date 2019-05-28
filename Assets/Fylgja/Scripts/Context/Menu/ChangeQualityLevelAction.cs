using UnityEngine;
using System.Collections;

public class ChangeQualityLevelAction : ActionArbitration
{
    [SerializeField] FylgjaQualityLevel targetQuality;
    [SerializeField] AudioClip clickAudio = null;
    [SerializeField] AudioHandler audioHandler = null;
    [SerializeField] float audioVolume = 0.2f;
    [SerializeField] Renderer targetRenderer = null;

    void Awake()
    {
        CheckQualityLevel();
    }

    public override void ExecuteAction(IAvatar avatar)
    {
        if (clickAudio != null)
        {
            audioHandler.CreateAndPlay(clickAudio, audioVolume);
        }
        else
        {
            audioHandler.CreateAndPlay(audioVolume);
        }

        QualitySettings.SetQualityLevel((int)targetQuality);
        Camera.main.SendMessage("ChangeQualityLevel", SendMessageOptions.DontRequireReceiver);
        Debug.Log(QualitySettings.GetQualityLevel());
        transform.parent.BroadcastMessage("CheckQualityLevel");
    }

    public void CheckQualityLevel()
    {
        if (targetRenderer.material.HasProperty("_BlendRange"))
        {
            if (QualitySettings.GetQualityLevel() == (int)targetQuality)
            {
                targetRenderer.material.SetFloat("_BlendRange", 1f);
            }
            else
            {
                targetRenderer.material.SetFloat("_BlendRange", 0f);
            }
        }
    }
}
