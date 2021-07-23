using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Colors
{
    [Header("Object Settings")]
    public SpriteRenderer ObjectToEffect;
    
    [Header("Color Settings")]
    public Color EndColor;
    
    [Header("Curve Settings")]
    public AnimationCurve EffectCurve;
}
