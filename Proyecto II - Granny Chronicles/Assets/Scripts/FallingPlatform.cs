using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    public SpriteRenderer[] spritePlattform = new SpriteRenderer[0];
    private BoxCollider2D collider;
    public float maxFallSpeed;
    public float timeUntilFalls;
    public float timeUntilDisappears;
    public float timeUntilAppears;
    private bool imFalling;
    private Vector2 initialPosition;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        initialPosition = transform.position;
        StopAllCoroutines();
    }

    void LateUpdate()
    {
        if(Mathf.Abs(rb.velocity.y) > maxFallSpeed)
        {
            rb.velocity = new Vector2(0, -maxFallSpeed);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player" && other.transform.position.y > transform.position.y)
        {
            Invoke("Fall", timeUntilFalls);
        }
    }

    public void Fall()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        StartCoroutine(RespawnPlattform());
    }

    public IEnumerator RespawnPlattform()
    {
        yield return new WaitForSeconds(timeUntilDisappears);
        DisablePlatform();
        collider.enabled = false;
        yield return new WaitForSeconds(timeUntilAppears);
        transform.position = initialPosition;
        EnablePlatform();
        collider.enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void EnablePlatform()
    {
        for(int i = 0; i < spritePlattform.Length; i++)
        {
            spritePlattform[i].enabled = true;
        }
    }

    public void DisablePlatform()
    {
        for (int i = 0; i < spritePlattform.Length; i++)
        {
            spritePlattform[i].enabled = false;
        }
    }
}

