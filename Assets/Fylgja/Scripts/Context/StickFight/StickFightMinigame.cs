using UnityEngine;

public class StickFightMinigame : Minigame
{
    public Camera stickFightCamera;
    public CharacterOpponentStickFight opponentStickFighter;
    public AvatarMessageSender avatarHitRightMessageSender;
    public AvatarMessageSender avatarHiLefttMessageSender;
    public AvatarMessageSender avatarDuckMessageSender;

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

        avatarHitRightMessageSender.Avatar = avatar;
        avatarHiLefttMessageSender.Avatar = avatar;
        avatarDuckMessageSender.Avatar = avatar;
        avatarHitRightMessageSender.gameObject.SetActive(SystemInfo.deviceType == DeviceType.Handheld);
        avatarHiLefttMessageSender.gameObject.SetActive(SystemInfo.deviceType == DeviceType.Handheld);
        avatarDuckMessageSender.gameObject.SetActive(SystemInfo.deviceType == DeviceType.Handheld);

    }

    void CloseStickFightMinigame()
    {
        avatarHitRightMessageSender.gameObject.SetActive(false);
        avatarHiLefttMessageSender.gameObject.SetActive(false);
        avatarDuckMessageSender.gameObject.SetActive(false);

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

