using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonUpdater : MonoBehaviour
{
    public int iMaxX = 5;
    public int iMaxY = 5;
    public int iColorCount = 1;
    public Text tCurrPosX, tCurrPosY;
    private int iCurrPosX = 1;
    private int iCurrPosY = 1;
    private int iCurrentColor = 1;
    public GameObject TilesMenager;
    private void Start()
    {
        UpdateDebugDataX();
        UpdateDebugDataY();
    }
    private void UpdateDebugDataX()
    {
        tCurrPosX.text = iCurrPosX.ToString();
    }
    private void UpdateDebugDataY()
    {
        tCurrPosY.text = iCurrPosY.ToString();
    }
    public void ButtonUpPress()
    {
        iCurrPosY += 1;
        if (iCurrPosY > iMaxY)
        {
            iCurrPosY = 1;
        }
        UpdateDebugDataY();
        TilesMenager.GetComponent<TilesMenager>().UpadtePosCursor(iCurrPosX - 1, iCurrPosY - 1);
    }
    public void ButtonDownPress()
    {
        iCurrPosY -= 1;
        if (iCurrPosY < 1)
        {
            iCurrPosY = iMaxY;
        }
        UpdateDebugDataY();
        TilesMenager.GetComponent<TilesMenager>().UpadtePosCursor(iCurrPosX - 1, iCurrPosY - 1);
    }
    public void ButtonLeftPress()
    {
        iCurrPosX -= 1;
        if (iCurrPosX < 1)
        {
            iCurrPosX = iMaxX;
        }
        UpdateDebugDataX();
        TilesMenager.GetComponent<TilesMenager>().UpadtePosCursor(iCurrPosX - 1, iCurrPosY - 1);
    }
    public void ButtonRightPress()
    {
        iCurrPosX += 1;
        if(iCurrPosX > iMaxX)
        {
            iCurrPosX = 1;
        }
        UpdateDebugDataX();
        TilesMenager.GetComponent<TilesMenager>().UpadtePosCursor(iCurrPosX - 1, iCurrPosY - 1);
    }    
    public void ButtonLeftUp()
    {
        iCurrPosX -= 1;
        if (iCurrPosX < 1)
        {
            iCurrPosX = iMaxX;
        }
        UpdateDebugDataX();
        iCurrPosY += 1;
        if (iCurrPosY > iMaxY)
        {
            iCurrPosY = 1;
        }
        UpdateDebugDataY();
        TilesMenager.GetComponent<TilesMenager>().UpadtePosCursor(iCurrPosX - 1, iCurrPosY - 1);
    }
    public void ButtonLeftDown()
    {
        iCurrPosX -= 1;
        if (iCurrPosX < 1)
        {
            iCurrPosX = iMaxX;
        }
        UpdateDebugDataX();
        iCurrPosY -= 1;
        if (iCurrPosY < 1)
        {
            iCurrPosY = iMaxY;
        }
        UpdateDebugDataY();
        TilesMenager.GetComponent<TilesMenager>().UpadtePosCursor(iCurrPosX - 1, iCurrPosY - 1);
    }
    public void ButtonRightDown()
    {
        iCurrPosX += 1;
        if (iCurrPosX > iMaxX)
        {
            iCurrPosX = 1;
        }
        UpdateDebugDataX();
        iCurrPosY -= 1;
        if (iCurrPosY < 1)
        {
            iCurrPosY = iMaxY;
        }
        UpdateDebugDataY();
        TilesMenager.GetComponent<TilesMenager>().UpadtePosCursor(iCurrPosX - 1, iCurrPosY - 1);
    }
    public void ButtonRightUp()
    {
        iCurrPosX += 1;
        if (iCurrPosX > iMaxX)
        {
            iCurrPosX = 1;
        }
        UpdateDebugDataX();
        iCurrPosY += 1;
        if (iCurrPosY > iMaxY)
        {
            iCurrPosY = 1;
        }
        UpdateDebugDataY();
        TilesMenager.GetComponent<TilesMenager>().UpadtePosCursor(iCurrPosX - 1, iCurrPosY - 1);
    }
    public void ButtonNextColorPress()
    {
        iCurrentColor += 1;
        if (iCurrentColor > iColorCount)
        {
            iCurrentColor = 1;
        }
        TilesMenager.GetComponent<TilesMenager>().UpdateColorSelection(iCurrentColor - 1);
    }
    public void ButtonPreviousColorPress()
    {
        iCurrentColor -= 1;
        if (iCurrentColor < 1)
        {
            iCurrentColor = iColorCount;
        }
        TilesMenager.GetComponent<TilesMenager>().UpdateColorSelection(iCurrentColor - 1);
    }
    public void ButtonMarkDown()
    {
        TilesMenager.GetComponent<TilesMenager>().ChangeTileModyfieMode(global::TilesMenager.TileModyfieMode.Mark);
    }
    public void ButtonNoMarkDown()
    {
        TilesMenager.GetComponent<TilesMenager>().ChangeTileModyfieMode(global::TilesMenager.TileModyfieMode.NoMark);
    }
    public void ButtonClearDown()
    {
        TilesMenager.GetComponent<TilesMenager>().ChangeTileModyfieMode(global::TilesMenager.TileModyfieMode.Clear);
    }
    public void ButtonAllUp()
    {
        TilesMenager.GetComponent<TilesMenager>().ChangeTileModyfieMode(global::TilesMenager.TileModyfieMode.none);
    }    
}