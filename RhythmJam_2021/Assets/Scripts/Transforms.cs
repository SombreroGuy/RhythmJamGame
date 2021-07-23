using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Transforms
{ 
    [Header("Object Settings")]
    public Transform ObjectToEffect;
    
    [Header("Position Settings")]
    public AnimationCurve PositionCurveX;
    public AnimationCurve PositionCurveY;
    public AnimationCurve PositionCurveZ;
    
    [Space(10f)]
    public bool SetPositionX;
    public bool SetPositionY;
    public bool SetPositionZ;
    
    [Header("Rotation Settings")]
    public AnimationCurve RotationCurveX;
    public AnimationCurve RotationCurveY;
    public AnimationCurve RotationCurveZ;
    
    [Space(10f)]
    public bool SetRotationX;
    public bool SetRotationY;
    public bool SetRotationZ;
    
    [Header("Scale Settings")]
    public AnimationCurve ScaleCurveX;
    public AnimationCurve ScaleCurveY;
    public AnimationCurve ScaleCurveZ;
    
    [Space(10f)]
    public bool SetScaleX;
    public bool SetScaleY;
    public bool SetScaleZ;

    [Header("Event Settings")]  
    public TimedEvent[] TimedEvents;
}
