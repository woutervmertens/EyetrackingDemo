using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchScreenController : MonoBehaviour
{
    public GameObject orbitPrefab;
    public Transform canvas;

    public void AddOrbit(Vector2 centerPoint, float diameter, Color color, float speed, int n, bool clockwise = true)
    {
        GameObject o = Instantiate(orbitPrefab, canvas);
        OrbitScript os = o.GetComponent<OrbitScript>();
        Vector2 offsetPoint = new Vector2(centerPoint.x,centerPoint.y - diameter);
        os.Initialize(offsetPoint,color,speed, clockwise);
        os.Offset(720 / n);
    }
}
