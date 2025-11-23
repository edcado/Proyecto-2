using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NarratorEvent
{
    public bool active = true;
    public string eventName;
    public MonoBehaviour eventReceiver;
    public float timeOffset;
}
