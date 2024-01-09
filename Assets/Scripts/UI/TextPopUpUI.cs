using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TextType
{
    damage,
    gold
}
public class TextPopUpUI : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public TextType type;
    public int InitCount;
}