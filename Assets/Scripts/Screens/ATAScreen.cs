using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    [System.Serializable]
    public struct ClockTime
    {
        public int Hours;
        public int Minutes;
    }
    public class ATAScreen : MenuScreen
    {
        public ClockScript Clock;
        
        public ClockTime[] Times;

        public override void OnTriggered(int n)
        {
            base.OnTriggered(n);
            //if(Times.Length > index)
                Reset();
        }
    
        public override void Reset()
        {
            //Remove orbits
            _screenController.ClearOrbits();
        
            //Create new orbits
            _screenController.AddOrbit(new Vector2(0,0),0.3f,Color.red, 120f,1 , true,true);

            //Set time
            if(Times.Length > index)
            {
                ClockTime c = Times[index];
                Clock?.SetTime(c.Hours, c.Minutes);
            }

            //Output
            if(index == 0) OutputMgr.Instance.StartNewTest($"ATA Test");

        }

    }
}