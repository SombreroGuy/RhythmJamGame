using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class AnimationEffector : MonoBehaviour
{
    public int LoopTimes;
    int TimesLooped;
    int CurrentColorIndex;

    public Anims[] anims;
    
    public string ClipName;

    [EventID]
    public string EventID;
    
    [HideInInspector]
    public List<KoreographyEvent> AllEvents;

    void Start()
    {
        Koreographer.Instance.RegisterForEvents(EventID, EffectColor);
        Koreographer.Instance.GetAllEventsInRange(ClipName, EventID, 0, Koreographer.Instance.GetMusicSampleLength(ClipName), AllEvents);
    }

    void EffectColor(KoreographyEvent Event)
    {
        if (this != null)
        {
            if (Event.IsOneOff())
            {                           
                if (CurrentColorIndex >= anims.Length && TimesLooped < LoopTimes)
                {            
                    AllEvents.Clear();
                    
                    Koreographer.Instance.GetAllEventsInRange(ClipName, EventID, Event.StartSample, Koreographer.Instance.GetMusicSampleLength(ClipName), AllEvents);
                
                    CurrentColorIndex = 0;
                
                    TimesLooped++;
                }
                
                if (CurrentColorIndex < anims.Length)
                {
                    anims[CurrentColorIndex].Anim.clip = anims[CurrentColorIndex].Clip;
                    
                    anims[CurrentColorIndex].Anim.Stop();
                    anims[CurrentColorIndex].Anim.Play();
                    
                    for (int i = 0; i < AllEvents.Count; i++)
                    {
                        if (AllEvents[i] == Event)
                        {
                            CurrentColorIndex = i + 1;
                        }
                    }
                }
            }
        }
    }
}
