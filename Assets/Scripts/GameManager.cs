using System;
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
    private int TPcount, FPcount = 0;

    void Awake()
    {
        FU_instance = new CustomFixedUpdate(OnFixedUpdate,30);
        OutputMgr.Instance.SetOutputType(OutputType.CSV);
        OutputMgr.Instance.Activate(SaveData);
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

        GameStateMgr.Instance.State = GameState.Measuring;
   
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

    
    /// <summary>
    /// If in measuring state, get all orbits and have them compare positional correlations with the gaze data queue.
    /// </summary>
    private void CallOrbitsCompare()
    {
        if (GameStateMgr.Instance.State != GameState.Measuring) return;
        ArrayList _orbits = _screenController.GetOrbits();
        Vector2[] tempR = eyegazeData.ToArray();
        for (var i = 0; i < _orbits.Count; i++)
        {
            var o = (OrbitScript) _orbits[i];
            HandleCompareResponse(o.Compare(tempR));
        }
    }

    /// <summary>
    /// If in measuring state, add new gaze point data to the queue.
    /// </summary>
    private void GetEyeGazeData()
    {
        if (GameStateMgr.Instance.State != GameState.Measuring) return;
        //Add the new data
        Vector3 vd = TobiiMgr.Instance.GetViewData();
        Vector2 v = TobiiMgr.Instance.WTS(vd);
        eyegazeData.Enqueue(v);
        //Limit the amount of datapoints
        if (eyegazeData.Count > WindowSize) eyegazeData.Dequeue();
        //Output
        OutputMgr.Instance.AddEyeData(v,TobiiMgr.Instance.WTS(vd));
    }

    private void HandleCompareResponse(CompareResponse res)
    {
        switch (res)
        {
            case CompareResponse.TargetSelected:
                TPcount++;
                break;
            case CompareResponse.DummySelected:
                FPcount++;
                break;
        }
        OutputMgr.Instance.AddCompareResponse(res);
    }
}
