using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ending : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    [SerializeField] private GameObject[] endingTriggers;
    [SerializeField] private GameObject grandma;
    [SerializeField] private GameObject goodEndingTrigger;
    [SerializeField] private GameObject blockTrigger;

    [TextArea(3, 5)]public string endingText;
    private TMP_Text textArea;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        
        textArea = GetComponentInChildren<TMP_Text>();
        player = FindObjectOfType<Player>();
        int objectNumber = 0;
        foreach (bool b in player.grandmaObjects)
        {
            if (b)
            {
                objectNumber++;
            }
        }
        for (int i = 0; i < endingTriggers.Length; i++)
        {
            endingTriggers[i].SetActive(i == objectNumber);
        }
    }

    public void CheckFinale()
    {
        bool b = true;
        foreach (var obj in objects)
        {
            if (!obj.GetComponent<ObjectPillar>().isTriggered)
            {
                b = false;
            }
        }
        
        if (b)
        {
            foreach(var obj in endingTriggers)
            {
                obj.SetActive(false);
            }
            //Launch Finale
            Debug.Log("I finished the game Mwahahahaha");

            textArea.text = endingText;

            Invoke("ReviveGrandma", 3.0f);
            //Invoke("CloseGame", 3.0f);
        }
    }

    private void ReviveGrandma()
    {
        player.grandmaNear = false;
        grandma.SetActive(true);
        goodEndingTrigger.SetActive(true);
        blockTrigger.SetActive(false);
        //Spawn Grandma, preferably with a particle system
        //Open path at the right
    }
    private void CloseGame()
    {
        Application.Quit();
        Debug.Log("Me he cerrado");
    }

    private void BadEnding()
    {
        //When Grandma is killed again
        //Bad Ending Dialogue
        //Closes the game
        Debug.Log("Bad Ending");
        CloseGame();
    }

    private void GoodEnding()
    {
        //When player goes to the right
        //Good Ending Dialogue
        //Closes the game
        Debug.Log("Good Ending");
        CloseGame();
    }
}
