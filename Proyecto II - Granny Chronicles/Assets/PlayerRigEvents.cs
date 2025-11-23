using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRigEvents : MonoBehaviour
{
    private Player player;
    public ParticleSystem BackflipAttackParticles;
    public ParticleSystem KickParticles;
    public GameObject BackflipAttackObj;
    private bool hasBeenPlayed;

    [Header("ScreenShake Kick")]
    [SerializeField] public CinemachineVirtualCamera virtualCam;
    [HideInInspector] private CinemachineBasicMultiChannelPerlin cameraNoise;
    [SerializeField] public float amplitudeGain;
    [SerializeField] public float frequencyGain;
    [SerializeField] public float time;
    void Start()
    {
        player = FindObjectOfType<Player>();
        cameraNoise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BlockPlayerMovement()
    {
        player.BlockPlayerMovement();
    }

    void StartBackflipParticles()
    {
        if(!hasBeenPlayed)
        {
            BackflipAttackObj.SetActive(true);
            BackflipAttackParticles.Play();
            hasBeenPlayed = true;
        }
    }

    void PlayKickParticles()
    {
        KickParticles.Play();
        Shake();
    }

    void StopBackflipParticles()
    {
        BackflipAttackObj.SetActive(false);
    }

    void KillGrandma()
    {
        //GameObject.Find("Abuela").transform.Find("Abuelita").Find("bone_1").GetComponent<Animator>().Play("Muerte");
        if (!player.grandmaObjects[0])
        {
            GameObject.Find("Abuela").SendMessage("GrandmaDeathAnimation");
        }
        else
        {
            GameObject.Find("AbuelaRevivida").SendMessage("GrandmaDeathAnimation");
        }
        player.stateMachine.ChangeState(player.idleState);
    }

    public void Shake()
    {
        cameraNoise.m_AmplitudeGain = amplitudeGain;
        cameraNoise.m_FrequencyGain = frequencyGain;

        CancelInvoke();
        Invoke("StopShake", time);
    }

    public void StopShake()
    {
        cameraNoise.m_AmplitudeGain = 0;
        cameraNoise.m_FrequencyGain = 0;
    }
}
