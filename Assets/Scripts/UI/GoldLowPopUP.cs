using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldLowPopUP : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("SetActiveFalse", 1f);
    }

    void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
