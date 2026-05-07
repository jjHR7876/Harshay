namespace ConsoleApp1;

using System;
using System.Timers;

public class GameTimer 
{
    private static System.Timers.Timer _minuteTimer;
    private static int _minutesElapsed = 0;

    public static void StartTimer()
    {
       
        _minuteTimer = new System.Timers.Timer(300000);
        
        _minuteTimer.Elapsed += OnMinuteEvent;
        
        _minuteTimer.AutoReset = true;
        _minuteTimer.Enabled = true;
        
    }

    private static void OnMinuteEvent(Object source, ElapsedEventArgs e)
    {
        _minutesElapsed++;
        if (_minutesElapsed == 1)
        {
            
            Console.WriteLine($"\n[SYSTEM]: {_minutesElapsed} minute(s) have passed!");
            
           
            _minutesElapsed = 0;
        }
    }
}
