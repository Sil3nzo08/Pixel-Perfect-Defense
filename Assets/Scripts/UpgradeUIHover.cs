using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class UpgradeUIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData data)
    {
        GameManager.instance.mouseOnUpgradeUI = true;
    }

    public void OnPointerExit(PointerEventData data)
    {
        GameManager.instance.mouseOnUpgradeUI = false;
    }
}
