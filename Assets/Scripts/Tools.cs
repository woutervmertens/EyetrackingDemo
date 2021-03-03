using UnityEngine;

namespace DefaultNamespace
{
    public static class Tools
    {
        public static Vector2 GetNormalizedTrajectory(Vector2 newPos, ref Vector2 oldPos)
        {
            //Trajectory = Destination - Origin
            Vector2 traj = newPos - oldPos;
            //Set the old position to the new position
            oldPos.Set(newPos.x,newPos.y);
            //return the normalized trajectory
            return traj.normalized;
        }
    }
}