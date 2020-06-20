using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchPanel : MonoBehaviour, IPointerDownHandler
{
    private int whicnpanel = 1;
    public GameObject selectPanel;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "SelectBt")
        {
            selectPanel.transform.GetChild(whicnpanel).gameObject.SetActive(false);
            if (whicnpanel == 0)
                whicnpanel = 1;
            else
                whicnpanel = 0;
            selectPanel.transform.GetChild(whicnpanel).gameObject.SetActive(true);
        }
        ShopMG.chooseBagReflash(whicnpanel + 1);
    }
}
