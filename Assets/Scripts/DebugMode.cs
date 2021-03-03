using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    private OrbitScript _target;

    public void SetTarget(OrbitScript t)
    {
        _target = t;
    }

    /// <summary>
    /// Returns the screen position of the target
    /// </summary>
    /// <returns>Vector2</returns>
    public Vector2 GetDebugEyeData()
    {
        return TobiiMgr.Instance.WTS(_target.GetOrbitPosition());
    }
}
