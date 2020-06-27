using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RemoveEquip : MonoBehaviour, IPointerDownHandler
{

    private GameObject lastObject;
    private float invoketime;
    private float interval = 3f;


    public void OnPointerDown(PointerEventData eventData)
    {
//        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        if (eventData.pointerCurrentRaycast.gameObject.name == "EqSlot(Clone)")
        {
            lastObject = eventData.pointerCurrentRaycast.gameObject.transform.GetChild(0).gameObject;
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<EquipSlot>().isEquip)
            { 
                lastObject.SetActive(true);
                invoketime = Time.time;
            }
            else
            {
                lastObject.SetActive(false);
            }
        }
        else if(eventData.pointerCurrentRaycast.gameObject.name == "Removetext")
        {
            //            Debug.Log(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
            int tempitemid = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<EquipSlot>().EquipSlotItem.itemid;
            EquipMG.PutEquipToBag(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<EquipSlot>().Eqslotid, tempitemid);
            InventoryMG.MGAddToBag(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<EquipSlot>().EquipSlotItem, 0);
        }
        else
        {
            lastObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - invoketime > interval&& lastObject!=null)
        {
            lastObject.SetActive(false);
        }
    }
}
