using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalNotRespawn : MonoBehaviour
{
    // Start is called before the first frame update
    public int index;
    private Player player;
    void Start()
    {
        player = FindObjectOfType<Player>();
        if (player.grandmaObjects[index])
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
