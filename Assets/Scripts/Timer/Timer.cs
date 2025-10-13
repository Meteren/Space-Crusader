using System;

public abstract class Timer 
{
    public event Action onStart;
    public event Action onEnd;

    protected float current;

    protected float startPoint;
    protected Timer(float startPoint) 
    { 
        this.startPoint = startPoint;
        current = startPoint;
        
    }
    public bool isFinished {  get; private set; }

    public float Current { get { return current; } set { current = value; } }

    public abstract void OnTick();

    public void StartTimer() 
    {
        TimeManager.instance.AddTimer(this);
        onStart?.Invoke();
    } 
    public void CancelTimer()
    {
        TimeManager.instance.RemoveTimer(this);
    }

    public void CleanAfterCompletion()
    {
        onEnd?.Invoke();
        isFinished = true;
        TimeManager.instance.RemoveTimer(this);
    }

    public virtual void Reset()
    {
        isFinished = false;
        current = startPoint;
    }


}
