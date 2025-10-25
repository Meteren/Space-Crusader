using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/EventController")]
public class EventController : ScriptableObject
{
    List<Listener> listeners = new List<Listener>();

    public void AddListener(Listener listener) => listeners.Add(listener);

    public void RemoveListener(Listener listener) => listeners.Remove(listener);
    

    public void ExecuteListeners()
    {
        foreach (var listener in listeners)
            listener.ExecuteEvent();
    }


}
