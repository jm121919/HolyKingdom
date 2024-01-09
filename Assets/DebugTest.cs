using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugTest : MonoBehaviour
{
    TextMeshProUGUI debugText;
    public static TextMeshProUGUI debugTextPro;
    // Start is called before the first frame update
    void Start()
    {
        debugText = GetComponent<TextMeshProUGUI>();
        debugTextPro = debugText; 
    }
    public static void ShowDebug(string text)
    {
        debugTextPro.text = text;
    }

 
}
