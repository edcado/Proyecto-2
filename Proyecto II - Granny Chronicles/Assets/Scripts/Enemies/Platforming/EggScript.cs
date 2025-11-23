using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour
{
    public float V_0;
    public float TimeActive;
    public float Gravity;
    public ParticleSystem CrackParticles;
    public SpriteRenderer eggSprite;
    public Rigidbody2D eggRb;
    public CapsuleCollider2D eggCollider;


    public float PosicionMRUA()
    {
        float result = transform.position.y + V_0 * TimeActive + ((Gravity * Mathf.Pow(TimeActive, 2) / 2));
        return result;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Floor")
        {
            eggCollider.enabled = false;
            eggRb.constraints = RigidbodyConstraints2D.FreezeAll;
            CrackParticles.Play();
            StartCoroutine(DestroyDelay());
            AudioManager.instance.PlaySFX(6, gameObject.transform);
        }
    }

    public IEnumerator DestroyDelay()
    {
        eggSprite.enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
