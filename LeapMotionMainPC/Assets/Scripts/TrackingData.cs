//using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using Leap;
using Leap.Unity;
using UnityEngine;
using UnityEngine.UI;

public class TrackingData : MonoBehaviour
{
    public HandModelBase _handModel;
    public GameObject _rayPoint;
    public Transform _circleRen;
    public bool _pinchRender = false;
    public RectTransform _canvas;
    public Transform _sphere;
    [Range(0.1f, 1f)]
    public float _floatRange;

    private float _deltaVelocity = 0.7f;
    private Finger.FingerType[] _yaGesture = { Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_MIDDLE };
    private Finger.FingerType[] _pointGesture = { Finger.FingerType.TYPE_INDEX };
    private Finger.FingerType[] _spidermanGesture = { Finger.FingerType.TYPE_THUMB, Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_PINKY };
    private RectTransform _pinchDisTex;
    private Text _swipeTex;
    private Text _gestureTex;

    void Start()
    {
        _pinchDisTex = _canvas.transform.GetChild(0).GetComponentInChildren<RectTransform>();
        _swipeTex = _canvas.transform.GetChild(1).GetComponentInChildren<Text>();
        _gestureTex = _canvas.transform.GetChild(2).GetComponentInChildren<Text>();

        GetComponent<LineRenderer>().enabled = false;
        _circleRen.gameObject.SetActive(false);
        _pinchDisTex.GetComponent<Text>().enabled = false;
        //_rayPoint.SetActive(false);
    }

    void Update()
    {
        Hand hand = _handModel.GetLeapHand();
        if (hand == null) return;


        if (IsMoveLeft(hand))
        {
            _swipeTex.text = "Swipe :  ←";
            //print("左手向左滑动");
        }
        if (IsMoveRight(hand))
        {
            _swipeTex.text = "Swipe :  →";
            //print("左手向右滑动");
        }
        if (IsMoveUp(hand))
        {
            _swipeTex.text = "Swipe :  ↑";
            //print("左手向上滑动");
        }
        if (IsMoveDown(hand))
        {
            _swipeTex.text = "Swipe :  ↓";
            //print("左手向下滑动");
        }
        // if (IsCloseHand(hand))
        // {
        //     print("握拳");
        // }
        // if (isOpenHand(hand))
        // {
        //     print("張開手");
        // }

        if (CheckFingerOpen(hand, _yaGesture))
        {
            _gestureTex.text = "Gesture :  YA!!";
            PrintImformation(hand);
            //print("YA");
        }
        else if (CheckFingerOpen(hand, _pointGesture))
        {
            _gestureTex.text = "Gesture :  Point";
            PrintImformation(hand);
            //print("POINT");
        }
        else if (hand.PinchStrength == 1)
        {
            _gestureTex.text = "Gesture :  Pinch";
            PrintImformation(hand);
            //print("PINCH");
        }
        else _gestureTex.text = "Gesture :";


        //print(hand.PalmVelocity.x + ", " + hand.PalmVelocity.y);
        PinchRender(hand);

    }

    //向左滑
    private bool IsMoveLeft(Hand hand)   // 手划向左边
    {
        //x轴移动的速度   _deltaVelocity = 0.7f   
        return hand.PalmVelocity.x < -_deltaVelocity;
    }

    //向右滑
    private bool IsMoveRight(Hand hand)
    {
        return hand.PalmVelocity.x > _deltaVelocity;
    }

    //向上滑
    private bool IsMoveUp(Hand hand)
    {
        return hand.PalmVelocity.y > _deltaVelocity;
    }

    //向下滑
    private bool IsMoveDown(Hand hand)
    {
        return hand.PalmVelocity.y < -_deltaVelocity;
    }

    //判斷握拳
    private bool IsCloseHand(Hand hand)
    {
        List<Finger> listOfFingers = hand.Fingers;
        int count = 0;
        //四根手指
        for (int f = 1; f < listOfFingers.Count; f++)
        {
            Finger finger = listOfFingers[f];
            if ((finger.TipPosition - hand.PalmPosition).Magnitude < 0.05f)
            {
                count++;
            }
        }
        //大拇指
        Finger thumb = listOfFingers[0];
        if ((thumb.TipPosition - hand.PalmPosition).Magnitude < 0.06f)
        {
            count++;
        }
        return (count == 5);
    }

    //判斷張開
    private bool isOpenHand(Hand hand)
    {
        List<Finger> listOfFingers = hand.Fingers;
        int count = 0;
        //四根手指
        for (int f = 1; f < listOfFingers.Count; f++)
        {
            Finger finger = listOfFingers[f];
            if ((finger.TipPosition - hand.PalmPosition).Magnitude > 0.082f)
            {
                count++;
            }
        }
        //大拇指
        Finger thumb = listOfFingers[0];
        if ((thumb.TipPosition - hand.PalmPosition).Magnitude > 0.08f)
        {
            count++;
        }
        return (count == 5);
    }

