using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{ 
    public static Timer Instance { get; private set; }

    private float time;
    private Action timerCallback;

    private void Awake()
    {
        Instance = this;
    }
    public void SetTimer(float time, Action timerCallback)
    {
        this.time = time;
        this.timerCallback = timerCallback;
    }

    private void Update()
    {
        if (time > 0f)
        {
            time -= Time.deltaTime;

            if (time <= 0f)
            {
                timerCallback();
            }
        }
    }
}
