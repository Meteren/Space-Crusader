using UnityEngine;

public class Listener : MonoBehaviour
{

    public EventController eventController;

    public delegate void Event();

    public event Event events;

    public void AddEvent(Event eventToAdd) => events += eventToAdd;

    public void RemoveEvent(Event eventToRemove) => events -= eventToRemove;


    private void AddListener() => eventController.AddListener(this);

    private void RemoveListener() => eventController.RemoveListener(this);

    public void ExecuteEvent() => events?.Invoke();


    private void OnEnable() => AddListener();

    private void OnDisable() => RemoveListener();


}
