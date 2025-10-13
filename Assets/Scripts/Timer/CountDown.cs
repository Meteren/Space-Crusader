using UnityEngine;

public class CountDown : Timer
{
    public CountDown(float startPoint,string source) : base(startPoint,source)
    {
    }

    public override void OnTick()
    {
        Debug.Log($"Current time:{current}");
        current -= Time.deltaTime;

    }
}
