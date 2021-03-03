using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.XR;
using System;
using MathNet.Numerics.LinearAlgebra.Double;

public class TobiiMgr : MonoBehaviour
{
    public float rayRadius = 0.05f;
    public float rayDistance = 10f;
    public int rayLayerMask = 1 << 9;

    private Camera hmd;

    private static TobiiMgr instance = null;
    public static TobiiMgr Instance { get { return instance != null ? instance : (instance = new GameObject("TobiiMgr").AddComponent<TobiiMgr>()); } }

    // Start is called before the first frame update
    void Start()
    {
        hmd = Camera.main;
    }

    /// <summary>
    /// Takes the gazeRay from the eyetrackingdata, if it is valid returns the world to screen point the eye is looking at.
    /// </summary>
    /// <returns>A vector2</returns>
    public Vector2 GetViewData()
    {
        var provider = TobiiXR.Internal.Provider;
        var eyeTrackingData = EyeTrackingDataHelper.Clone(provider.EyeTrackingDataLocal);
        var localToWorldMatrix = provider.LocalToWorldMatrix;
        EyeTrackingDataHelper.TransformGazeData(eyeTrackingData, localToWorldMatrix);
        
        TobiiXR_GazeRay gazeRay = eyeTrackingData.GazeRay;
        if (gazeRay.IsValid)
        {
            RaycastHit hit;
            float hitDistance = 1f;
            Ray ray = new Ray(gazeRay.Origin, gazeRay.Direction);
            //sphere moves along ray and returns hits
            //hitDistance = (Physics.SphereCast (ray, rayRadius, out hit, rayDistance, rayLayerMask)) ? hit.distance : rayDistance / 10f;
            Debug.DrawRay(gazeRay.Origin, gazeRay.Direction, Color.red);
            
            return hmd.WorldToScreenPoint(ray.GetPoint(hitDistance));
        }
        return new Vector2(0, 0);
    }

    private Transform getHMDTransform()
    {
        if (!hmd) Start();
        return hmd.transform;
    }

    public Vector3 WTS(Vector3 v)
    {
        if (!hmd) Start();
        return hmd.WorldToScreenPoint(v);
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
