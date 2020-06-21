using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyGoods : MonoBehaviour
{
    public GameObject WarningPanel;

    private int thisitemprice;
    private int hasedcoin;

    public void Buy()
    {
        if (thisitemprice < hasedcoin)
        {
            hasedcoin -= thisitemprice;

        }
    }
}
