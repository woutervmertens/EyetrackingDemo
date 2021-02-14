using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public GameObject orbitPrefab;
    public MenuScreen testScreen;
    public GameObject clock;
    public GameObject atp;
    public GameObject ata;
    public GameObject musicPlayer;
    public GameObject callMenu;
    public Transform canvas;
    
    private ArrayList _orbits = new ArrayList();

    private void Start()
    {
        testScreen.SetScreenController(this);
        testScreen.Reset();
    }

    public void AddOrbit(Vector2 centerPoint, float diameter, Color color, float speed, int n, bool clockwise = true, bool isTarget = false)
    {
        GameObject o = Instantiate(orbitPrefab, canvas);
        OrbitScript os = o.GetComponent<OrbitScript>();
        Vector2 offsetPoint = new Vector2(centerPoint.x,centerPoint.y - diameter);
        os.Initialize(offsetPoint,color,speed, clockwise,diameter, isTarget);
        os.Offset(720 / n);
        _orbits.Add(os);
    }

    public void ClearOrbits()
    {
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
