using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class OutputEyetrackingData : MonoBehaviour
{
    private CustomFixedUpdate FU_instance;

    void Awake()
    {
        FU_instance = new CustomFixedUpdate(OnFixedUpdate,30);
    }

    // Update is called once per frame
    void Update()
    {
        FU_instance.Update();
    }

    void OnFixedUpdate(float dt)
    {
        Console.Out.WriteLine("Viewdata: " + TobiiMgr.Instance.GetViewData().ToString());
        Console.Out.WriteLine("Headpose: " + TobiiMgr.Instance.GetHMDRotation().eulerAngles.ToString());
    }
}
