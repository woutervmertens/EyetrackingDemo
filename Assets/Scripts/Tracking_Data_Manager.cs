using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Tobii.Research.Unity;
using Tobii.XR.GazeModifier;
using Tobii.XR;

public class Tracking_Data_Manager : MonoBehaviour
{


    [Header("Gaze Variables")]
    public int windowSize = 10;
    public float saccadeThreshold = 1.5f;
    const int MAXHISTORY = 150;

    List<Vector3> gazeHistory = new List<Vector3>();

    //public IVRGazeData currentGazeData 
    //{
    //	get 
    //	{ 
    //		return m_currentGazeData;
    //	}
    //	set 
    //	{
    //		if (m_currentGazeData == value) 
    //		{
    //			return;
    //		}
    //		m_currentGazeData = value;
    //		if (OnNewGazeData != null) 
    //		{
    //			OnNewGazeData (m_currentGazeData);
    //		}
    //	}
    //}
    //private IVRGazeData m_currentGazeData;
    //public delegate void NewGazeDataEventHandler(IVRGazeData newGazeData);
    //public static event NewGazeDataEventHandler OnNewGazeData;

    public Ray currentProcessedGazeRay
    {
        get
        {
            return m_currentProcessedGazeRay;
        }
        set
        {
            m_currentProcessedGazeRay = value;
            if (OnNewProcessedGazeRay != null)
            {
                //print("hej");
                OnNewProcessedGazeRay(m_currentProcessedGazeRay);
            }
        }
    }
    private Ray m_currentProcessedGazeRay;
    public delegate void NewProcessedGazeRayEventHandler(Ray newProcessedGazeRay);
    public static event NewProcessedGazeRayEventHandler OnNewProcessedGazeRay;


    public Ray currentHeadRay
    {
        get
        {
            return m_currentHeadRay;
        }
        set
        {
            m_currentHeadRay = value;
            if (OnNewHeadRay != null)
            {
                OnNewHeadRay(m_currentHeadRay);
            }
        }
    }
    private Ray m_currentHeadRay;
    public delegate void NewHeadRayEventHandler(Ray newHeadRay);
    public static event NewHeadRayEventHandler OnNewHeadRay;


    //private VREyeTracker eyeTracker;
    private Camera hmd;

    // Use this for initialization
    void Start()
    {
        //eyeTracker = VREyeTracker.Instance;
        hmd = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //if (eyeTracker)
        //{
        //    while (eyeTracker.GazeDataCount > 0)
        //    {
        //        currentGazeData = eyeTracker.NextData;
        //        currentProcessedGazeRay = ProcessGazeRay(currentGazeData.CombinedGazeRayWorld);
        //    }
        //}
        //else
        //{
        var provider = TobiiXR.Internal.Provider;
        var eyeTrackingData = EyeTrackingDataHelper.Clone(provider.EyeTrackingDataLocal);
        var localToWorldMatrix = provider.LocalToWorldMatrix;
        var worldForward = localToWorldMatrix.MultiplyVector(Vector3.forward);
        EyeTrackingDataHelper.TransformGazeData(eyeTrackingData, localToWorldMatrix);

        var gazeRay = eyeTrackingData.GazeRay;
        Ray newRay = new Ray(gazeRay.Origin, gazeRay.Direction);
        currentProcessedGazeRay = ProcessGazeRay(newRay);
        //}
        currentHeadRay = new Ray(hmd.transform.position, hmd.transform.rotation * Vector3.forward);
    }

    private Ray ProcessGazeRay(Ray ray)
    {
        Vector3 origin = new Vector3(0, 0, 0);
        SmoothGazeRay(ray.direction);
        // we don't need to smooth the origin
        origin = ray.origin;


        Vector3 direction = ComputeAverage(gazeHistory);


        return new Ray(origin, direction);
    }


    private void SmoothGazeRay(Vector3 value)
    {
        gazeHistory.Add(value);
        if (gazeHistory.Count > MAXHISTORY) gazeHistory.RemoveAt(0);
        if (IsSaccade(gazeHistory))
        {
            gazeHistory.Clear();
            gazeHistory.Add(value);
        }
    }

    private bool IsSaccade(List<Vector3> history)
    {
        if (history.Count >= 2)
        {
            // check angle of current and previous gaze sample. if bigger, its a saccade, and then reset history
            Vector3 currentgaze = history[history.Count - 1];
            Vector3 lastgaze = history[history.Count - 2];

            Vector3 origin = Vector3.zero;
            Vector3 currentGazeDirection = currentgaze - origin;
            Vector3 lastGazeDirection = lastgaze - origin;

            float currentangle = Vector3.Angle(currentGazeDirection, lastGazeDirection);

            if (currentangle > saccadeThreshold)
            {
                return true;
            }

        }
        return false;
    }

    private Vector3 ComputeAverage(List<Vector3> history)
    {
        Vector3 avg = Vector3.zero;
        int window = (windowSize < history.Count) ? windowSize : history.Count;

        for (int i = 1; i <= window; i = i + 1)
        {
            avg += history[history.Count - i];
        }
        // print ("average of " + window + "frames");
        return new Vector3(avg.x / window, avg.y / window, avg.z / window);
    }


}
