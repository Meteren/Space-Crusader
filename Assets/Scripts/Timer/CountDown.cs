using UnityEngine;

public class CountDown : Timer
{
    public CountDown(float startPoint,string source) : base(startPoint,source)
    {
    }

    public override void OnTick()
    {
        current -= Time.deltaTime;

    }
}
