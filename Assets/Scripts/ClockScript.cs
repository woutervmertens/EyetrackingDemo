using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockScript : MonoBehaviour
{
    public GameObject numbers;

    public Transform HourHand;

    public Transform MinuteHand;

    public Transform SecondHand;

    private Boolean IsCustomTime = false;

    const float hoursToDegrees = -30f, minutesToDegrees = -6f, secondsToDegrees = -6f;

    private TimeSpan time, startTime;

    private int customHour = 0, customMinute = 0;

    // Update is called once per frame
    void Update()
    {
        time = DateTime.Now.TimeOfDay;
        float seconds = (IsCustomTime) ? (float)(time.TotalSeconds - startTime.TotalSeconds) : (float)time.TotalSeconds;
        float minutes = (IsCustomTime) ? customMinute + (float)(time.TotalMinutes - startTime.TotalMinutes): (float)time.TotalMinutes;
        float hours = (IsCustomTime) ? customHour + minutes/60f: (float)time.TotalHours;
        

        HourHand.localEulerAngles = new Vector3(0, 0, hoursToDegrees * hours);
        MinuteHand.localEulerAngles = new Vector3(0, 0, minutesToDegrees * minutes);
        SecondHand.localEulerAngles = new Vector3(0, 0, secondsToDegrees * seconds);
    }

    public void SetRealTime()
    {
        IsCustomTime = false;
    }

    public void SetTime(int hours, int minutes)
    {
        customHour = hours;
        customMinute = minutes;
        startTime = DateTime.Now.TimeOfDay;
        IsCustomTime = true;
    }

    public void TurnOffNumbers()
    {
        numbers.SetActive(false);
    }
    public void TurnOmNumbers()
    {
        numbers.SetActive(true);
    }
}
