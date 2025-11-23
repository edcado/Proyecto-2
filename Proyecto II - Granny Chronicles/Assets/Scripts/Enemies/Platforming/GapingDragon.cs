using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GapingDragon : MonoBehaviour
{
    private GameObject fire;

    public float fireTime;
    public float waitTime;
    private float timer;
    public bool isFiring;
    public GameObject warningFireObject;
    private ParticleSystem.EmissionModule fireParticlesEmission;
    public ParticleSystem fireParticles;
    private ParticleSystem warningFireParticles;

    [Header("ScreenShake Fire")]

    [HideInInspector] private CinemachineVirtualCamera virtualCam;
    [HideInInspector] private CinemachineBasicMultiChannelPerlin cameraNoise;
    [SerializeField] public float amplitudeGain;
    [SerializeField] public float frequencyGain;
    [SerializeField] public float time;
    [SerializeField] private bool inCamera;
    void Start()
    {
        virtualCam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        cameraNoise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        warningFireParticles = warningFireObject.GetComponent<ParticleSystem>();
        fireParticlesEmission = fireParticles.emission;
        warningFireObject.SetActive(true);
        fire = transform.Find("DragonFire").gameObject;
        fire.SetActive(false);
        timer = waitTime;
        isFiring = false;
        time = fireTime;
    }

    void Update()
    {

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            
            if (isFiring)
            {
                AudioManager.instance.PlaySFX(8, gameObject.transform);
                SwitchToWait();
            }
            else
            {
                AudioManager.instance.StopSFX(8);
                SwitchToFire();
            }
        }
        
    }

    private void OnBecameVisible()
    {
        Debug.Log("Aparezco");
        inCamera = true;
    }

    private void OnBecameInvisible()
    {
        Debug.Log("No estoy :C");
        inCamera = false;
    }

    void SwitchToFire()
    {
        timer = fireTime;
        isFiring = true;
        fire.SetActive(true);
        warningFireObject.SetActive(false);
        warningFireParticles.Stop();
        fireParticlesEmission.enabled = true;

        if (inCamera)
        {
            Shake();
        }
    }
    void SwitchToWait()
    {
        timer = waitTime;
        isFiring = false;
        fire.SetActive(false);
        warningFireParticles.Play();
        warningFireObject.SetActive(true);
        fireParticlesEmission.enabled = false;


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
