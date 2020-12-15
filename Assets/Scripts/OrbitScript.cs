using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DefaultNamespace;
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

    public void Initialize(Vector2 startPos, Color color, float speed, bool clockwise, float diameter)
    {
        Start();
        orbitSpeed = speed;
        _orbitTransform.localPosition = new Vector3(startPos.x,startPos.y);
        _diameter = diameter;
        _orbitImage.color = color;
        rotationDirection = clockwise ? 1 : -1;
    }

    public void Offset(float angle)
    {
        _orbitTransform.RotateAround(this.transform.position, -transform.forward, rotationDirection * angle);
    }

    public Vector2 GetNormalizedPosition()
    {
        return new Vector2(_orbitTransform.localPosition.x/_diameter,_orbitTransform.localPosition.y/_diameter);;
    }

    public void SetAsTarget()
    {
        SetAsTarget(Color.red);
    }
    public void SetAsTarget(Color c)
    {
        _isTarget = true;
        _orbitImage.color = c;
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
        //Remove oldest reading. Get normalised position and add it to the queue
        _readings.Dequeue();
        _readings.Enqueue(GetNormalizedPosition());
        
        //Split X and Y arrays
        Vector2[] tempR = _readings.ToArray();
        eyeX = new double[30];
        eyeY = new double[30];
        orbX = new double[30];
        orbY = new double[30];
        for (int i = 0; i < 30; i++)
        {
            eyeX[i] = eyeReadings[i].x;
            eyeY[i] = eyeReadings[i].y;
            orbX[i] = tempR[i].x;
            orbY[i] = tempR[i].y;
        }
        //Calculate the correlations, take the minimum, see if it passes the threshold
        double correlationX = Correlation.Pearson(eyeX, orbX);
        double correlationY = Correlation.Pearson(eyeY, orbY);
        double correlation = Math.Min(correlationX, correlationY);
        if (correlation > threshold)
        {
            res = (_isTarget) ? CompareResponse.TP : CompareResponse.FP;
        }
        
        //If threshold is passed call the event
        if(res != 0) OnSelected.Invoke();
        return res;
    }
}
