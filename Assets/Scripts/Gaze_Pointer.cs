using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Tobii.Research.Unity;

public class Gaze_Pointer : Pointer 
{

	// Use this for initialization
	void Start ()
	{
		Tracking_Data_Manager.OnNewProcessedGazeRay += Point;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
