using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using Tobii.Gaming;

public class GameManager : MonoBehaviour
{
    public Boolean WatchMode = true;
    public Boolean StaticAlwaysOn = true;
    public int WindowSize = 30;

    public GameObject Watch;
    public GameObject StaticScreen;
    private ScreenController _screenController;
    private CustomFixedUpdate FU_instance;
    private HeadPose hp;
    private GazePoint gp;
    private Queue<Vector2> eyegazeData = new Queue<Vector2>();
    private int TPcount, FPcount = 0;

    void Awake()
    {
        FU_instance = new CustomFixedUpdate(OnFixedUpdate,30);
    }
    // Start is called before the first frame update
    void Start()
    {
        Watch.SetActive(WatchMode);
        StaticScreen.SetActive(!WatchMode && StaticAlwaysOn);
        if (WatchMode)
        {
            _screenController = Watch.transform.GetComponentInChildren<ScreenController>();
        }
        else
        {
            _screenController = StaticScreen.transform.GetComponentInChildren<ScreenController>();
        }
        hp = TobiiAPI.GetHeadPose();
        gp = TobiiAPI.GetGazePoint();
        //Test
        _screenController.AddOrbit(new Vector2(0,0),0.003f,Color.red, 120f,1 );
    }

    // Update is called once per frame
    void Update()
    {
        StaticScreen.SetActive(!WatchMode && !StaticAlwaysOn && hp.Rotation.eulerAngles.x > 50);
        hp = TobiiAPI.GetHeadPose();

        Debug.Log("hp: " + ((hp.IsRecent()) ? hp.Rotation.eulerAngles.x.ToString() : "No Headposition detected"));

        gp = TobiiAPI.GetGazePoint();
        Debug.Log("gp: " + ((gp.IsRecent()) ? gp.Viewport.normalized.ToString() : "No GazePoint detected"));
        FU_instance.Update();
    }
    
    void OnFixedUpdate(float dt)
    {
        GetEyeGazeData();
        CallOrbitsCompare();
    }

    private void CallOrbitsCompare()
    {
        ArrayList _orbits = _screenController.GetOrbits();
        foreach (OrbitScript o in _orbits)
        {
            HandleCompareResponse(o.Compare(eyegazeData.ToArray()));
        }
    }

    private void GetEyeGazeData()
    {
        //Add the new data (normalized)
        eyegazeData.Enqueue(gp.Viewport.normalized);
        //Limit the amount of datapoints
        if (eyegazeData.Count > WindowSize) eyegazeData.Dequeue();
    }

    private void HandleCompareResponse(CompareResponse res)
    {
        switch (res)
        {
            case CompareResponse.TP:
                TPcount++;
                break;
            case CompareResponse.FP:
                FPcount++;
                break;
        }
    }
}
