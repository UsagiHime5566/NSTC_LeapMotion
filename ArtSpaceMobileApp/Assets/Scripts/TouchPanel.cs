using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Canvas parentCanvas;
    public IconFish Prefab_fish;
    public float CreatePeriod = 0.2f;
    Image ptouch;
    public Vector2 posInPanel;
    public Image ball;

    bool createFish = false;
    float lastCreateTime;

    void Awake(){
        ptouch = GetComponent<Image>();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("down");
        createFish = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("up");
        createFish = false;
    }

    void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform, Input.mousePosition,
            parentCanvas.worldCamera,
            out posInPanel);

        if(ball) (ball.transform as RectTransform).anchoredPosition = posInPanel;

        if(createFish){
            if((Time.time - lastCreateTime) > CreatePeriod){
                lastCreateTime = Time.time;

                var fish = Instantiate(Prefab_fish, parentCanvas.transform);

                fish.rectTransform.anchoredPosition = posInPanel;
            }
        }
    }
}
