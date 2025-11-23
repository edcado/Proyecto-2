using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorTrigger : MonoBehaviour
{
    public NarratorComment comment;

    public bool comentado;
    void Start()
    {
        if (comment.sentences.Length != comment.clips.Length) 
        {
            Debug.LogError("NarratorComments sentences and clips have to be the same length!");
        }
    }
    public void TriggerComment()
    {
        if (comentado == false)
        {
            FindObjectOfType<NarratorManager>().StartComment(comment);
            comentado = true;
        }
    }









}
