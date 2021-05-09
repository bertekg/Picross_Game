using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMark : MonoBehaviour
{
    public GameObject MarkText;
    public void SetMark(bool newMarkState)
    {
         MarkText.SetActive(newMarkState);
    }
    public bool GetMark()
    {
        return MarkText.activeSelf;
    }
}
