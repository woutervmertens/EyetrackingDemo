using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class OutputEyetrackingData : MonoBehaviour
{
    private CustomFixedUpdate FU_instance;
    private GazePoint gp;
    private HeadPose hp;

    void Awake()
    {
        FU_instance = new CustomFixedUpdate(OnFixedUpdate,30);
    }
    // Start is called before the first frame update
    void Start()
    {
        gp = TobiiAPI.GetGazePoint();
        hp = TobiiAPI.GetHeadPose();
    }
    
    // Update is called once per frame
    void Update()
    {
        FU_instance.Update();
    }

    void OnFixedUpdate(float dt)
    {
        Console.Out.WriteLine("Viewport: " + gp.Viewport.ToString());
        Console.Out.WriteLine("Screen: " + gp.Screen.ToString());
        Console.Out.WriteLine("Headpose: " + hp.Rotation.eulerAngles.ToString());
    }
}
