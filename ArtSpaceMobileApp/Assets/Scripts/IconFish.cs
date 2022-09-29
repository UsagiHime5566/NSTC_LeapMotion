using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IconFish : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image image;
    public float moveDistance = 15f;
    public float moveTime = 3;
    public float sizeRand = 2;

    void Start()
    {
        float faceAngle = Random.Range(0f, Mathf.PI * 2);
        Vector2 toPoint = (transform as RectTransform).anchoredPosition + new Vector2(moveDistance * Mathf.Cos(faceAngle), moveDistance * Mathf.Sin(faceAngle));

        transform.localEulerAngles = new Vector3(0, 0, (faceAngle/(Mathf.PI*2)) * 360);
        (transform as RectTransform).DOAnchorPos(toPoint, moveTime).SetEase(Ease.Linear).OnComplete(() => {
            Destroy(gameObject, 0.1f);
        });
        
        image.DOFade(0, moveTime);

        transform.DOShakeRotation(moveTime);

        transform.localScale = transform.localScale * Random.Range(1f, sizeRand);
    }
}
