using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptFondo_Temporal : MonoBehaviour
{
    public bool StealthZone;
    public SpriteRenderer fondo;
    public GameObject camera;

    private void Start()
    {
        camera = GameObject.Find("Main Camera");
    }

    private void FixedUpdate()
    {
        transform.position = camera.transform.position + new Vector3(0,0,10f);
    }
    
}
