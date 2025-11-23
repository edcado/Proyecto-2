using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake_Prueba : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;
    private CinemachineBasicMultiChannelPerlin cameraNoise;
    public ParticleSystem jumpAndFallRight;
    public ParticleSystem jumpAndFallLeft;
    public float amplitudeGain;
    public float frequencyGain;
    public float time;
    void Start()
    {
        cameraNoise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            Shake();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            jumpAndFallLeft.Play();
            jumpAndFallRight.Play();
        }
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
