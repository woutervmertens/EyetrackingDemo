using System.Runtime.CompilerServices;
using UnityEngine;

namespace DefaultNamespace
{
    public class MenuScreen : MonoBehaviour
    {
        protected ScreenController _screenController;
        public virtual void Reset(){}
        public virtual void OnTriggered(int n){}

        public void SetScreenController(ScreenController s)
        {
            _screenController = s;
        }
    }
}