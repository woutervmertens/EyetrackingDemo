using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Object = UnityEngine.Object;

public class ScreenController : MonoBehaviour
{
    public GameObject orbitPrefab;
    public MenuScreen testScreen;
    public MenuScreen[] Screens;
    public int currentScreenIndex = 0;
    public Transform canvas;
    public Boolean IsTestMode = false;
    
    private ArrayList _orbits = new ArrayList();

    private void Start()
    {
        canvas = transform.GetComponentInChildren<Canvas>().transform;
        //test
        if (IsTestMode)
        {
            testScreen.SetScreenController(this);
            testScreen.Reset();
            foreach (var item in Screens)
            {
                item.gameObject.SetActive(false);
            }
            return;
        }
        //start with first screen
        if (Screens.Length > currentScreenIndex )
        {
            MenuScreen curr = Screens[currentScreenIndex];
            curr.gameObject.SetActive(true);
            curr.SetScreenController(this);
            curr.Reset();
        }
    }

    public void Next()
    {
        if (IsTestMode)
        {
            ClearOrbits();
            return;
        }
        //turn off old
        MenuScreen curr = Screens[currentScreenIndex];
        curr.gameObject.SetActive(false);
        ClearOrbits();
        //start new
        if (Screens.Length > currentScreenIndex + 1)
        {
            currentScreenIndex++;
            curr = Screens[currentScreenIndex];
            curr.gameObject.SetActive(true);
            curr.SetScreenController(this);
            curr.Reset();
        }
    }

    public void AddOrbit(Vector2 centerPoint, float diameter, Color color, float speed, int n, bool clockwise = true, bool isTarget = false)
    {
        GameObject o = Instantiate(orbitPrefab, canvas);
        OrbitScript os = o.GetComponent<OrbitScript>();
        Vector2 offsetPoint = new Vector2(centerPoint.x,centerPoint.y - diameter);
        os.Initialize(offsetPoint,color,speed, clockwise,diameter, isTarget, n);
        os.Offset(360 / n);
        if(IsTestMode) os.OnSelected.AddListener(() => { testScreen.OnTriggered(n); });
        else os.OnSelected.AddListener(() => { Screens[currentScreenIndex].OnTriggered(n);});
        _orbits.Add(os);
    }

    public void ClearOrbits()
    {
        for (int i = _orbits.Count - 1; i >= 0; i--)
        {
            Destroy(((OrbitScript) _orbits[i]).gameObject);
        }
        _orbits.Clear();
    }

    /// <summary>
    /// Returns all the currently active orbits.
    /// </summary>
    /// <returns>An ArrayList of OrbitScript</returns>
    public ArrayList GetOrbits()
    {
        return _orbits;
    }
    
    
}
