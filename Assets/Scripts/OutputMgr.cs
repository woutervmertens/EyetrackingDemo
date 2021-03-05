using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace DefaultNamespace
{
    enum DataType
    {
        Test,
        Measurement
    }

    struct OrbitInfo
    {
        internal int orbitId { get; }
        internal string normalised{ get; }
        internal string position { get; }
        internal string correlation{ get; }

        public OrbitInfo(int id, string norm, string pos, string corr)
        {
            orbitId = id;
            normalised = norm;
            position = pos;
            correlation = corr;
        }
    }
    class CSVData
    {
        private int _frame;
        private string _test = "";
        private string _testResult = "";
        private string _eyeTrajectory = "";
        private string _eyePos = "";
        private SortedList<int,OrbitInfo> _orbitsInfos = new SortedList<int,OrbitInfo>();
        public CSVData(int frame, string test, int orbitId, Vector2 norm, Vector2 pos, double minCorrelation)
        {
            _frame = frame;
            _test = test;
            _orbitsInfos.Add(orbitId, new OrbitInfo(orbitId, vec2toStr(norm), vec2toStr(pos),minCorrelation.ToString()));
        }
        public CSVData(int frame, string test, Vector2 norm, Vector2 pos)
        {
            _frame = frame;
            _test = test;
            _eyeTrajectory = vec2toStr(norm);
            _eyePos = vec2toStr(pos);
        }

        private String vec2toStr(Vector2 vec)
        {
            return $"{vec.x}\t{vec.y}";
        }

        public void SetTest(string test)
        {
            _test = test;
        }
        public void SetEye(Vector2 eyeNorm, Vector2 eyePos)
        {
            _eyeTrajectory = vec2toStr(eyeNorm);
            _eyePos = vec2toStr(eyePos);
        }
        public void AddOrbit(int orbitId, Vector2 norm, Vector2 pos, double minCorrelation)
        {
            if (_orbitsInfos.ContainsKey(orbitId))
            {
                _orbitsInfos[orbitId] = new OrbitInfo(orbitId, vec2toStr(norm), vec2toStr(pos), minCorrelation.ToString());
            }
            else
            {
                _orbitsInfos.Add(orbitId, new OrbitInfo(orbitId, vec2toStr(norm), vec2toStr(pos), minCorrelation.ToString()));
            }
        }
        public void AddCompareResponse(CompareResponse res)
        {
            switch (res)
            {
                case CompareResponse.N:
                    _testResult = "None";
                    break;
                case CompareResponse.TargetSelected:
                    _testResult = "Target Selected";
                    break;
                case CompareResponse.DummySelected:
                    _testResult = "Wrong Orbit Selected";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(res), res, null);
            }
        }
        /*CSV OUTPUT:
            Frame:  |Test:  |Eye Norm:    |OrbitId:   |Norm:    |Correlation:   |Result: 
         */
        public String GetOutput(string del)
        {
            string outstr = "";
            foreach (OrbitInfo info in _orbitsInfos.Values)
            {
                outstr += $"{_frame}{del}{_test}{del}{_eyeTrajectory}{del}{_eyePos}{del}{info.orbitId}{del}{info.normalised}{del}{info.position}{del}{info.correlation}{del}{_testResult}\n";
            }
            return outstr;
        }
        
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
            _type = DataType.Measurement;
        }
        public OutputData(int second, Vector2 trajectory)
        {
            _second = second;
            _eyeString = $"Eye Trajectory {trajectory}";
            _type = DataType.Measurement;
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

        public void AddCompareResponse(CompareResponse res)
        {
            _type = DataType.Test;
            String str = "";
            switch (res)
            {
                case CompareResponse.N:
                    str = "No target selected.";
                    break;
                case CompareResponse.TargetSelected:
                    str = "The correct orbit was selected.";
                    break;
                case CompareResponse.DummySelected:
                    str = "The wrong orbit was selected.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(res), res, null);
            }
            _testString = $"Test result: {str}\n";
        }

        public String GetOutput()
        {
            switch (_type)
            {
                case DataType.Test:
                    return _testString;
                    break;
                case DataType.Measurement:
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

    public enum OutputType
    {
        TEXT,
        CSV
    }
    
    public class OutputMgr : MonoBehaviour
    {
        private static OutputMgr instance = null;
        public static OutputMgr Instance { get { return instance != null ? instance : (instance = new GameObject("OutputMgr").AddComponent<OutputMgr>()); } }

        public string path = "Assets/TestData/";
        private SortedList outputList = new SortedList();

        private CustomFixedUpdate FU_instance;
        private int second = 0;
        private string currTest = "";
        private OutputType outputType = OutputType.TEXT;
        private bool _saveData;
        private string del = "\t";
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

        public void SetOutputType(OutputType type)
        {
            outputType = type;
        }

        public void StartNewTest(String name)
        {
            switch (outputType)
            {
                case OutputType.TEXT:
                    if(!outputList.ContainsKey(second))outputList.Add(second, new OutputData(second, name));
                    break;
                case OutputType.CSV:
                    if(outputList.ContainsKey(second))(outputList[second] as CSVData).SetTest(name);
                    currTest = name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddOrbitData(int orbitId, Vector2 norm, Vector2 pos, double minCorrelation)
        {
            switch (outputType)
            {
                case OutputType.TEXT:
                    if (outputList.ContainsKey(second))
                    {
                        (outputList[second] as OutputData).AddOrbit(orbitId, norm,minCorrelation);
                    }
                    else
                    {
                        outputList.Add(second, new OutputData(second, orbitId, norm, minCorrelation));
                    }
                    break;
                case OutputType.CSV:
                    if (outputList.ContainsKey(second))
                    {
                        (outputList[second] as CSVData).AddOrbit(orbitId, norm,pos,minCorrelation);
                    }
                    else
                    {
                        outputList.Add(second, new CSVData(second,currTest,orbitId, norm,pos, minCorrelation));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        public void AddEyeData(Vector2 norm,Vector2 pos)
        {
            switch (outputType)
            {
                case OutputType.TEXT:
                    if (outputList.ContainsKey(second))
                    {
                        (outputList[second] as OutputData).AddEye(norm);
                    }
                    else
                    {
                        outputList.Add(second, new OutputData(second, norm));
                    }
                    break;
                case OutputType.CSV:
                    if (outputList.ContainsKey(second))
                    {
                        (outputList[second] as CSVData).SetEye(norm,pos);
                    }
                    else
                    {
                        outputList.Add(second, new CSVData(second, currTest,norm,pos));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
        public void AddCompareResponse(CompareResponse res)
        {
            switch (outputType)
            {
                case OutputType.TEXT:
                    if (outputList.ContainsKey(second))
                    {
                        (outputList[second] as OutputData).AddCompareResponse(res);
                    }
                    break;
                case OutputType.CSV:
                    if (outputList.ContainsKey(second))
                    {
                        (outputList[second] as CSVData).AddCompareResponse(res);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void OnDestroy()
        {
            if (_saveData) OutputMgr.Instance.Save();
        }

        public void Activate(bool b) { _saveData = b; }

        public void Save()
        {
            String output = "";
            String ext = "";
            switch (outputType)
            {
                case OutputType.TEXT:
                    ext = ".txt";
                    break;
                case OutputType.CSV:
                    output = $"Frame{del}Test{del}Eye Norm x{del}Eye Norm y{del}Eye pos x{del}Eye pos y{del}OrbitID{del}Orbit Norm x{del}Orbit Norm y{del}Orbit pos x{del}Orbit pos y{del}Correlation{del}Test Result\n";
                    ext = ".csv";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            foreach (var oD in outputList.Values)
            {
                switch (outputType)
                {
                    case OutputType.TEXT:
                        output += (oD as OutputData)?.GetOutput();
                        break;
                    case OutputType.CSV:
                        output += (oD as CSVData)?.GetOutput(del);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
            }
            var file = File.Open(path + DateTime.Now.ToString("dd-MM-yy_HH-mm") + ext, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter writer = new StreamWriter(file);
            writer.Write(output);
            writer.Close();
        }
    }
}