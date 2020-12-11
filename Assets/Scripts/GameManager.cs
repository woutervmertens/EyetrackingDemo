using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Boolean WatchMode = true;
    public int WindowSize = 30;
    
    
    public GameObject Watch;
    public GameObject StaticScreen;
    private ScreenController _screenController;
    private CustomFixedUpdate FU_instance;
    
    private Dictionary<OrbitScript,Queue<Vector2>> _windowMap = new Dictionary<OrbitScript,Queue<Vector2>>();

    void Awake()
    {
        FU_instance = new CustomFixedUpdate(OnFixedUpdate,30);
    }
    // Start is called before the first frame update
    void Start()
    {
        Watch.SetActive(WatchMode);
        StaticScreen.SetActive(!WatchMode);
        if (WatchMode)
        {
            _screenController = Watch.transform.GetComponentInChildren<ScreenController>();
        }
        else
        {
            _screenController = StaticScreen.transform.GetComponentInChildren<ScreenController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        FU_instance.Update();
    }
    
    void OnFixedUpdate(float dt)
    {
        ArrayList _orbits = _screenController.GetOrbits();
        foreach (OrbitScript o in _orbits)
        {
            Queue<Vector2> q = _windowMap[o];
            if (q == null)
            {
                q = new Queue<Vector2>();
                _windowMap.Add(o,q);
            }
            q.Enqueue(o.GetNormalizedPosition());
            if (q.Count > WindowSize) q.Dequeue();
            _windowMap[o] = q;
        }
    }
}
