using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace DefaultNamespace
{
    public class TextData
    {
        private int _second;
        private SortedList _orbitStrings = new SortedList();
        private String _eyeString = "";
        private String _testString = "";
        private DataType _type;
        
        public TextData(int second, String name)
        {
            _second = second;
            _testString = $"{second} ===== New Test: {name} ===== \n";
            _type = DataType.Test;
        }
        public TextData(int second, int orbitId, Vector2 trajectory, double minCorrelation)
        {
            _second = second;
            _orbitStrings.Add(orbitId, $"Orbit {orbitId}, Trajectory {trajectory}, Correlation {minCorrelation}");
            _type = DataType.Measurement;
        }
        public TextData(int second, Vector2 trajectory)
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
}