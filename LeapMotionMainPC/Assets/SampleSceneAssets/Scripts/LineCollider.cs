using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollider : MonoBehaviour
{
    public Transform StartNode;
    public Transform EndNode;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(StartNode.position, EndNode.position); //Change Scale
        transform.localScale = new Vector3(0.25f, 0.25f, distance);

        Vector3 middlePoint = (StartNode.position + EndNode.position) / 2; //Change Position
        transform.position = middlePoint;

        Vector3 rotationDirection = (EndNode.position - StartNode.position); //Change Rotation
        transform.rotation = Quaternion.LookRotation(rotationDirection);
    }
}
