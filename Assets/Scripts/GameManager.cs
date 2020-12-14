using System;
using System.Collections;
using System.Collections.Generic;
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
    
    private Dictionary<OrbitScript,Queue<Vector2>> _windowMap = new Dictionary<OrbitScript,Queue<Vector2>>();

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
    }

    // Update is called once per frame
    void Update()
    {
        FU_instance.Update();
        StaticScreen.SetActive(!WatchMode && !StaticAlwaysOn && hp.Rotation.eulerAngles.x > 50);
    }
    
    void OnFixedUpdate(float dt)
    {
        GetOrbitsData();
        GetEyeGazeData();
        //Compare
    }

    private void GetOrbitsData()
    {
        ArrayList _orbits = _screenController.GetOrbits();
        foreach (OrbitScript o in _orbits)
        {
            //Get the data queue or create a new one
            Queue<Vector2> q = _windowMap[o];
            if (q == null)
            {
                q = new Queue<Vector2>();
                _windowMap.Add(o,q);
            }
            //Add the new data (normalized)
            q.Enqueue(o.GetNormalizedPosition());
            //Limit the amount of datapoints
            if (q.Count > WindowSize) q.Dequeue();
            //Set the datapoints for this orbit
            _windowMap[o] = q;
        }
    }

    private void GetEyeGazeData()
    {
        //Add the new data (normalized)
        eyegazeData.Enqueue(gp.Viewport.normalized);
        //Limit the amount of datapoints
        if (eyegazeData.Count > WindowSize) eyegazeData.Dequeue();
    }
}
