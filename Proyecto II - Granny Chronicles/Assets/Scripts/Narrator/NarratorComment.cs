using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NarratorComment
{
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;
    public AudioClip[] clips;
    public NarratorEvent[] events;

    [Tooltip("¿Can the dialogue be reproduced infinitely or only once?")] 
    public bool isPermanent;
}
