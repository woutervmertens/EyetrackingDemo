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

    public void AddListener(UnityAction call)
    {
        OnSelected.AddListener(call);
    }

    public Vector3 GetOrbitPosition()
    {
        return _orbitTransform.position;
    }

    /// <summary>
    /// Adds new position to the window, calculates the Pearson's correlation and reacts when the threshold is crossed.
    /// Pearson Correlation:
    ///     1: dataA = dataB: perfect correlation
    ///     -1: dataA = reverse dataB: inverse correlation
    ///     0: no correlation
    /// </summary>
    /// <param name="eyeReadings">The window of the eyetracking data.</param>
    /// <param name="threshold">The threshold to be crossed before a Positive is signaled.</param>
    /// <returns>A CompareResponse value.</returns>
    public CompareResponse Compare(in Vector2[] eyeReadings, float threshold = 0.8f)
    {
        CompareResponse res = CompareResponse.N;
        Vector2 v = TobiiMgr.Instance.WTS(GetOrbitPosition());
        //Don't start until window is filled
        if (_startUp++ < _windowsize)
        {
            _readings.Enqueue(v);
            return res;
        }
        //Remove oldest reading. Get normalised position and add it to the queue
        _readings.Dequeue();
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
        double correlationX = resolveNaN(Correlation.Pearson(eyeX, orbX));
        double correlationY = resolveNaN(Correlation.Pearson(eyeY, orbY));

        double correlation = Math.Min(correlationX, correlationY);
        if (correlation > threshold && isFacingCamera())
        {
            res = (_isTarget) ? CompareResponse.TargetSelected : CompareResponse.DummySelected;
        }
        
        //If threshold is passed call the event
        if(res == CompareResponse.TargetSelected) OnSelected.Invoke();
        
        //Output
        OutputMgr.Instance.AddOrbitData(_orbitId,v, TobiiMgr.Instance.WTS(GetOrbitPosition()), correlation);
        return res;
    }

    private bool isFacingCamera()
    {
        return Vector3.Dot(transform.forward, TobiiMgr.Instance.GetHMDPosition() - transform.position) < 0;
    }

    /// <summary>
    /// In case <paramref name="d"/> is NaN, return -1.
    /// </summary>
    /// <param name="d">The double to check.</param>
    /// <returns>The given double or -1.</returns>
    private double resolveNaN(double d)
    {
        return (Double.IsNaN(d)) ? -1 : d;
    }
    
}
