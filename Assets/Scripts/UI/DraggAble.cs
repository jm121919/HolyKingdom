using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggAble : MonoBehaviour,IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        eventData.pointerDrag.gameObject.transform.parent.gameObject.transform.position = Input.mousePosition;
    }

}
