﻿using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class OrbitScript : MonoBehaviour
{
    private Image _orbitImage;

    private Transform _orbitTransform;

    public float orbitSpeed;

    public int rotationDirection = 1;

    private float _diameter = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        _orbitTransform = this.gameObject.transform.GetChild(0);
        _orbitImage = _orbitTransform.GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _orbitTransform.RotateAround(this.transform.position,Vector3.up,rotationDirection * orbitSpeed * Time.deltaTime);
    }

    public void Initialize(Vector2 startPos, Color color, float speed, bool clockwise, float diameter)
    {
        orbitSpeed = speed;
        _orbitTransform.localPosition = new Vector3(startPos.x,startPos.y);
        _diameter = diameter;
        _orbitImage.color = color;
        rotationDirection = clockwise ? 1 : -1;
    }

    public void Offset(float angle)
    {
        _orbitTransform.RotateAround(this.transform.position,Vector3.up,rotationDirection * angle);
    }

    public Vector2 GetNormalizedPosition()
    {
        return new Vector2(_orbitTransform.localPosition.x/_diameter,_orbitTransform.localPosition.y/_diameter);;
    }
}
