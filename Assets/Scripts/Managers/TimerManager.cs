using System.Collections.Generic;
using System.Linq;

public class TimeManager : SingleTon<TimeManager>
{
    private List<Timer> timers = new();

    public void AddTimer(Timer timer) => timers.Add(timer);

    public void RemoveTimer(Timer timer) => timers.Remove(timer);

    private void Update()
    {
        foreach(var timer in timers.ToList())
        {
            timer.OnTick();
            if (timer.Current <= 0)
                timer.CleanAfterCompletion();
           
        }
    }


}
