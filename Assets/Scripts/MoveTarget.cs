using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objLeft;
    public GameObject objRight;

    float step = 0.008f;
    GameObject target;
    Vector3 targetPos;

    Vector3 startPos;
    Vector3 endPos;

    Vector3 leftPos;
    Vector3 rightPos;

    void Start()
    {
        target = this.gameObject;
        targetPos = this.gameObject.transform.position;

        leftPos = objLeft.transform.position;
        rightPos = objRight.transform.position;

        this.gameObject.transform.position = leftPos;
        endPos = rightPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPos != endPos)
        {
            targetPos = Vector3.MoveTowards(targetPos, endPos, step);
            this.gameObject.transform.position = targetPos;
        } else if (targetPos == endPos)
        {
            if (targetPos == leftPos)
            {
                endPos = rightPos;
            }
            else
            {
                endPos = leftPos;
            }

        }
    }
}
