using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.XR;
using System;
using MathNet.Numerics.LinearAlgebra.Double;

public class TobiiMgr : MonoBehaviour
{
    float _defaultDistance = 2f;
    Plane plane;
    Vector3 distanceFromCamera;

    private Camera hmd;

    private static TobiiMgr instance = null;
    public static TobiiMgr Instance { get { return instance != null ? instance : (instance = new GameObject("TobiiMgr").AddComponent<TobiiMgr>()); } }

    // Start is called before the first frame update
    void Start()
    {
        hmd = Camera.main;
        distanceFromCamera = new Vector3(hmd.transform.position.x, hmd.transform.position.y, hmd.transform.position.z - _defaultDistance);
        plane = new Plane(Vector3.forward, distanceFromCamera);
        
    }

    /// <summary>
    /// Takes the gazeRay from the eyetrackingdata, if it is valid returns a normalised vector which approximates the point on the screen the eye is looking at.
    /// </summary>
    /// <returns>A vector2 between (-1,-1) and (1,1)</returns>
    public Vector2 GetViewData()
    {
        distanceFromCamera = new Vector3(hmd.transform.position.x, hmd.transform.position.y, hmd.transform.position.z - _defaultDistance);
        plane.Translate(distanceFromCamera);
        var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.Local);
        TobiiXR_GazeRay gazeRay = eyeTrackingData.GazeRay;
        if (gazeRay.IsValid)
        {
            return hmd.WorldToScreenPoint(gazeRay.Origin + gazeRay.Direction.normalized * 10);
            
            /*Ray ray = new Ray(gazeRay.Origin, gazeRay.Direction);
            float enter = 0.0f;
            if(plane.Raycast(ray, out enter))
            {
                Vector3 screenPosition = ray.GetPoint(enter);
                return hmd.WorldToScreenPoint(screenPosition);
            }*/
        }
        return new Vector2(0, 0);
    }

    private Transform getHMDTransform()
    {
        if (!hmd) Start();
        return hmd.transform;
    }
   

    public Vector3 GetHMDPosition()
    {
        return getHMDTransform().position;
    }

    public Vector3 GetHMDRotation()
    {
        return getHMDTransform().eulerAngles;
    }

    public Vector3 GetHMDDirection()
    {
        return getHMDTransform().forward;
    }
}
