using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class HangGesture : MonoBehaviour
{
    public GameObject _handModel;

    private PointerLogic _PointerLogic;
    private MeshTrail_2 _leftMeshTrail;
    private MeshTrail_2 _rightMeshTrail;
    private float _deltaVelocity = 0.9f;
    private Finger.FingerType[] _yaGesture = { Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_MIDDLE };
    private Finger.FingerType[] _pointGesture = { Finger.FingerType.TYPE_INDEX };
    private Finger.FingerType[] _spidermanGesture = { Finger.FingerType.TYPE_THUMB, Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_PINKY };

    void Start()
    {
        _PointerLogic = GetComponentInChildren<PointerLogic>();
        _rightMeshTrail = _handModel.GetComponentInChildren<Transform>().Find("LoPoly Rigged Hand Right").
        GetComponentInChildren<Transform>().Find("LoPoly_Hand_Mesh_Right").GetComponentInChildren<MeshTrail_2>();
        _leftMeshTrail = _handModel.GetComponentInChildren<Transform>().Find("LoPoly Rigged Hand Left").
        GetComponentInChildren<Transform>().Find("LoPoly_Hand_Mesh_Left").GetComponentInChildren<MeshTrail_2>();

    }

    void Update()
    {
        Hand _rightHand = _handModel.GetComponentInChildren<Transform>().Find("LoPoly Rigged Hand Right").GetComponent<HandModelBase>().GetLeapHand();
        Hand _LeftHand = _handModel.GetComponentInChildren<Transform>().Find("LoPoly Rigged Hand Left").GetComponent<HandModelBase>().GetLeapHand();

        if (_rightHand == null || _LeftHand == null)
        {
            return;
        }

        if (CheckFingerOpen(_rightHand, _pointGesture))
        {
            _PointerLogic.Pointer(_rightHand);
        }
        else { _PointerLogic.UnPointer(); }

        WaveHand(_rightHand, _rightMeshTrail);
        WaveHand(_LeftHand, _leftMeshTrail);

    }

    private void WaveHand(Hand hand, MeshTrail_2 effectHand)
    {
        if (IsMoveLeft(hand))
        {
            //print("<<");
            effectHand.StartTrail();
        }
        if (IsMoveRight(hand))
        {
            //print(">>");
            effectHand.StartTrail();
        }
        if (IsMoveUp(hand))
        {
            effectHand.StartTrail();
        }
        if (IsMoveDown(hand))
        {
            effectHand.StartTrail();
        }
        if (effectHand.GetComponentInParent<RiggedHand>() == null)
        {
            effectHand._isTrailActiveRef = false;
        }
    }

    private bool CheckFingerOpen(Hand hand, Finger.FingerType[] fingerTypesArr)
    {
        List<Finger> listOfFingers = hand.Fingers;
        float count = 0;
        //????????????
        Finger thumb = listOfFingers[0];
        if ((thumb.TipPosition - hand.PalmPosition).Magnitude < 0.06f)
        {
            for (int i = 0; i < fingerTypesArr.Length; i++)
            {
                //????????????????????????????????????
                if (thumb.Type == fingerTypesArr[i])
                {
                    return false;
                }
                else
                {
                    count++;
                }
            }
        }
        //??????4?????????
        for (int f = 1; f < listOfFingers.Count; f++)
        {
            Finger finger = listOfFingers[f];
            //???????????? ????????????????????????????????????????????????????????????????????????????????????
            if ((finger.TipPosition - hand.PalmPosition).Magnitude < 0.075f)
            {
                for (int i = 0; i < fingerTypesArr.Length; i++)
                {
                    //??????????????????????????????????????????
                    if (finger.Type == fingerTypesArr[i])
                    {
                        return false;
                    }
                    else
                    {
                        count++;
                    }
                }
            }
        }

        // ??????length ???????????? for ??????????????? length ???
        return (count / fingerTypesArr.Length == 5 - fingerTypesArr.Length);
    }

    //?????????
    private bool IsMoveLeft(Hand hand)   // ???????????????
    {
        //x??????????????????   _deltaVelocity = 0.7f   
        return hand.PalmVelocity.x < -_deltaVelocity;
    }

    //?????????
    private bool IsMoveRight(Hand hand)
    {
        return hand.PalmVelocity.x > _deltaVelocity;
    }

    //?????????
    private bool IsMoveUp(Hand hand)
    {
        return hand.PalmVelocity.y > _deltaVelocity;
    }

    //?????????
    private bool IsMoveDown(Hand hand)
    {
        return hand.PalmVelocity.y < -_deltaVelocity;
    }
}
