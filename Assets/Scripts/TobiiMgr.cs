using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.XR;
using System;

public class TobiiMgr : MonoBehaviour
{
    float _defaultDistance = 2f;

    private Camera hmd;

    private static TobiiMgr instance = null;
    public static TobiiMgr Instance { get { return instance != null ? instance : (instance = new GameObject("TobiiMgr").AddComponent<TobiiMgr>()); } }

    // Start is called before the first frame update
    void Start()
    {
        hmd = Camera.main;
    }

    /// <summary>
    /// Takes the gazeRay from the eyetrackingdata, if it is valid returns a normalised vector which approximates the point on the screen the eye is looking at.
    /// </summary>
    /// <returns>A vector2 between (-1,-1) and (1,1)</returns>
    public Vector2 GetViewData()
    {
        var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.Local);
        TobiiXR_GazeRay gazeRay = eyeTrackingData.GazeRay;
        if (gazeRay.IsValid)
        {
            Vector3 screenPosition = gazeRay.Origin + gazeRay.Direction.normalized * _defaultDistance;
            return new Vector2(screenPosition.x, screenPosition.y);//TODO: normalize?
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

    public Quaternion GetHMDRotation()
    {
        return getHMDTransform().rotation;
    }

    public Vector3 GetHMDDirection()
    {
        return getHMDTransform().forward;
    }
}
