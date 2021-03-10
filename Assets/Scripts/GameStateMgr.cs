using UnityEngine;

namespace DefaultNamespace
{
    public enum GameState
    {
        Measuring,
        Resetting,
        Paused
    }
    public class GameStateMgr : MonoBehaviour
    {
        private static GameStateMgr instance = null;
        public static GameStateMgr Instance { get { return instance != null ? instance : (instance = new GameObject("GameStateMgr").AddComponent<GameStateMgr>()); } }
        
        public GameState State { get; set; }
    }

    /* How to reset:
    IEnumerator endReset()
    {
        yield return new WaitForSeconds(0.5f);
        GameStateMgr.Instance.State = GameState.Measuring;
    }
    */
}