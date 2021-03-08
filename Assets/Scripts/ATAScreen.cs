using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    
    public class ATAScreen : MenuScreen
    {
        private int i = 0;
        public override void OnTriggered(int n)
        {
            i++;
            _screenController.Next();
        }
    
        public override void Reset()
        {
            GameStateMgr.Instance.State = GameState.Resetting;
            //Remove orbits
            _screenController.ClearOrbits();
        
            //Create new orbits
            _screenController.AddOrbit(new Vector2(0,0),0.3f,Color.red, 120f,1 , true,true);

            //Output
            if(i == 0) OutputMgr.Instance.StartNewTest($"ATA Test");

            StartCoroutine(endReset());
        }

        IEnumerator endReset()
        {
            yield return new WaitForSeconds(0.5f);
            GameStateMgr.Instance.State = GameState.Measuring;
        }
    }
}