using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fish : MonoBehaviour
{
    public SpriteRenderer image;
    public float moveDistance = 0.15f;
    public float moveTime = 3;
    public float sizeRand = 2;

    void Start()
    {
        float faceAngle = Random.Range(0f, Mathf.PI * 2);
        Vector3 toPoint = transform.position + new Vector3(moveDistance * Mathf.Cos(faceAngle), 0, moveDistance * Mathf.Sin(faceAngle));

        //Debug.Log("" + faceAngle + ", cpos:" + transform.position + ", toPos:" + toPoint);

        //transform.localEulerAngles = new Vector3(0, (faceAngle/(Mathf.PI*2)) * 360, 0);

        transform.LookAt(toPoint);
        transform.DOMove(toPoint, moveTime).SetEase(Ease.Linear).OnComplete(() => {
            Destroy(gameObject, 0.1f);
        });
        
        image.DOFade(0, moveTime);

        transform.DOShakeRotation(moveTime, 35, 5);

        transform.localScale = transform.localScale * Random.Range(1f, sizeRand);
    }
}
