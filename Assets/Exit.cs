using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private bool Changed = false;
    private void OnTriggerStay2D(Collider2D collider)
    {
        if(!Changed)
        if (collider.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.sceneLevel++;
            GameManager.Instance.reStart();
                Changed = true;
        }
    }
}
