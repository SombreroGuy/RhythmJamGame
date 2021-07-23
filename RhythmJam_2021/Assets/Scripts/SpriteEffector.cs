using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class SpriteEffector : MonoBehaviour
{
    public int LoopTimes;
    int TimesLooped;
    int CurrentColorIndex;

    public Colors[] colors;
    Color CurrentColor;

    [EventID]
    public string EventID;
    
    KoreographyEvent CurrentEvent;

    void Start()
    {
        Koreographer.Instance.RegisterForEventsWithTime(EventID, EffectCurveColor);
        Koreographer.Instance.RegisterForEvents(EventID, EffectColor);
    
        CurrentColor = colors[CurrentColorIndex].ObjectToEffect.color;
    }

    void EffectColor(KoreographyEvent Event)
    {
        if (this != null)
        {
            if (Event.IsOneOff())
            {                           
                if (CurrentColorIndex >= colors.Length && TimesLooped < LoopTimes)
                {            
                    CurrentColorIndex = 0;
                
                    TimesLooped++;
                }                
                if (CurrentColorIndex < colors.Length)
                {
                    colors[CurrentColorIndex].ObjectToEffect.color = colors[CurrentColorIndex].EndColor;
                
                    CurrentColor = colors[CurrentColorIndex].EndColor;
                    
                    CurrentColorIndex++;
                }
            }
        }
    }

    void EffectCurveColor(KoreographyEvent Event, int SampleTime, int SampleDelta, DeltaSlice Delta)
    {
        if (this != null)
        {
            if (Event.HasCurvePayload())
            {
                float CurveValue = Event.GetValueOfCurveAtTime(SampleTime);
                
                if (CurrentColorIndex >= colors.Length && TimesLooped < LoopTimes)
                {            
                    CurrentColorIndex = 0;

                    TimesLooped++;
                }            
                if (CurrentColorIndex < colors.Length)
                {
                    float ColorCurve = colors[CurrentColorIndex].EffectCurve.Evaluate(CurveValue);
                    
                    colors[CurrentColorIndex].ObjectToEffect.color = Color.Lerp(CurrentColor, colors[CurrentColorIndex].EndColor, ColorCurve);
                }
            
                if (CurveValue >= 1f && CurrentEvent != Event)
                {           
                    CurrentColor = Color.Lerp(CurrentColor, colors[CurrentColorIndex].EndColor, colors[CurrentColorIndex].EffectCurve.keys[colors[CurrentColorIndex].EffectCurve.length - 1].value);

                    CurrentColorIndex++;
                
                    CurrentEvent = Event;
                }
            }
        }
    }
}
