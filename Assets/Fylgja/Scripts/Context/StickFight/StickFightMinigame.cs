using UnityEngine;

public class StickFightMinigame : Minigame
{
    public Camera stickFightCamera;
    public CharacterOpponentStickFight opponentStickFighter;
    public StickFightHitButton stickFightHitButtonRight;
    public StickFightHitButton stickFightHitButtonLeft;

    public override bool AllowedToMove()
    {
        return false;
    }

    public override void StartMinigame(IAvatar a)
    {
        Debug.Log("StickFightMinigame request start!");
        base.StartMinigame(a);
        opponentStickFighter.gameObject.BroadcastMessage("OnStickFightMinigameStart", this);
        avatar.transform.parent.BroadcastMessage("OnStickFightMinigameStart", this);
        stickFightHitButtonRight.Avatar = avatar;
        stickFightHitButtonLeft.Avatar = avatar;
        stickFightHitButtonRight.gameObject.SetActive(SystemInfo.deviceType == DeviceType.Handheld);
        stickFightHitButtonLeft.gameObject.SetActive(SystemInfo.deviceType == DeviceType.Handheld);
    }

    void CloseStickFightMinigame()
    {
        stickFightHitButtonRight.gameObject.SetActive(false);
        stickFightHitButtonLeft.gameObject.SetActive(false);
        opponentStickFighter.gameObject.BroadcastMessage("OnStickFightMinigameClose");
        avatar.transform.parent.BroadcastMessage("OnStickFightMinigameClose");
    }

    public override void QuitMinigame()
    {
        Debug.Log("Closed minigame!");
        CloseStickFightMinigame();
        //opponentStickFighter.gameObject.BroadcastMessage("OnStickFightMinigameQuit");
        avatar.transform.parent.BroadcastMessage("OnStickFightMinigameQuit");
        base.QuitMinigame();
    }

    public override void CompletedMinigame()
    {
        CloseStickFightMinigame();
        // avatar.transform.parent.BroadcastMessage("OnStickFightMinigameDone");
        base.CompletedMinigame();
    }

    public override void FailedMinigame()
    {
        // avatar.transform.parent.BroadcastMessage("OnStickFightMinigameFailed");
        CloseStickFightMinigame();
        base.FailedMinigame();
    }

    public void OnOpponentLost()
    {
        CompletedMinigame();
    }

    public void OnCharacterLost()
    {
        FailedMinigame();
    }
}

