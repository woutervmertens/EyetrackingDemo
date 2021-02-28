using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour {

	public float rayRadius = 0.05f;
	public float rayDistance = 100f;
	public int rayLayerMask = 1 << 8;

	public GameObject pointedObject 
	{
		get 
		{ 
			return privPointedObject;
		}
		set 
		{
			if (privPointedObject == value) 
			{
				return;
			}
			privPointedObject = value;
			if (OnNewPointedObject != null) 
			{
				OnNewPointedObject (privPointedObject);
			}
		}
	}
	private GameObject privPointedObject;
	public delegate void NewPointedObjectEventHandler(GameObject newPointedObject);
	public event NewPointedObjectEventHandler OnNewPointedObject;

	public Ray pointingRay
	{
		get
		{ 
			return privPointingRay;
		}
		set
		{ 
			privPointingRay = value;
			if (OnNewPointingRay != null)
			{
				OnNewPointingRay (privPointingRay);
			}
		}
	}
	private Ray privPointingRay;
	public delegate void NewPointingRayEventHandler(Ray newPointingRay);
	public event NewPointingRayEventHandler OnNewPointingRay;


	

	public Vector3 hitPosition
	{
		get
		{ 
			return privHitPosition;
		}
		set
		{ 
			privHitPosition = value;
			if (OnNewHitPosition != null)
			{
				OnNewHitPosition (privHitPosition);
			}
		}
	}
	private Vector3 privHitPosition;
	public delegate void NewHitPositionEventHandler(Vector3 newHitPosition);
	public event NewHitPositionEventHandler OnNewHitPosition;

	public Vector3 screenPosition
	{
		get
		{ 
			return privScreenPosition;
		}
		set
		{ 
			privScreenPosition = value;
			if (OnNewScreenPosition != null)
			{
				OnNewScreenPosition (privScreenPosition);
			}
		}
	}
	private Vector3 privScreenPosition;
	public delegate void NewScreenPositionEventHandler(Vector3 newScreenPosition);
	public event NewScreenPositionEventHandler OnNewScreenPosition;

	public Vector3 pointingDirection
	{
		get
		{ 
			return privPointingDirection;
		}
		set
		{ 
			privPointingDirection = value;
			if (OnNewPointingDirection != null)
			{
				OnNewPointingDirection (privPointingDirection);
			}
		}
	}
	private Vector3 privPointingDirection;
	public delegate void NewPointingDirectionEventHandler(Vector3 newPointingDirection);
	public event NewPointingDirectionEventHandler OnNewPointingDirection;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Point(Ray ray)
	{
		pointingRay = ray;
		RaycastHit hit;
		float hitDistance = 0;

		//sphere moves along ray and returns hits
		if (Physics.SphereCast (ray, rayRadius, out hit, rayDistance, rayLayerMask)) 
		{
			pointedObject = hit.collider.gameObject;
			hitPosition = hit.point;
			hitDistance = hit.distance;
		} 
		else 
		{
			pointedObject = null;
			hitPosition = ray.GetPoint (rayDistance / 100f);
			hitDistance = rayDistance / 100f;
		}
		pointingDirection = ray.direction;
		UpdateScreenPosition(ray, hitDistance);
	}

	private void UpdateScreenPosition (Ray ray, float distance)
	{
		screenPosition = Camera.main.WorldToScreenPoint(ray.GetPoint(distance));
	}
}
