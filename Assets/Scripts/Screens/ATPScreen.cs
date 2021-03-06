using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    
    public class ATPScreen : MenuScreen
    {
        public override void OnTriggered(int n)
        {
            base.OnTriggered(n);
            _screenController?.Next();
        }
    
        public override void Reset()
        {
            //Remove orbits
            _screenController.ClearOrbits();
        
            //Create new orbits
            _screenController.AddOrbit(new Vector2(0,0),0.3f,Color.red, 120f,1 , true,true);

            //Output
            if(index == 0) OutputMgr.Instance.StartNewTest($"ATP Test");

        }

    }
}