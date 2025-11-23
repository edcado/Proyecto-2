using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSingleton : MonoBehaviour
{

    public static CameraSingleton Instance { get; private set; }
    public static ICinemachineCamera Camera { get; private set; }
    private void Awake()
    {
        //Singleton
        if (Camera != null && Camera != GetComponent<CinemachineBrain>().ActiveVirtualCamera)
        {
            Destroy(this);
        }
        else
        {
            Camera = GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        }
        DontDestroyOnLoad(GameObject.Find("Virtual Camera"));

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        //
    }
}
