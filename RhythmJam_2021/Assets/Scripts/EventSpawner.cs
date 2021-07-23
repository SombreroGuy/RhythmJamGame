using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class EventSpawner : MonoBehaviour
{
    public GameObject ObjectToSpawn;  
    GameObject OldEventObject;
    
    public Transform SpawnPoint;
    
    [EventID]
    public string EventID;
    
    void Start()
    {
        Koreographer.Instance.RegisterForEvents(EventID, SpawnObjects);
    }

    void SpawnObjects(KoreographyEvent Event)
    {
        if (ObjectToSpawn == null || SpawnPoint == null)
            return;

        GameObject EventObject = Instantiate(ObjectToSpawn, SpawnPoint.position, SpawnPoint.rotation);
        
        EventObject.name = ObjectToSpawn.name + " " + Event.StartSample;

        if (OldEventObject != null)
        {
            if (EventObject.name == OldEventObject.name)
            {
                Destroy(EventObject);
            
                return;
            }
        }
    
        if (EventObject != null)
        {
            OldEventObject = EventObject;
        }
    }
}
