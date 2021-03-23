using System;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IOutputType
    {
        void StartNewTest(int second,String name);
        void AddOrbitData(int second,int orbitId, Vector2 pos, double minCorrelation);
        void AddEyeData(int second, Vector2 pos);
        void AddCompareResponse(int second,CompareResponse res);
        String GetExtention();
        String GetInitialOutput(String delimiter);
        String GetOutput(String delimiter);
    }
}