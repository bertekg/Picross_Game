using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TilesMenager : MonoBehaviour
{
    public Canvas canvasData;
    public Image imagePrefab;
    public int intSizeWidth = 5;
    public int intSizeHeight = 5;
    public GameObject panelCheck;
    public Text textInfoError;
    public Text textInfoToFin;
    public GameObject panelInfoFinish;
    private GameObject panel;
    private int intCurrentSelectX = 0;
    private int intCurrentSelectY = 0;
    private int intCurrentSelectColor = 0;
    private LoadFromText.GameLevel currGameLevel = new LoadFromText.GameLevel();
    private LoadFromText.ConvertedColors currConvertedLevel;
    public enum TileModyfieMode { none, Mark, NoMark, Clear };
    TileModyfieMode tmmCurrent = TileModyfieMode.none;
    // Use this for initialization
    void Start()
    {
        panel = new GameObject();
        panel.name = "MainPanel";
        panel.AddComponent<CanvasRenderer>();
        panel.AddComponent<RectTransform>();
        panel.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.25f);
        panel.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.9f);
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        panel.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        float sHeight = 0.65f * (float)Screen.height;
        float sPanelWidth = (float)Screen.width;
        float fOffsetHeight = 0, fOffsetWidth = 0;
        float fBasicTileScale = 0;
        if (sHeight / intSizeHeight >= sPanelWidth / intSizeWidth)
        {
            fBasicTileScale = sPanelWidth / intSizeWidth;
            fOffsetHeight = sHeight - (fBasicTileScale * intSizeHeight);
        }
        else
        {
            fBasicTileScale = sHeight / intSizeHeight;
            fOffsetWidth = sPanelWidth - (fBasicTileScale * intSizeWidth);
        }
        panel.AddComponent<Image>();
        panel.transform.SetParent(canvasData.transform, false);
        for (int i = 0; i < intSizeWidth; i++)
        {
            for (int j = 0; j < intSizeHeight; j++)
            {
                Image newImage = (Image)Instantiate(imagePrefab);
                newImage.name = "Tille_" + i.ToString() + "_" + j.ToString();
                //newImage.color = Color.red;
                newImage.transform.position = new Vector3((fOffsetWidth / 2) + i * fBasicTileScale, (fOffsetHeight / 2) + j * fBasicTileScale);
                newImage.GetComponent<RectTransform>().sizeDelta = new Vector2(fBasicTileScale, fBasicTileScale);
                newImage.transform.SetParent(panel.transform, false);
            }
        }
    }
    public void SetLevelDataClass(LoadFromText.GameLevel newGameLevel)
    {
        currGameLevel = newGameLevel;
        currConvertedLevel = new LoadFromText.ConvertedColors(currGameLevel);
        for(int i = 0; i < currGameLevel.listLevelTilesOrder.Count; i++)
        {
            LocationData ldTemp = new LocationData();
            ldTemp.bytePosX = currGameLevel.listLevelTilesOrder[i].tileLocationX;
            ldTemp.bytePosY = currGameLevel.listLevelTilesOrder[i].tileLocationY;
            listOfCellsToFinish.Add(ldTemp);
        }
        UpdateColorSelection(0);
        UpadtePosCursor(0, 0);
    }
    string sObjectName = "";
    // Update is called once per frame
    private void ChangeColorFrame(Color cToColor)
    {
        sObjectName = "Tille_" + intCurrentSelectX.ToString() + "_" + intCurrentSelectY.ToString();
        GameObject newImage = GameObject.Find(sObjectName);
        if (newImage != null)
        {
            newImage.GetComponent<Image>().color = cToColor;
        }
    }
    public void UpadtePosCursor(int x, int y)
    {
        Color insideColr = new Color();
        sObjectName = "Tille_" + intCurrentSelectX.ToString() + "_" + intCurrentSelectY.ToString();
        GameObject newImage = GameObject.Find(sObjectName);
        if (newImage != null)
        {
            insideColr = newImage.GetComponent<SetColorInside>().GetColor();
        }
        ChangeColorFrame(insideColr);
        intCurrentSelectX = x;
        intCurrentSelectY = y;
        ChangeColorFrame(currConvertedLevel.colorMarker);
        ChangeCurrentTiles();
    }
    string sColorName = "";
    public void UpdateColorSelection(int currColorSelect)
    {
        sColorName = "Color_" + intCurrentSelectColor.ToString();
        GameObject newImage = GameObject.Find(sColorName);
        if (newImage != null)
        {
            newImage.GetComponent<Image>().color = newImage.GetComponent<SetColorInside>().GetColor();
        }
        intCurrentSelectColor = currColorSelect;
        sColorName = "Color_" + intCurrentSelectColor.ToString();
        GameObject newImage2 = GameObject.Find(sColorName);
        if (newImage2 != null)
        {
            newImage2.GetComponent<Image>().color = currConvertedLevel.colorMarker;
        }
    }
    public void ChangeTileModyfieMode(TileModyfieMode newMode)
    {
        tmmCurrent = newMode;
        ChangeCurrentTiles();
    }
    private void ChangeCurrentTiles()
    {
        GameObject newImage;
        switch (tmmCurrent)
        {
            case TileModyfieMode.Mark:
                sObjectName = "Tille_" + intCurrentSelectX.ToString() + "_" + intCurrentSelectY.ToString();
                newImage = GameObject.Find(sObjectName);
                if (newImage != null)
                {
                    newImage.GetComponent<SetColorInside>().SetColor(currConvertedLevel.listPossibleColorsOfTiles[intCurrentSelectColor]);
                    newImage.GetComponent<ShowMark>().SetMark(false);
                }
                if(currGameLevel.listLevelTilesOrder.Any(item => (item.tileLocationX == intCurrentSelectX && item.tileLocationY == intCurrentSelectY)))
                {
                    if(currGameLevel.listLevelTilesOrder.First(item => (item.tileLocationX == intCurrentSelectX && item.tileLocationY == intCurrentSelectY)).colorId == intCurrentSelectColor)
                    {
                        listOfCellsToFinish.RemoveAll(item => (item.bytePosX == intCurrentSelectX && item.bytePosY == intCurrentSelectY));
                        listOfCellsErrors.RemoveAll(item => (item.bytePosX == intCurrentSelectX && item.bytePosY == intCurrentSelectY));
                    }
                    else
                    {
                        LocationData ldTemp = new LocationData();
                        ldTemp.bytePosX = (byte)intCurrentSelectX;
                        ldTemp.bytePosY = (byte)intCurrentSelectY;
                        if (!listOfCellsErrors.Any(item => (item.bytePosX == intCurrentSelectX && item.bytePosY == intCurrentSelectY)))
                        {                            
                            listOfCellsErrors.Add(ldTemp);
                        }
                        if (!listOfCellsToFinish.Any(item => (item.bytePosX == intCurrentSelectX && item.bytePosY == intCurrentSelectY)))
                        {
                            listOfCellsToFinish.Add(ldTemp);
                        }
                    }
                }
                else
                {                    
                    if(!listOfCellsErrors.Any(item => (item.bytePosX == intCurrentSelectX && item.bytePosY == intCurrentSelectY)))
                    {
                        LocationData ldTemp = new LocationData();
                        ldTemp.bytePosX = (byte)intCurrentSelectX;
                        ldTemp.bytePosY = (byte)intCurrentSelectY;
                        listOfCellsErrors.Add(ldTemp);
                    }                    
                }
                if (listOfCellsErrors.Count == 0 && listOfCellsToFinish.Count == 0)
                {
                    ShowFinish();
                }
                break;
            case TileModyfieMode.NoMark:
                sObjectName = "Tille_" + intCurrentSelectX.ToString() + "_" + intCurrentSelectY.ToString();
                newImage = GameObject.Find(sObjectName);
                if (newImage != null)
                {
                    newImage.GetComponent<SetColorInside>().SetColor(currConvertedLevel.colorTilesNeutral);
                    newImage.GetComponent<ShowMark>().SetMark(true);
                }
                listOfCellsErrors.RemoveAll(item => (item.bytePosX == intCurrentSelectX && item.bytePosY == intCurrentSelectY));
                if (currGameLevel.listLevelTilesOrder.Any(item => (item.tileLocationX == intCurrentSelectX && item.tileLocationY == intCurrentSelectY)))
                {
                    if (!listOfCellsErrors.Any(item => (item.bytePosX == intCurrentSelectX && item.bytePosY == intCurrentSelectY)))
                    {
                        LocationData ldTemp = new LocationData();
                        ldTemp.bytePosX = (byte)intCurrentSelectX;
                        ldTemp.bytePosY = (byte)intCurrentSelectY;
                        listOfCellsErrors.Add(ldTemp);
                    }
                }
                break;
            case TileModyfieMode.Clear:
                sObjectName = "Tille_" + intCurrentSelectX.ToString() + "_" + intCurrentSelectY.ToString();
                newImage = GameObject.Find(sObjectName);
                if (newImage != null)
                {
                    newImage.GetComponent<SetColorInside>().SetColor(currConvertedLevel.colorTilesNeutral);
                    newImage.GetComponent<ShowMark>().SetMark(false);
                }
                listOfCellsErrors.RemoveAll(item => (item.bytePosX == intCurrentSelectX && item.bytePosY == intCurrentSelectY));
                if (currGameLevel.listLevelTilesOrder.Any(item => (item.tileLocationX == intCurrentSelectX && item.tileLocationY == intCurrentSelectY)))
                {
                    if(!listOfCellsToFinish.Any(item => (item.bytePosX == intCurrentSelectX && item.bytePosY == intCurrentSelectY)))
                    {
                        LocationData ldTemp = new LocationData();
                        ldTemp.bytePosX = (byte)intCurrentSelectX;
                        ldTemp.bytePosY = (byte)intCurrentSelectY;
                        listOfCellsToFinish.Add(ldTemp);
                    }
                }
                break;
        }
    }
    public void CheckLevel()
    {
        textInfoError.text = listOfCellsErrors.Count.ToString();
        textInfoToFin.text = listOfCellsToFinish.Count.ToString();
        canvasData.enabled = false;
        panelCheck.SetActive(true);
    }
    public void CloseCheck()
    {
        canvasData.enabled = true;
        panelCheck.SetActive(false);
    }
    public void FixLevel()
    {
        canvasData.enabled = true;
        panelCheck.SetActive(false);
    }
    private List<LocationData> listOfCellsToFinish = new List<LocationData>();
    private List<LocationData> listOfCellsErrors = new List<LocationData>();
    public struct LocationData
    {
        public byte bytePosX { get; set; }
        public byte bytePosY { get; set; }
    }
    public void ShowFinish()
    {
        canvasData.enabled = false;
        panelInfoFinish.SetActive(true);
        GameObject finishTexture = GameObject.Find("RawImageFinish");
        finishTexture.GetComponent<CreateTexture>().MakeTexture(currGameLevel);
    }
}