    //判斷特定手指手勢
    private bool CheckFingerOpen(Hand hand, Finger.FingerType[] fingerTypesArr)
    {
        List<Finger> listOfFingers = hand.Fingers;
        float count = 0;
        //拇指判斷
        Finger thumb = listOfFingers[0];
        if ((thumb.TipPosition - hand.PalmPosition).Magnitude < 0.06f)
        {
            for (int i = 0; i < fingerTypesArr.Length; i++)
            {
                //若拇指緊握，直接跳出方法
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
        //遍歷4根手指
        for (int f = 1; f < listOfFingers.Count; f++)
        {
            Finger finger = listOfFingers[f];
            //判斷手指 「指尖」到「掌心」的長度是否小於值，確定手指是否貼近掌心
            if ((finger.TipPosition - hand.PalmPosition).Magnitude < 0.075f)
            {
                for (int i = 0; i < fingerTypesArr.Length; i++)
                {
                    //若指定手指緊握，直接跳出方法
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

        // 除以length 因為每次 for 循環都執行 length 次
        return (count / fingerTypesArr.Length == 5 - fingerTypesArr.Length);
    }

    private void PinchRender(Hand hand)
    {

        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        if (_pinchRender)
        {
            if (!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
                _circleRen.gameObject.SetActive(true);
                _pinchDisTex.GetComponent<Text>().enabled = true;
            }

            lineRenderer.SetPosition(0, hand.Fingers[0].TipPosition.ToVector3());
            lineRenderer.SetPosition(1, hand.Fingers[1].TipPosition.ToVector3());

            //預測捏位置
            //Vector3 pinPos = hand.GetPinchPosition();
            //_rayPoint.transform.position = new Vector3(pinPos.x, pinPos.y, pinPos.z);

            //中點位置
            Vector3 halfDis = (hand.Fingers[1].TipPosition - hand.Fingers[0].TipPosition).ToVector3();
            halfDis /= 2;
            halfDis += hand.Fingers[0].TipPosition.ToVector3();
            _circleRen.gameObject.SetActive(true);
            _circleRen.transform.position = new Vector3(halfDis.x, halfDis.y, halfDis.z);

            //UI Text
            Vector2 texPos = UIPosition.WorldToUI(_canvas, halfDis);
            _pinchDisTex.anchoredPosition = new Vector2(texPos.x + 75, texPos.y);
            Text tex = _pinchDisTex.GetComponent<Text>();
            tex.text = ((int)hand.PinchDistance).ToString();
        }
        else
        {
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                _circleRen.gameObject.SetActive(false);
                _pinchDisTex.GetComponent<Text>().enabled = false;
            }
        }

        // print("力量: " + hand.PinchStrength);
        // print("距離: " + hand.PinchDistance);

    }

    public void Pointer()
    {
        //Debug.Log("指");
        if (!_rayPoint.activeSelf)
        {
            _rayPoint.SetActive(true);
        }
        InvokeRepeating("RayCast", 0, 0.02f);
    }

    public void UnPointer()
    {
        //Debug.Log("不指");
        _rayPoint.SetActive(false);
        GetComponent<LineRenderer>().enabled = false;
        CancelInvoke("RayCast");
    }

    private void RayCast(Hand hand)
    {
        Ray ray = new Ray(hand.Fingers[1].Bone(Bone.BoneType.TYPE_DISTAL).NextJoint.ToVector3(), hand.Fingers[1].Direction.ToVector3());
        RaycastHit hit;
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        if (Physics.Raycast(ray, out hit))
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, hand.Fingers[1].Bone(Bone.BoneType.TYPE_DISTAL).NextJoint.ToVector3());
            lineRenderer.SetPosition(1, hit.point);
            _rayPoint.transform.position = new Vector3(hit.point.x, hit.point.y, hit.transform.position.z);
            //print(hit.point);
        }
        else
        {
            lineRenderer.enabled = false;
        };
    }

    private void PrintImformation(Hand hand)
    {
        Finger thumb = hand.Fingers[0];
        Finger index = hand.Fingers[1];
        Finger middle = hand.Fingers[2];
        Finger ring = hand.Fingers[3];
        Finger pinky = hand.Fingers[4];

        Vector3 tipPos = thumb.TipPosition.ToVector3();
        Vector3 tipRot = thumb.Bone(Bone.BoneType.TYPE_DISTAL).Rotation.ToQuaternion().eulerAngles;
        print("Thumb_Position: (" + tipPos.x + ", " + tipPos.y + "," + tipPos.z + ")");
        print("Thumb_Rotation: (" + tipRot.x + ", " + tipRot.y + "," + tipRot.z + ")");

        tipPos = index.TipPosition.ToVector3();
        tipRot = index.Bone(Bone.BoneType.TYPE_DISTAL).Rotation.ToQuaternion().eulerAngles;
        print("Index_Position: (" + tipPos.x + ", " + tipPos.y + "," + tipPos.z + ")");
        print("Index_Rotation: (" + tipRot.x + ", " + tipRot.y + "," + tipRot.z + ")");

        tipPos = middle.TipPosition.ToVector3();
        tipRot = middle.Bone(Bone.BoneType.TYPE_DISTAL).Rotation.ToQuaternion().eulerAngles;
        print("Middle_Position: (" + tipPos.x + ", " + tipPos.y + "," + tipPos.z + ")");
        print("Middle_Rotation: (" + tipRot.x + ", " + tipRot.y + "," + tipRot.z + ")");

        tipPos = ring.TipPosition.ToVector3();
        tipRot = ring.Bone(Bone.BoneType.TYPE_DISTAL).Rotation.ToQuaternion().eulerAngles;
        print("Ring_Position: (" + tipPos.x + ", " + tipPos.y + "," + tipPos.z + ")");
        print("Ring_Rotation: (" + tipRot.x + ", " + tipRot.y + "," + tipRot.z + ")");

        tipPos = pinky.TipPosition.ToVector3();
        tipRot = pinky.Bone(Bone.BoneType.TYPE_DISTAL).Rotation.ToQuaternion().eulerAngles;
        print("Pinky_Position: (" + tipPos.x + ", " + tipPos.y + "," + tipPos.z + ")");
        print("Pinky_Rotation: (" + tipRot.x + ", " + tipRot.y + "," + tipRot.z + ")");
    }
}
