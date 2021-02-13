using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestDetection : MonoBehaviour
{
    public Text text;
    private int i = 0;
    public void OnTriggered()
    {
        i++;
        text.text = i.ToString();
        Reset();
    }

    void Reset()
    {
        //Remove orbits
        
        //Create new orbits
    }
}
