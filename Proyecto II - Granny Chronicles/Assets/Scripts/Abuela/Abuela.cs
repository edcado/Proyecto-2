using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abuela : MonoBehaviour
{
    public GameObject player;
    public Animator abuelaAnim;
    public SpriteRenderer[] GrandmaSprites;
    public bool isRevived;

    private void Start()
    {
        player = GameObject.Find("Player");
        if ((player.GetComponent<Player>().grandmaObjects[0] || player.GetComponent<Player>().grandmaObjects[1]) && !isRevived)
        {
            Destroy(this.gameObject);
        }
    }



    void Update()
    {
        
    }

    void Die()
    {
        transform.parent.parent.GetComponent<NarratorTrigger>().TriggerComment();
        Destroy(transform.parent.parent.gameObject);
    }

    public void GrandmaDeathAnimation()
    {
        abuelaAnim.Play("Muerte");
    }

    void FeedbackDeath()
    {
        StartCoroutine(GrandmaBlinkRed());
    }

    public IEnumerator GrandmaBlinkRed()
    {
        bool grandmaDead = false;
        if (!grandmaDead)
        {
            AudioManager.instance.StopSFX(3);
            AudioManager.instance.PlaySFX(4, null);
            grandmaDead = true;
        }
        
        Color[] defaultcolor = new Color[GrandmaSprites.Length];
        for (int i = 0;i < GrandmaSprites.Length; i++)
        {
            defaultcolor[i] = GrandmaSprites[i].color;
            GrandmaSprites[i].color = Color.red;
        }
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < GrandmaSprites.Length; i++)
        {
            GrandmaSprites[i].color = defaultcolor[i];
        }
    }
}
