using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStealthEnemy : MonoBehaviour
{
    private Vector2 initialPosition;

    [Header("Movement")]
    [SerializeField] public float rightMovement;
    [SerializeField] public float leftMovement;
    [SerializeField, Range(0,500)] public float speed;

    [Header("Direction")]
    [SerializeField] public bool startRight;

    [Header("Options")]
    [SerializeField] public float turnDelay;

    [Header("Components")]
    [SerializeField] public Rigidbody2D rbEnemy;
    [SerializeField] public Transform rightLimit;
    [SerializeField] public Transform leftLimit;

    private int direction;

    void Start()
    {
        initialPosition = transform.position;
        if(startRight)
        {
            direction = 1;
            transform.eulerAngles = new Vector2(0, 0);
        }
        else
        {
            direction = -1;
            transform.eulerAngles = new Vector2(0, 180);
        }
    }


    void Update()
    {
        if(rightLimit != null)
        {
            if (transform.position.x > initialPosition.x + rightMovement || transform.position.x > rightLimit.position.x)
            {
                StartCoroutine(ChangeDirectionToLeft());
            }
        }
        else
        {
            if (transform.position.x > initialPosition.x + rightMovement)
            {
                StartCoroutine(ChangeDirectionToLeft());
            }
        }

        if (leftLimit != null)
        {
            if (transform.position.x < initialPosition.x - leftMovement || transform.position.x < leftLimit.position.x)
            {
                StartCoroutine(ChangeDirectionToRight());
            }
        }
        else
        {
            if (transform.position.x < initialPosition.x - leftMovement)
            {
                StartCoroutine(ChangeDirectionToRight());
            }
        }
        
    }

    private void FixedUpdate()
    {
        rbEnemy.velocity = new Vector2(direction, 0) * speed * Time.deltaTime;
    }

    public IEnumerator ChangeDirectionToRight()
    {
        direction = 0;
        yield return new WaitForSeconds(turnDelay);
        direction = 1;
        transform.eulerAngles = new Vector2(0, 0);
    }

    public IEnumerator ChangeDirectionToLeft()
    {
        direction = 0;
        yield return new WaitForSeconds(turnDelay);
        direction = -1;
        transform.eulerAngles = new Vector2(0, 180);
    }

}
