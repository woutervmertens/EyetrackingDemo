using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace DefaultNamespace
{
    enum DataType
    {
        Test,
        Trajectory
    }

    class OutputData
    {
        private int _second;
        private SortedList _orbitStrings = new SortedList();
        private String _eyeString = "";
        private String _testString = "";
        private DataType _type;
        
        public OutputData(int second, String name)
        {
            _second = second;
            _testString = $"{second} ===== New Test: {name} ===== \n";
            _type = DataType.Test;
        }
        public OutputData(int second, int orbitId, Vector2 trajectory, double minCorrelation)
        {
            _second = second;
            _orbitStrings.Add(orbitId, $"Orbit {orbitId}, Trajectory {trajectory}, Correlation {minCorrelation}");
            _type = DataType.Trajectory;
        }
        public OutputData(int second, Vector2 trajectory)
        {
            _second = second;
            _eyeString = $"Eye Trajectory {trajectory}";
            _type = DataType.Trajectory;
        }

        public void AddOrbit(int orbitId, Vector2 trajectory, double minCorrelation)
        {
            Assert.AreNotEqual(_type,DataType.Test);
            if(!_orbitStrings.ContainsKey(orbitId))_orbitStrings.Add(orbitId, $"Orbit {orbitId}, Trajectory {trajectory}, Correlation {minCorrelation}");
        }

        public void AddEye(Vector2 trajectory)
        {
            Assert.AreNotEqual(_type,DataType.Test);
            _eyeString = $"Eye Trajectory {trajectory}";
        }

        public String GetOutput()
        {
            switch (_type)
            {
                case DataType.Test:
                    return _testString;
                    break;
                case DataType.Trajectory:
                    String o = "";
                    foreach (var str in _orbitStrings.Values)
                    {
                        o += $"{str}; ";
                    }
                    return $"{_second}: {_eyeString}; {o}\n";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public class OutputMgr : MonoBehaviour
    {
        private static OutputMgr instance = null;
        public static OutputMgr Instance { get { return instance != null ? instance : (instance = new GameObject("OutputMgr").AddComponent<OutputMgr>()); } }

        public string path = "Assets/TestData/";
        private SortedList outputList = new SortedList(); 
        
        private CustomFixedUpdate FU_instance;
        private int second = 0;
        void Awake()
        {
            FU_instance = new CustomFixedUpdate(OnFixedUpdate,30);
        }
        void Update()
        {
            FU_instance.Update();
        }
        void OnFixedUpdate(float dt)
        {
            second++;
        }

        public void StartNewTest(String name)
        {
            outputList.Add(second, new OutputData(second, name));
        }

        public void AddOrbitTrajectory(int orbitId, Vector2 trajectory, double minCorrelation)
        {
            if (outputList.ContainsKey(second))
            {
                (outputList[second] as OutputData).AddOrbit(orbitId, trajectory,minCorrelation);
            }
            else
            {
                outputList.Add(second, new OutputData(second, orbitId, trajectory, minCorrelation));
            }
        }

        public void AddEyeTrajectory(Vector2 trajectory)
        {
            if (outputList.ContainsKey(second))
            {
                (outputList[second] as OutputData).AddEye(trajectory);
            }
            else
            {
                outputList.Add(second, new OutputData(second, trajectory));
            }
        }

        public void Save()
        {
            String output = "";
            foreach (var oD in outputList.Values)
            {
                output += (oD as OutputData)?.GetOutput();
            }
            StreamWriter writer = new StreamWriter(path + DateTime.Now.ToString("dd-MM-yy_HH:mm"), true);
            writer.Write(output);
            writer.Close();
        }
    }
}