﻿using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Boolean WatchMode = true;
    public Boolean StaticAlwaysOn = true;
    public Boolean SaveData = false;
    public int WindowSize = 30;

    public GameObject Watch;
    public GameObject StaticScreen;
    private ScreenController _screenController;
    private CustomFixedUpdate FU_instance;
    
    private Queue<Vector2> eyegazeData = new Queue<Vector2>(); // holds the normalised trajectories of the eye
    private Vector2 _prevEyePos = Vector2.zero;
    private int TPcount, FPcount = 0;

    void Awake()
    {
        FU_instance = new CustomFixedUpdate(OnFixedUpdate,30);
    }
    // Start is called before the first frame update
    void Start()
    {
        OutputMgr.Instance.SetOutputType(OutputType.CSV);
        OutputMgr.Instance.Activate(SaveData);
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

        GameStateMgr.Instance.State = GameState.Measuring;
        //Test
        //_screenController.AddOrbit(new Vector2(0,0),0.003f,Color.red, 120f,1 );
    }

    // Update is called once per frame
    void Update()
    {
        
        float x = TobiiMgr.Instance.GetHMDRotation().x;
        StaticScreen.SetActive(!WatchMode && (StaticAlwaysOn || (x > 15 && x < 60)));
        FU_instance.Update();
    }
    
    void OnFixedUpdate(float dt)
    {
        if (GameStateMgr.Instance.State != GameState.Measuring) return;
        GetEyeGazeData();
        CallOrbitsCompare();
    }

    

    private void CallOrbitsCompare()
    {
        if (GameStateMgr.Instance.State != GameState.Measuring) return;
        ArrayList _orbits = _screenController.GetOrbits();
        for (var i = 0; i < _orbits.Count; i++)
        {
            var o = (OrbitScript) _orbits[i];
            HandleCompareResponse(o.Compare(eyegazeData.ToArray()));
        }
    }

    private void GetEyeGazeData()
    {
        if (GameStateMgr.Instance.State != GameState.Measuring) return;
        //Add the new data (normalized)
        Vector2 vd = TobiiMgr.Instance.GetViewData();
        Vector2 v = Tools.GetNormalizedTrajectory(vd, ref _prevEyePos);
        eyegazeData.Enqueue(v);
        //Limit the amount of datapoints
        if (eyegazeData.Count > WindowSize) eyegazeData.Dequeue();
        //Output
        OutputMgr.Instance.AddEyeTrajectory(v,vd);
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
        OutputMgr.Instance.AddCompareResponse(res);
    }
}
