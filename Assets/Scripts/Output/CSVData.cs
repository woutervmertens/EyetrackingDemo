using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class CSVData
    {
        private int _frame;
        private string _test = "";
        private string _testResult = "";
        private string _eyePos = "";
        private SortedList<int,OrbitInfo> _orbitsInfos = new SortedList<int,OrbitInfo>();
        public CSVData(int frame, string test, int orbitId, Vector2 pos, double minCorrelation)
        {
            _frame = frame;
            _test = test;
            _orbitsInfos.Add(orbitId, new OrbitInfo(orbitId,  vec2toStr(pos),minCorrelation.ToString()));
        }
        public CSVData(int frame, string test, Vector2 pos)
        {
            _frame = frame;
            _test = test;
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
        public void SetEye(Vector2 eyePos)
        {
            _eyePos = vec2toStr(eyePos);
        }
        public void AddOrbit(int orbitId, Vector2 pos, double minCorrelation)
        {
            if (_orbitsInfos.ContainsKey(orbitId))
            {
                _orbitsInfos[orbitId] = new OrbitInfo(orbitId, vec2toStr(pos), minCorrelation.ToString());
            }
            else
            {
                _orbitsInfos.Add(orbitId, new OrbitInfo(orbitId, vec2toStr(pos), minCorrelation.ToString()));
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
        
        public String GetOutput(string del)
        {
            string outstr = "";
            outstr += $"{_frame}{del}{_test}{del}{_eyePos}{del}";
            foreach (OrbitInfo info in _orbitsInfos.Values)
            {
                outstr += $"{info.orbitId}{del}{info.position}{del}{info.correlation}{del}";
            }

            outstr += $"{_testResult}\n";
            return outstr;
        }
        
    }
}