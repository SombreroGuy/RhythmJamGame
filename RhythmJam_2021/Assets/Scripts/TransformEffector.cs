using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SonicBloom.Koreo;

public class TransformEffector : MonoBehaviour
{
    public int LoopTimes;
    int TimesLooped = 0;
    int CurrentTransformsIndex = 0;   
    int CurrentEventIndex = 0;

    [Space(10)]
    public Transforms[] transforms;

    Quaternion OldRotation;
    
    Vector3 CurrentPosition;
    Vector3 CurrentRotation;
    Vector3 CurrentScale;
    Vector3 OldPosition;
    Vector3 OldScale;
    
    [EventID]
    public string EventID;

    [Space(10)]
    public Mesh PreviewMesh;

    KoreographyEvent CurrentEvent;

    void Start()
    {
        Koreographer.Instance.RegisterForEventsWithTime(EventID, EffectCruveTransform);
        Koreographer.Instance.RegisterForEvents(EventID, EffectTransform);    
    
        CurrentPosition = transforms[CurrentTransformsIndex].ObjectToEffect.localPosition;
        CurrentRotation = transforms[CurrentTransformsIndex].ObjectToEffect.eulerAngles;
        CurrentScale = transforms[CurrentTransformsIndex].ObjectToEffect.localScale;
    }

    void EffectTransform(KoreographyEvent Event)
    {
        if (this != null)
        {
            if (Event.IsOneOff())
            {                           
                if (CurrentTransformsIndex >= transforms.Length && TimesLooped < LoopTimes)
                {            
                    CurrentTransformsIndex = 0;
                
                    TimesLooped++;
                }   
                if (CurrentTransformsIndex < transforms.Length)
                {            
                    Vector3 PositionCurve = new Vector3(transforms[CurrentTransformsIndex].PositionCurveX.keys[transforms[CurrentTransformsIndex].PositionCurveX.length - 1].value, transforms[CurrentTransformsIndex].PositionCurveY.keys[transforms[CurrentTransformsIndex].PositionCurveY.length - 1].value, transforms[CurrentTransformsIndex].PositionCurveZ.keys[transforms[CurrentTransformsIndex].PositionCurveZ.length - 1].value);
                    Vector3 RotationCurve = new Vector3(transforms[CurrentTransformsIndex].RotationCurveX.keys[transforms[CurrentTransformsIndex].RotationCurveX.length - 1].value, transforms[CurrentTransformsIndex].RotationCurveY.keys[transforms[CurrentTransformsIndex].RotationCurveY.length - 1].value, transforms[CurrentTransformsIndex].RotationCurveZ.keys[transforms[CurrentTransformsIndex].RotationCurveZ.length - 1].value);
                    Vector3 ScaleCurve = new Vector3(transforms[CurrentTransformsIndex].ScaleCurveX.keys[transforms[CurrentTransformsIndex].ScaleCurveX.length - 1].value, transforms[CurrentTransformsIndex].ScaleCurveY.keys[transforms[CurrentTransformsIndex].ScaleCurveY.length - 1].value, transforms[CurrentTransformsIndex].ScaleCurveZ.keys[transforms[CurrentTransformsIndex].ScaleCurveZ.length - 1].value);

                    transforms[CurrentTransformsIndex].ObjectToEffect.localPosition = NewCurrentPosition(CurrentTransformsIndex, CurrentPosition) + PositionCurve;    
                    transforms[CurrentTransformsIndex].ObjectToEffect.eulerAngles = NewCurrentRotation(CurrentTransformsIndex, CurrentRotation) + RotationCurve; 
                    transforms[CurrentTransformsIndex].ObjectToEffect.localScale = NewCurrentScale(CurrentTransformsIndex, CurrentScale) + ScaleCurve;  
                
                    CurrentPosition = transforms[CurrentTransformsIndex].ObjectToEffect.localPosition;
                    CurrentRotation = transforms[CurrentTransformsIndex].ObjectToEffect.eulerAngles;
                    CurrentScale = transforms[CurrentTransformsIndex].ObjectToEffect.localScale;
                    
                    CurrentTransformsIndex++;
                }
            }            
        }
    }

