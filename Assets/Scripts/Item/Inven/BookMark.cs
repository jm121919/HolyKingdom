using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum BookMarkType
{
    None,
    Equip,
    Info
}
public class BookMark : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public BookMarkType type;
    public Dictionary<BookMarkType, GameObject> bookMarkTypeDic;
    private RectTransform bookMarkRectTransform;
    public RectTransform iconRectTransform;
    private void Start()
    {
        bookMarkTypeDic = new Dictionary<BookMarkType, GameObject>()
        {
            { BookMarkType.Equip, UIManager.instance.equipUIObject },
            { BookMarkType.Info, UIManager.instance.InfoUIObject }
        };
        bookMarkRectTransform = GetComponent<RectTransform>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        foreach(var dicGameObject in bookMarkTypeDic.Values)
        {
            dicGameObject.SetActive(false);
        }
        bookMarkTypeDic[type].SetActive(true);//ÀÌÀüÈ°¼ºÈ­²ô°í Áö±Ý²¨ Å°±â
    }

    public void OnPointerDown(PointerEventData eventData)//¹öÆ° ±ôºýÀÓ ±¸Çö
    {
        bookMarkRectTransform.sizeDelta = new Vector2(90, 40);
        iconRectTransform.sizeDelta = new Vector2(30, 30);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bookMarkRectTransform.sizeDelta = new Vector2(100, 50);
        iconRectTransform.sizeDelta = new Vector2(40, 40);
    }
}
