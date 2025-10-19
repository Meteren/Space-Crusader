using System;

using UnityEngine;

public abstract class Timer 
{
    public event Action onStart;
    public event Action onEnd;

    protected float current;

    protected float startPoint;

    protected string source;
    protected Timer(float startPoint,string source) 
    { 
        this.startPoint = startPoint;
        this.source = source;
        current = startPoint;
        
    }
    public bool isFinished {  get; private set; }

    public float Current { get { return current; } set { current = value; } }

    public float StartPoint { get => startPoint; }

    public abstract void OnTick();

    public void StartTimer() 
    {
        Debug.Log($"{source} timer started.");
        TimeManager.instance.AddTimer(this);
        onStart?.Invoke();
    }

    public void StopTimer() =>
        TimeManager.instance.RemoveTimer(this);

    
    public void SetTimer(float current)
    {
        this.current = Mathf.Min(current,startPoint);
    }
    public void CancelTimer()
    {
        CleanAfterCompletion();
    }

    public virtual void CleanAfterCompletion()
    {
        Debug.Log($"{source} timer ended.");
        onEnd?.Invoke();
        isFinished = true;
        onStart = null;
        onEnd = null;
        TimeManager.instance.RemoveTimer(this);
    }

    public virtual void Reset()
    {
        isFinished = false;
        current = startPoint;
    }


}
