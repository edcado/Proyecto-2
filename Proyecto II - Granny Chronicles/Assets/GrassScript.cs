using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassScript : MonoBehaviour
{
    public Animator grassAnim;
    public string grassAnimation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            grassAnim.Play(grassAnimation);
        }
    }

}
