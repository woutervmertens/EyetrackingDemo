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

    public Vector2 GetViewData()
    {
        var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.Local);
        TobiiXR_GazeRay gazeRay = eyeTrackingData.GazeRay;
        if (gazeRay.IsValid)
        {
            Vector3 screenPosition = gazeRay.Origin + gazeRay.Direction.normalized * _defaultDistance;
            return new Vector2(screenPosition.x, screenPosition.y);
        }
        return new Vector2(0, 0);
    }
   

    public Vector3 GetHMDPosition()
    {
        if (!hmd) Start();
        return hmd.transform.position;
    }

    public Vector3 GetHMDDirection()
    {
        if (!hmd) Start();
        return hmd.transform.forward;
    }
}
