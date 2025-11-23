using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbuelaHablando : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            AudioManager.instance.PlaySFX(3,null);
        }
    }
}
