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

    const float hoursToDegrees = -30f, minutesToDegrees = -6f, secondsToDegrees = -6f;

    // Update is called once per frame
    void Update()
    {
        TimeSpan time = DateTime.Now.TimeOfDay;
        HourHand.localEulerAngles = new Vector3(0, 0, hoursToDegrees * (float) time.TotalHours);
        MinuteHand.localEulerAngles = new Vector3(0, 0, minutesToDegrees * (float) time.TotalMinutes);
        SecondHand.localEulerAngles = new Vector3(0, 0, secondsToDegrees * (float) time.TotalSeconds);
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
