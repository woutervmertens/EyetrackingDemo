﻿using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class TestDetection : MenuScreen
{
    public Text text;
    private int i = 0;
    public void OnTriggered()
    {
        i++;
        text.text = i.ToString();
        Reset();
    }
    
    public void Reset()
    {
        //Remove orbits
        _screenController.ClearOrbits();
        
        //Create new orbits
        _screenController.AddOrbit(new Vector2(-0.5f,0),0.2f,Color.red, 120f,1 , true);
        _screenController.AddOrbit(new Vector2(0.5f,0),0.2f,Color.black, 120f,2 );
    }
}
