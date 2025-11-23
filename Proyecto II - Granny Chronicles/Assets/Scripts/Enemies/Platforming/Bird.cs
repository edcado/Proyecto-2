using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float spawnTime;
    [SerializeField]private GameObject egg;


    void Start()
    {

        InvokeRepeating("SpawnEgg", 0, spawnTime);
    }

    private void OnBecameInvisible()
    {
        //CancelInvoke(); 
    }
    public void SpawnEgg()
    {
        Instantiate(egg, transform.position, Quaternion.identity);
        AudioManager.instance.PlaySFX(7, gameObject.transform);
    }
}
