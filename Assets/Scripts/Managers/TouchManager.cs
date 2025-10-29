using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class TouchManager : SingleTon<TouchManager>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Touch touch {  get; private set; }

    public int activeTouchesCount { get; private set; }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
#if UNITY_EDITOR
        TouchSimulation.Enable();
#endif 
    }     

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
#if UNITY_EDITOR
        TouchSimulation.Disable();
#endif
    }
    // Update is called once per frame
    private void Update()
    {
        var activeTouches = Touch.activeTouches;

        activeTouchesCount = activeTouches.Count;

        if (activeTouchesCount > 0)
        {
            touch = Touch.activeTouches[0];

        }

    }

    public bool TryGetTouchById(int fingerID, out Touch touch)
    {
        foreach(var t in Touch.activeTouches)
        {
            if(t.finger.index == fingerID)
            {
                touch = t;
                return true;
            }
        }
        touch = default;
        return false;
    }
}
