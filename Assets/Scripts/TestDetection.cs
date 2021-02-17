using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class TestDetection : MenuScreen
{
    public Text text;
    private int i = 0;
    public override void OnTriggered(int n)
    {
        i++;
        text.text = i.ToString();
        Reset();
    }
    
    public override void Reset()
    {
        GameStateMgr.Instance.State = GameState.Resetting;
        //Remove orbits
        _screenController.ClearOrbits();
        
        //Create new orbits
        _screenController.AddOrbit(new Vector2(0,0),0.3f,Color.red, 120f,1 , true,true);
        _screenController.AddOrbit(new Vector2(0,0),0.3f,Color.black, 120f,2 );
        
        //Output
        OutputMgr.Instance.StartNewTest($"CorrelationTest {i}");

        StartCoroutine(endReset());
    }

    IEnumerator endReset()
    {
        yield return new WaitForSeconds(0.5f);
        GameStateMgr.Instance.State = GameState.Measuring;
    }
}
