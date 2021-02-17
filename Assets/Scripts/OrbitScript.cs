using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DefaultNamespace;
using MathNet.Numerics.LinearAlgebra.Double;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MathNet.Numerics.Statistics;

public class OrbitScript : MonoBehaviour
{
    public float orbitSpeed;

    public int rotationDirection = 1;

    public UnityEvent OnSelected;
    
    private Image _orbitImage;

    private Transform _orbitTransform;

    private float _diameter = 0.1f;
    
    private Queue<Vector2> _readings = new Queue<Vector2>();
    private Vector2 _prevPosition = Vector2.zero;

    private Boolean _isTarget = false;

    private int _windowsize = 30;
    private int _startUp = 0;
    private int _orbitId = -1;

    private double[] eyeX, eyeY, orbX, orbY;
    // Start is called before the first frame update
    void Start()
    {
        _orbitTransform = this.gameObject.transform.GetChild(0);
        _orbitImage = _orbitTransform.GetComponent<Image>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _orbitTransform.RotateAround(this.transform.position,-transform.forward,rotationDirection * orbitSpeed * Time.deltaTime);
    }

    public void Initialize(Vector2 startPos, Color color, float speed, bool clockwise, float diameter, bool isTarget, int id)
    {
        Start();
        orbitSpeed = speed;
        _orbitTransform.localPosition = new Vector3(startPos.x,startPos.y);
        _diameter = diameter;
        _orbitImage.color = color;
        rotationDirection = clockwise ? 1 : -1;
        _isTarget = isTarget;
        _orbitId = id;
    }

    public void Offset(float angle)
    {
        _orbitTransform.RotateAround(this.transform.position, -transform.forward, rotationDirection * angle);
    }

    private Vector2 GetNormalizedTrajectory()
    {
        Vector2 pos = new Vector2(_orbitTransform.localPosition.x, _orbitTransform.localPosition.y);
        return Tools.GetNormalizedTrajectory(pos, _prevPosition);;
    }

    /// <summary>
    /// Adds new position to the window, calculates the Pearson's correlation and reacts when the threshold is crossed.
    /// </summary>
    /// <param name="eyeReadings">The window of the eyetracking data.</param>
    /// <param name="threshold">The threshold to be crossed before a Positive is signaled.</param>
    /// <returns>A CompareResponse value.</returns>
    public CompareResponse Compare(Vector2[] eyeReadings, float threshold = 0.8f)
    {
        CompareResponse res = CompareResponse.N;
        //Don't start until window is filled
        if (_startUp++ < _windowsize)
        {
            _readings.Enqueue(GetNormalizedTrajectory());
            return res;
        }
        //Remove oldest reading. Get normalised position and add it to the queue
        _readings.Dequeue();
        Vector2 v = GetNormalizedTrajectory();
        _readings.Enqueue(v);
        
        //Split X and Y arrays
        Vector2[] tempR = _readings.ToArray();
        eyeX = new double[_windowsize];
        eyeY = new double[_windowsize];
        orbX = new double[_windowsize];
        orbY = new double[_windowsize];
        for (int i = 0; i < _windowsize; i++)
        {
            eyeX[i] = eyeReadings[i].x;
            eyeY[i] = eyeReadings[i].y;
            orbX[i] = tempR[i].x;
            orbY[i] = tempR[i].y;
        }
        //Calculate the correlations, take the minimum, see if it passes the threshold
        double correlationX = Math.Abs(Correlation.Pearson(eyeX, orbX));
        double correlationY = Math.Abs(Correlation.Pearson(eyeY, orbY));
        double correlation = Math.Min(correlationX, correlationY);
        if (correlation > threshold)
        {
            res = (_isTarget) ? CompareResponse.TP : CompareResponse.FP;
        }
        
        //If threshold is passed call the event
        if(res != 0) OnSelected.Invoke();
        
        //Output
        OutputMgr.Instance.AddOrbitTrajectory(_orbitId,v,correlation);
        return res;
    }
}
