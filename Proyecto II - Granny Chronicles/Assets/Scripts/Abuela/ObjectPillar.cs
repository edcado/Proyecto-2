using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPillar : MonoBehaviour
{
    public int index;
    private SpriteRenderer sr;
    public bool isTriggered;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().grandmaObjects[index])
            {
                sr.enabled = true;
                isTriggered = true;
                transform.parent.parent.GetComponent<Ending>().CheckFinale();
            }
        }
    }
}
