using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReminder : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject Button;
    [HideInInspector]public Player player;

    [Header("ExtraChanges")]
    public bool iAmBush;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (player.canKillGrandma && player.grandmaNear && !iAmBush)
        {
            Button.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && iAmBush)
        {
            Button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Button.SetActive(false);
        }
    }

}
