using UnityEngine;

namespace DefaultNamespace
{
    public static class Tools
    {
        public static Vector2 GetNormalizedTrajectory(Vector2 newPos, Vector2 oldPos)
        {
            Vector2 traj = newPos - oldPos;
            oldPos.Set(newPos.x,newPos.y);
            return traj.normalized;
        }
    }
}