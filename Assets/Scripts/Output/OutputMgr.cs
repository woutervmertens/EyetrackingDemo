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
        internal string position { get; }
        internal string correlation{ get; }

        public OrbitInfo(int id, string pos, string corr)
        {
            orbitId = id;
            position = pos;
            correlation = corr;
        }
    }

    public enum OutputType
    {
        TEXT,
        CSV,
        NO_OUTPUT
    }
    
    public class OutputMgr : MonoBehaviour
    {
        private static OutputMgr instance = null;
        public static OutputMgr Instance { get { return (instance != null) ? instance : (instance = new GameObject("OutputMgr").AddComponent<OutputMgr>()); } }

        public string path = "Assets/TestData/";
        private SortedList outputList = new SortedList();

        private CustomFixedUpdate FU_instance;
        private int second = 0;
        private IOutputType _outputType = new CSVOutputType();
        private bool _saveData;
        private string del = "\t";
        private string _subjectID;
        private string _testSaveName;
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

        public void SetOutputType(IOutputType type)
        {
            _outputType = type;
        }

        public void SetSubjectID(String subjectID)
        {
            _subjectID = subjectID;
        }

        public void StartNewTest(String name)
        {
            _outputType.StartNewTest(second,name);
            _testSaveName = name;
        }

        public void AddOrbitData(int orbitId, Vector2 pos, double minCorrelation)
        {
            _outputType.AddOrbitData(second,orbitId,pos,minCorrelation);
        }

        public void AddEyeData(Vector2 pos)
        {
            _outputType.AddEyeData(second,pos);
        }
        public void AddCompareResponse(CompareResponse res)
        {
            _outputType.AddCompareResponse(second,res);
        }
        private void OnDestroy()
        {
            if (_saveData) OutputMgr.Instance.Save();
        }

        public void Activate(bool b) { _saveData = b; }

        public void Save()
        {
            if (!_saveData) return;
            String output = _outputType.GetInitialOutput(del);
            String ext = _outputType.GetExtention();
            output += _outputType.GetOutput(del);
            
            var file = File.Open(path + _subjectID + "_" + _testSaveName + ext, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter writer = new StreamWriter(file);
            writer.Write(output);
            writer.Close();
        }
    }
}