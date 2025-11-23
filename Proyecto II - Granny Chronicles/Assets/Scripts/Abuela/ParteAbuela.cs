using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParteAbuela : MonoBehaviour
{
    public int index;

    private float amplitude = .3f;
    private float speed = 2f;
    private Vector2 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }
    private void Update()
    {
        transform.position = startPosition + new Vector2(0, amplitude * Mathf.Cos(speed * Time.time));
    }
}
