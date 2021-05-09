using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColorInside : MonoBehaviour
{
    public UnityEngine.UI.Image image;
    public void SetColor(Color color)
    {
        image.color = color;
    }
    public Color GetColor()
    {
        return image.color;
    }
}
