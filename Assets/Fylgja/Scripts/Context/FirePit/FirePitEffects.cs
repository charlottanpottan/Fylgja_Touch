using UnityEngine;
using System.Collections;

public class FirePitEffects : MonoBehaviour
{
    public AudioClip normalFlame;
    public AudioClip fullFlame;

    public ParticleSystem fire;
    public GameObject bigFan;
    public GameObject smallFan;
    public GameObject fanFailed;
    public GameObject fireDied;
    public GameObject lostFullFlame;
    public GameObject ignite;

    ParticleSystem bigFanParticleSystem;
    ParticleSystem smallFanParticleSystem;
    ParticleSystem fanFailedParticleSystem;
    ParticleSystem fireDiedParticleSystem;
    ParticleSystem lostFullFlameParticleSystem;
    ParticleSystem igniteParticleSystem;


    public Transform effectTransform;

    private AnimationState fireState;

    void Start()
    {
        bigFanParticleSystem = bigFan.transform.GetChild(0).GetComponent<ParticleSystem>();
        smallFanParticleSystem = smallFan.transform.GetChild(0).GetComponent<ParticleSystem>();
        fanFailedParticleSystem = fanFailed.transform.GetChild(0).GetComponent<ParticleSystem>();
        fireDiedParticleSystem = fireDied.transform.GetChild(0).GetComponent<ParticleSystem>();
        lostFullFlameParticleSystem = lostFullFlame.transform.GetChild(0).GetComponent<ParticleSystem>();
        igniteParticleSystem = ignite.transform.GetChild(0).GetComponent<ParticleSystem>();

        GetComponent<AudioSource>().loop = true;
    }

    void Update()
    {
    }

    public void SetScale(ParticleSystem particleSystem, float scale)
    {
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particleSystems.Length; i++)
        {
            ParticleSystem.MainModule mainModule = particleSystems[i].main;
            mainModule.scalingMode = ParticleSystemScalingMode.Hierarchy;
        }
        particleSystem.transform.localScale = new Vector3(scale, scale, scale);
    }

    void OnFirePitStrength(float strength)
    {
        SetScale(fire, strength);
    }

    void TriggerEffect(ParticleSystem ps)
    {
        Debug.Log("Effect: " + ps.name);
        ParticleSystem instantiatedParticleSystem = Instantiate(ps, effectTransform.position, effectTransform.rotation);
        StartCoroutine(DestroyParticleSystemAfterBeingPlayed(instantiatedParticleSystem));
    }

    IEnumerator DestroyParticleSystemAfterBeingPlayed(ParticleSystem ps)
    {
        ParticleSystem.MainModule mainModule = ps.main;
        yield return new WaitForSeconds(mainModule.duration);
        Destroy(ps.gameObject);
    }

    void OnFirePitFanFailed()
    {
        TriggerEffect(fanFailedParticleSystem);
    }

    void OnFirePitBigFan()
    {
        TriggerEffect(bigFanParticleSystem);
    }

    void OnFirePitSmallFan()
    {
        TriggerEffect(smallFanParticleSystem);
    }

    void OnFirePitDied()
    {
        GetComponent<AudioSource>().Stop();
        TriggerEffect(fireDiedParticleSystem);
    }

    void OnFirePitIgnited()
    {
        Debug.Log("Effect: ignite flame");

        TriggerEffect(igniteParticleSystem);

        GetComponent<AudioSource>().clip = normalFlame;
        GetComponent<AudioSource>().Play();
    }

    void OnFirePitFullFlame()
    {
        Debug.Log("Effect: full flame");
        GetComponent<AudioSource>().clip = fullFlame;
        GetComponent<AudioSource>().Play();
    }

    void OnFirePitLostFullFlame()
    {
        Debug.Log("Effect: lost full flame");
        TriggerEffect(lostFullFlameParticleSystem);
    }

    void ResetEffects()
    {
        //AnimationState state = GetComponent<Animation>()[fire.name];
        //state.normalizedTime = 0;
        fire.transform.localScale = Vector3.zero;

        GetComponent<AudioSource>().Stop();
    }
}
