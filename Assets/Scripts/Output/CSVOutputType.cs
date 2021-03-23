using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    
    public class CSVOutputType : IOutputType
    {
        private SortedList<int, CSVData> outputList = new SortedList<int, CSVData>();
        private int _numberOfOrbits = 0;
        private string _currentTestName = "";
        public void StartNewTest(int second, string name)
        {
            if (outputList.ContainsKey(second)) outputList[second].SetTest(name);
            _currentTestName = name;
        }

        public void AddOrbitData(int second, int orbitId, Vector2 pos, double minCorrelation)
        {
            _numberOfOrbits = Math.Max(_numberOfOrbits, orbitId);
            if (outputList.ContainsKey(second))
            {
                outputList[second].AddOrbit(orbitId, pos,minCorrelation);
                return;
            }
            outputList.Add(second, new CSVData(second, _currentTestName, orbitId, pos, minCorrelation));
        }

        public void AddEyeData(int second, Vector2 pos)
        {
            if (outputList.ContainsKey(second))
            {
                outputList[second].SetEye(pos);
                return;
            }
            outputList.Add(second, new CSVData(second, _currentTestName,pos));
            
        }

        public void AddCompareResponse(int second,CompareResponse res)
        {
            if (outputList.ContainsKey(second))
            {
                outputList[second].AddCompareResponse(res);
            }
        }

        public string GetExtention()
        {
            return ".csv";
        }

        public string GetInitialOutput(String del)
        {
            String orbitTitle = "";
            for (int i = 0; i < _numberOfOrbits; i++)
            {
                orbitTitle += $"OrbitID{del}Orbit pos x{del}Orbit pos y{del}Correlation";
            }
            return $"Frame{del}Test{del}Eye pos x{del}Eye pos y{del}{orbitTitle}{del}Test Result\n";
        }

        public string GetOutput(String delimiter)
        {
            String output = "";
            foreach (var csvData in outputList.Values)
            {
                output += csvData.GetOutput(delimiter);
            }

            return output;
        }
    }
}