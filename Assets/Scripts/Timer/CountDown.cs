using UnityEngine;

public class CountDown : Timer
{
    public CountDown(float startPoint) : base(startPoint)
    {
    }

    public override void OnTick()
    {
        Debug.Log($"Current time:{current}");
        current -= Time.deltaTime;

    }
}
