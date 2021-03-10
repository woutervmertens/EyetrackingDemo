using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedCallScreen : MenuScreen
{
    public override void OnTriggered(int n)
    {
        base.OnTriggered(n);
    }

    public override void Reset()
    {
        //Output
        if (index == 0) OutputMgr.Instance.StartNewTest($"Missed Call Test");
    }
}
