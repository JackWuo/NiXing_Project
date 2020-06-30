using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveBag : MonoBehaviour, IDragHandler
{
    public Canvas canvas;
    RectTransform currrect;

    public void OnDrag(PointerEventData eventData)
    {
        currrect.anchoredPosition += eventData.delta;
    }


    private void Awake()
    {
        currrect = GetComponent<RectTransform>();
    }


    private void Start()
    {
        gameObject.SetActive(false);
    }
}
