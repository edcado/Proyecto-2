using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OnOffEnemy : MonoBehaviour
{
    [Header("Time On and Off")]
    [SerializeField] public float timeOff;
    [SerializeField] public float timeOn;
    [SerializeField] public float beginDelay;

    [Header("Components")]

    public ParticleSystem warningParticles;
    public GameObject warningObj;
    public SpriteRenderer spriteLantern;
    public PolygonCollider2D colliderLantern;
    public Light2D light2D;

    private float timeActiveOff = 0;
    private float timeActiveOn = 0;
    private bool isOn = true;
    private bool isOff;
    private float timeDelayCounter;


    // Update is called once per frame
    void Update()
    {
        timeDelayCounter += Time.deltaTime;
        if(timeDelayCounter > beginDelay)
        {

        if (isOff)
        {
            timeActiveOff += Time.deltaTime;
            warningObj.SetActive(true);
            warningParticles.Play();
            if(timeActiveOff >= timeOff)
            {
                isOn = true;
                isOff = false;
                spriteLantern.enabled = true;
                colliderLantern.enabled = true;
                light2D.enabled = true;
                timeActiveOff = 0;
            }
        }
        else if(isOn)
        {
            timeActiveOn += Time.deltaTime;
            warningParticles.Stop();
            warningObj.SetActive(false);
            if( timeActiveOn >= timeOn)
            {
                isOn = false;
                isOff = true;
                spriteLantern.enabled = false;
                colliderLantern.enabled = false;
                light2D.enabled = false;
                timeActiveOn = 0;
            }
        }
        }
    }


   
}
