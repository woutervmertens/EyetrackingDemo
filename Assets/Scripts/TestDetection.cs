using System.Collections;
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
    
    public override void Reset()
    {
        //Remove orbits
        _screenController.ClearOrbits();
        
        //Create new orbits
        _screenController.AddOrbit(new Vector2(0,0),0.1f,Color.red, 120f,1 , true);
        _screenController.AddOrbit(new Vector2(0,0),0.1f,Color.black, 120f,2 );
    }
}
