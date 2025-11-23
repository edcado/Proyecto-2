using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    private GameObject cam;

    [SerializeField] private float parallaxEffectX;
    [SerializeField] private float parallaxEffectY;

    private float xPosition;
    private float yPosition;

    void Start()
    {
        cam = GameObject.Find("Main Camera");

        xPosition = transform.position.x;
        yPosition = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //float distanceToMoveX = cam.transform.position.x * parallaxEffectX;
        //float distanceToMoveY = cam.transform.position.y * parallaxEffectY;

        float distanceToMoveX = cam.transform.position.x * parallaxEffectX;
        float distanceToMoveY = cam.transform.position.y * parallaxEffectY;

        transform.position = new Vector3(xPosition + distanceToMoveX, yPosition + distanceToMoveY);
    }
}