    void EffectCruveTransform(KoreographyEvent Event, int SampleTime, int SampleDelta, DeltaSlice Delta)
    {
        if (this != null)
        {        
            if (Event.HasCurvePayload())
            {
                float CurveValue = Event.GetValueOfCurveAtTime(SampleTime);
                
                if (CurrentTransformsIndex >= transforms.Length && TimesLooped < LoopTimes)
                {            
                    CurrentTransformsIndex = 0;
                        
                    TimesLooped++;
                } 
                if (CurrentTransformsIndex < transforms.Length)
                {            
                    Vector3 PositionCurve = new Vector3(transforms[CurrentTransformsIndex].PositionCurveX.Evaluate(CurveValue), transforms[CurrentTransformsIndex].PositionCurveY.Evaluate(CurveValue), transforms[CurrentTransformsIndex].PositionCurveZ.Evaluate(CurveValue));
                    Vector3 RotationCurve = new Vector3(transforms[CurrentTransformsIndex].RotationCurveX.Evaluate(CurveValue), transforms[CurrentTransformsIndex].RotationCurveY.Evaluate(CurveValue), transforms[CurrentTransformsIndex].RotationCurveZ.Evaluate(CurveValue));
                    Vector3 ScaleCurve = new Vector3(transforms[CurrentTransformsIndex].ScaleCurveX.Evaluate(CurveValue), transforms[CurrentTransformsIndex].ScaleCurveY.Evaluate(CurveValue), transforms[CurrentTransformsIndex].ScaleCurveZ.Evaluate(CurveValue));

                    transforms[CurrentTransformsIndex].ObjectToEffect.localPosition = NewCurrentPosition(CurrentTransformsIndex, CurrentPosition) + PositionCurve;    
                    transforms[CurrentTransformsIndex].ObjectToEffect.eulerAngles = NewCurrentRotation(CurrentTransformsIndex, CurrentRotation) + RotationCurve; 
                    transforms[CurrentTransformsIndex].ObjectToEffect.localScale = NewCurrentScale(CurrentTransformsIndex, CurrentScale) + ScaleCurve;  
                
                    if (CurrentEventIndex < transforms[CurrentTransformsIndex].TimedEvents.Length)
                    {
                        if (CurveValue >= transforms[CurrentTransformsIndex].TimedEvents[CurrentEventIndex].CallTime)
                        {
                            transforms[CurrentTransformsIndex].TimedEvents[CurrentEventIndex].Event.Invoke();

                            CurrentEventIndex++;
                        }
                    }

                    if (CurveValue >= 1 && CurrentEvent != Event)
                    {                                
                        CurrentPosition = transforms[CurrentTransformsIndex].ObjectToEffect.localPosition;
                        CurrentRotation = transforms[CurrentTransformsIndex].ObjectToEffect.eulerAngles;
                        CurrentScale = transforms[CurrentTransformsIndex].ObjectToEffect.localScale;
                        
                        CurrentTransformsIndex++;
                        CurrentEventIndex = 0;
                    
                        CurrentEvent = Event;
                    }
                }
            }
        }   
    }

    Vector3 NewCurrentPosition(int Index, Vector3 OldCurrentPosition)
    {
        Vector3 NewCurrentPosition = Vector3.zero;
        
        if (transforms[Index].SetPositionX == false)
        { 
            NewCurrentPosition += new Vector3(OldCurrentPosition.x, 0f, 0f);
        }
        if (transforms[Index].SetPositionY == false)
        { 
            NewCurrentPosition += new Vector3(0f, OldCurrentPosition.y, 0f);
        }
        if (transforms[Index].SetPositionZ == false)
        { 
            NewCurrentPosition += new Vector3(0f, 0f, OldCurrentPosition.z);
        }
    
        return NewCurrentPosition;
    }
    Vector3 NewCurrentRotation(int Index, Vector3 OldCurrentRotation)
    {      
        Vector3 NewCurrentRotation = Vector3.zero;
        
        if (transforms[Index].SetRotationX == false)
        { 
            NewCurrentRotation += new Vector3(OldCurrentRotation.x, 0f, 0f);
        }
        if (transforms[Index].SetRotationY == false)
        { 
            NewCurrentRotation += new Vector3(0f, OldCurrentRotation.y, 0f);
        }
        if (transforms[Index].SetRotationZ == false)
        { 
            NewCurrentRotation += new Vector3(0f, 0f, OldCurrentRotation.z);
        }
    
        return NewCurrentRotation;
    }
    
