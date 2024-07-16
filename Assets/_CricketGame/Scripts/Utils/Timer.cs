using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic Timer class for managing countdown or count-up timers.
/// </summary>
public class Timer
{
    private float duration;
    private bool isCountDown;
    private bool isRunning;
    private float elapsedTime;
    private Action onTimerStart;
    private Action onTimerEnd;
    private Action<float> onTimerTick;

    public bool IsRunning => isRunning;

    public bool TimeRemaining => isCountDown ? elapsedTime > 0 : elapsedTime < duration;

    /// <summary>
    /// Creates a new Timer instance.
    /// </summary>
    /// <param name="duration">The duration of the timer in seconds.</param>
    /// <param name="countUp">Whether the timer should count up instead of counting down.</param>
    /// <param name="onStart">The action to be invoked when the timer starts.</param>
    /// <param name="onEnd">The action to be invoked when the timer ends.</param>
    public Timer(float duration, bool countdown, Action onStart, Action onEnd, Action<float> onTimerTick)
    {
        this.duration = duration;
        this.isCountDown = countdown;
        this.onTimerStart = onStart;
        this.onTimerEnd = onEnd;
        this.onTimerTick = onTimerTick;
    }

    /// <summary>
    /// Starts the timer.
    /// </summary>
    public void Start()
    {
        if (isRunning) return;

        isRunning = true;
        elapsedTime = isCountDown ? duration : 0;

        onTimerStart?.Invoke();

        Update();
    }



    /// <summary>
    /// Updates the timer.
    /// </summary>
    public void Update()
    {
        if (!isRunning) return;

        if (isCountDown)
        {
            elapsedTime -= Time.deltaTime;
            if (elapsedTime <= 0f)
            {
                elapsedTime = 0f;
                isRunning = false;
                onTimerEnd?.Invoke();
            }
        }
        else
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration)
            {
                elapsedTime = duration;
                isRunning = false;
                onTimerEnd?.Invoke();
            }
            
        }

        onTimerTick?.Invoke(elapsedTime);
    }

    


    public void Pause()
    {
        if(isRunning)
            isRunning = false;
    }

    public void Resume()
    {
        if(!isRunning)
            isRunning = true;
    }

    /// <summary>
    /// Resets the timer.
    /// </summary>
    public void Reset()
    {
        isRunning = false;
        elapsedTime = isCountDown ? duration : 0;
    }
}
