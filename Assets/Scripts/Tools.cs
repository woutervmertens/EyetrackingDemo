using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public static class Tools
    {
        public static Vector2 GetNormalizedTrajectory(Vector2 destination, ref Vector2 origin)
        {
            //Trajectory = Destination - Origin
            Vector2 direction = (destination - origin).normalized;
            //Set the old position to the new position
            origin.Set(destination.x, destination.y);
            //return the normalized trajectory
            return direction;
        }
    }
}