    Vector3 NewCurrentScale(int Index, Vector3 OldCurrentScale)
    {
        Vector3 NewCurrentScale = Vector3.zero;

        if (transforms[Index].SetScaleX == false)
        { 
            NewCurrentScale += new Vector3(OldCurrentScale.x, 0f, 0f);
        }
        if (transforms[Index].SetScaleY == false)
        { 
            NewCurrentScale += new Vector3(0f, OldCurrentScale.y, 0f);
        }
        if (transforms[Index].SetScaleZ == false)
        { 
            NewCurrentScale += new Vector3(0f, 0f, OldCurrentScale.z);
        }
    
        return NewCurrentScale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.25f, 0.25f, 0.1f);
        
        if (!Application.isPlaying)
        {
            for (int i = 0; i < transforms.Length; i++)
            {
                for (float j = 0; j < 1; j += 0.1f)
                {
                    Vector3 EndPosition = new Vector3(transforms[i].PositionCurveX.keys[transforms[i].PositionCurveX.length - 1].value, transforms[i].PositionCurveY.keys[transforms[i].PositionCurveY.length - 1].value, transforms[i].PositionCurveZ.keys[transforms[i].PositionCurveZ.length - 1].value);
                    Vector3 EndVectorRotation = new Vector3(transforms[i].RotationCurveX.keys[transforms[i].RotationCurveX.length - 1].value, transforms[i].RotationCurveY.keys[transforms[i].RotationCurveY.length - 1].value, transforms[i].RotationCurveZ.keys[transforms[i].RotationCurveZ.length - 1].value);
                    Vector3 EndScale = new Vector3(transforms[i].ScaleCurveX.keys[transforms[i].ScaleCurveX.length - 1].value, transforms[i].ScaleCurveY.keys[transforms[i].ScaleCurveY.length - 1].value, transforms[i].ScaleCurveZ.keys[transforms[i].ScaleCurveZ.length - 1].value);

                    Vector3 PositionCurve = new Vector3(transforms[i].PositionCurveX.Evaluate(j), transforms[i].PositionCurveY.Evaluate(j), transforms[i].PositionCurveZ.Evaluate(j));
                    Vector3 RotationCurve = new Vector3(transforms[i].RotationCurveX.Evaluate(j), transforms[i].RotationCurveY.Evaluate(j), transforms[i].RotationCurveZ.Evaluate(j));
                    Vector3 ScaleCurve = new Vector3(transforms[i].ScaleCurveX.Evaluate(j), transforms[i].ScaleCurveY.Evaluate(j), transforms[i].ScaleCurveZ.Evaluate(j));

                    Quaternion EndRotation = Quaternion.Euler(RotationCurve.x, RotationCurve.y, RotationCurve.z);

                    if (i == 0)
                    {
                        Gizmos.DrawWireMesh(PreviewMesh, NewCurrentPosition(i, transform.position) + PositionCurve, EndRotation, NewCurrentScale(i, transform.localScale) + ScaleCurve);
                    
                        if (j >= 0.9)
                        {
                            OldPosition = NewCurrentPosition(i, transform.position) + EndPosition;
                            OldScale = NewCurrentScale(i, transform.localScale) + EndScale;
                        }
                    }
                    else
                    {
                        Gizmos.DrawWireMesh(PreviewMesh, NewCurrentPosition(i, OldPosition) + PositionCurve, EndRotation, NewCurrentScale(i, OldScale) + ScaleCurve);
                    
                        if (j >= 0.9)
                        {
                            OldPosition = NewCurrentPosition(i, OldPosition) + EndPosition;
                            OldScale = NewCurrentScale(i, OldScale) + EndPosition;
                        }
                    }
                }
            }
        }
    }
}
