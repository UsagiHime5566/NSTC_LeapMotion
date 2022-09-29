using System.Collections;
using System.Collections.Generic;
using Leap;
using Leap.Unity;
using UnityEngine;

public class PointerLogic : MonoBehaviour
{
    public GameObject _rayPoint;

    void Start()
    {
        GetComponent<LineRenderer>().enabled = false;
        _rayPoint.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //指
    public void Pointer(Hand hand)
    {
        _rayPoint.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        RayCast(hand);
    }

    //沒指
    public void UnPointer()
    {
        _rayPoint.transform.localScale = new Vector3(0f, 0f, 0f);
        GetComponent<LineRenderer>().enabled = false;
    }

    private void RayCast(Hand hand)
    {

        //手腕 Vector3
        Vector3 wrist = hand.Fingers[1].Bone(Bone.BoneType.TYPE_METACARPAL).PrevJoint.ToVector3();
        //指尖 Vector3
        Vector3 fingertip = hand.Fingers[1].Bone(Bone.BoneType.TYPE_DISTAL).NextJoint.ToVector3();
        Vector3 dir = fingertip - wrist;
        Vector3.Normalize(dir);

        Ray ray = new Ray(fingertip, dir);
        RaycastHit hit;
        LineRenderer lineRender = GetComponent<LineRenderer>();

        if (Physics.Raycast(ray, out hit))
        {
            lineRender.enabled = true;
            lineRender.SetPosition(0, fingertip);
            lineRender.SetPosition(1, hit.point);
            _rayPoint.transform.position = new Vector3(hit.point.x, hit.point.y, hit.transform.position.z);
        }
        else
        {
            lineRender.enabled = false;
        };
    }

}
