using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform origintransform;

    public Inventory Equipbag;
    public Inventory Goodsbag;
    private bool isequipbag;
    private int originslotid;

    private int DragitemId;
    public void OnBeginDrag(PointerEventData eventData)
    {
        origintransform = transform.parent;
        DragitemId = origintransform.GetComponent<Slot>().slotitem.itemid;
        isequipbag = origintransform.GetComponent<Slot>().slotitem.isequip;

        originslotid = origintransform.GetComponent<Slot>().slotid;
//        Debug.Log(transform.parent);
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;
        transform.GetChild(1).gameObject.SetActive(false);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        transform.GetChild(1).gameObject.SetActive(false);
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "itemimg")
        {
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;
            eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position = origintransform.position;
            eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(origintransform);

            int currid = transform.gameObject.GetComponentInParent<Slot>().slotid;
            if (isequipbag)
            {
                SwapInBag(Equipbag, originslotid, currid, 0);
                InventoryMG.reflashItem(Equipbag);
            }
            else
            {
                SwapInBag(Goodsbag, originslotid, currid, 0);
                InventoryMG.reflashItem(Goodsbag);
            }
//            Debug.Log(eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotid);

            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.GetChild(1).gameObject.SetActive(true);
            return;
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "Slot(Clone)")
        {
            int tempid = eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotid;
            if (isequipbag)
            {
                SwapInBag(Equipbag, originslotid, tempid, 1);
                InventoryMG.reflashItem(Equipbag);
            }
            else
            {
                SwapInBag(Goodsbag, originslotid, tempid, 1);
                InventoryMG.reflashItem(Goodsbag);
            }
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
            eventData.pointerCurrentRaycast.gameObject.transform.GetChild(0).position = origintransform.position;
            eventData.pointerCurrentRaycast.gameObject.transform.GetChild(0).SetParent(origintransform);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.GetChild(1).gameObject.SetActive(true);
            return;
        }
        else
        {
            if (isequipbag && eventData.pointerCurrentRaycast.gameObject.name == "EqSlot(Clone)")
            {
                int tempitemid = origintransform.GetComponent<Slot>().slotitem.itemid;
                int tempslotid = eventData.pointerCurrentRaycast.gameObject.GetComponent<EquipSlot>().Eqslotid + 1;
                PutToEquipPos(origintransform.GetComponent<Slot>().slotitem, tempitemid, tempslotid);
            }

            transform.SetParent(origintransform);
            transform.position = origintransform.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void SwapInBag(Inventory bag, int originid, int curid, int sign)
    {
        if (sign == 0)
        {
            var temp = bag.itemlist[originid];
            bag.itemlist[originid] = bag.itemlist[curid];
            bag.itemlist[curid] = temp;
        }else if (sign == 1)
        {
            bag.itemlist[curid] = bag.itemlist[originid];
            if (curid != originid)
            {
                bag.itemlist[originid] = null;
            }
        }
    }


    private void PutToEquipPos(item thisitem, int itemid, int slotid)
    {
        if (itemid == slotid)
        {
            item RemovedEquip = EquipMG.GetEquipFromBag(thisitem);
            if (RemovedEquip != null)
            {
                InventoryMG.MGAddToBag(RemovedEquip, 0);
            }
            thisitem.itemHeld -= 1;
            InventoryMG.reflashbag(0);
        }
    }
}
