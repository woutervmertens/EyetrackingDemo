﻿using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScreen : MenuScreen
{
    public override void OnTriggered(int n)
    {
        base.OnTriggered(n);
    }

    public override void Reset()
    {
        //Output
        if (index == 0) OutputMgr.Instance.StartNewTest($"Music screen Test");
    }
}
