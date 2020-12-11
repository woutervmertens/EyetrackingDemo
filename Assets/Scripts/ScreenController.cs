using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public GameObject orbitPrefab;
    public Transform canvas;
    
    private ArrayList _orbits = new ArrayList();

    public void AddOrbit(Vector2 centerPoint, float diameter, Color color, float speed, int n, bool clockwise = true)
    {
        GameObject o = Instantiate(orbitPrefab, canvas);
        OrbitScript os = o.GetComponent<OrbitScript>();
        Vector2 offsetPoint = new Vector2(centerPoint.x,centerPoint.y - diameter);
        os.Initialize(offsetPoint,color,speed, clockwise,diameter);
        os.Offset(720 / n);
        _orbits.Add(os);
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
