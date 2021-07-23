using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TimedEvent
{
    [Range(0f, 1f)]
    public float CallTime; 

    [Space(10f)]  
    public UnityEvent Event;
}
