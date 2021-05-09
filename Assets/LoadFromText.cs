using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

public class LoadFromText : MonoBehaviour
{
    public Canvas canvasData;
    public Image imagePrefab;
    public Text textPrefab;
    public GameObject colorPanel;
    private GameObject panel;
    public GameObject controllMenager;
    private GameLevel currGameLevel = new GameLevel();
    public Color GetColorOfMarker()
    {
        return new Color(((float)currGameLevel.colorMarker.colR) / 255, ((float)currGameLevel.colorMarker.colG) / 255, ((float)currGameLevel.colorMarker.colB) / 255);
    }
    void Start()
    {
        string sLevelToLoad = PlayerPrefs.GetString("LevelToLoad", "Nothing");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(sLevelToLoad));

        string xmlString = xmlDoc.OuterXml;
        using (StringReader read = new StringReader(xmlString))
        {
            System.Type outType = typeof(GameLevel);

            XmlSerializer serializer = new XmlSerializer(outType);
            using (XmlReader reader = new XmlTextReader(read))
            {
                currGameLevel = (GameLevel)serializer.Deserialize(reader);
                reader.Close();
            }
            read.Close();
        }
        panel = new GameObject();
        panel.name = "MainPanel";
        panel.AddComponent<CanvasRenderer>();
        panel.AddComponent<RectTransform>();
        float anchorMinY = (0.6f * (float)Screen.width) / (float)Screen.height;
        panel.GetComponent<RectTransform>().anchorMin = new Vector2(0, anchorMinY);
        panel.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.94f);
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        panel.GetComponent<RectTransform>().pivot = new Vector2(0, 0);        
        float sPanelHeight = 0.94f *(float)Screen.height - (0.6f * (float)Screen.width);
        float sPanelWidth = (float)Screen.width;
        float fOffsetHeight = 0, fOffsetWidth = 0;
        float fBasicTileScale = 0;
        int iMaxVerticalHint = 0;
        for(int i = 0; i < currGameLevel.listVerticlaNumbersHints.Count; i++)
        {
            if(iMaxVerticalHint < currGameLevel.listVerticlaNumbersHints[i].Count)
            {
                iMaxVerticalHint = currGameLevel.listVerticlaNumbersHints[i].Count;
            }
        }
        int iMaxHorizontalHint = 0;
        for (int i = 0; i < currGameLevel.listHorizontalNumberHints.Count; i++)
        {
            if (iMaxHorizontalHint < currGameLevel.listHorizontalNumberHints[i].Count)
            {
                iMaxHorizontalHint = currGameLevel.listHorizontalNumberHints[i].Count;
            }
        }
        if (sPanelHeight / (currGameLevel.intLevelHeightY + iMaxHorizontalHint) >= sPanelWidth / (currGameLevel.intLevelWidthX + iMaxVerticalHint))
        {
            fBasicTileScale = sPanelWidth / (currGameLevel.intLevelWidthX + iMaxVerticalHint);
            fOffsetHeight = sPanelHeight - (fBasicTileScale * (currGameLevel.intLevelHeightY + iMaxHorizontalHint));
        }
        else
        {
            fBasicTileScale = sPanelHeight / (currGameLevel.intLevelHeightY + iMaxHorizontalHint);
            fOffsetWidth = sPanelWidth - (fBasicTileScale * (currGameLevel.intLevelWidthX + iMaxVerticalHint));
        }
        panel.AddComponent<Image>();
        panel.GetComponent<Image>().color = new Color(currGameLevel.colorBackground.colR, currGameLevel.colorBackground.colG, currGameLevel.colorBackground.colB);
        panel.transform.SetParent(canvasData.transform, false);
        for (int i = 0; i < currGameLevel.intLevelWidthX; i++)
        {
            for (int j = 0; j < currGameLevel.intLevelHeightY; j++)
            {
                Image newImage2 = (Image)Instantiate(imagePrefab);
                
                newImage2.name = "Tille_" + i.ToString() + "_" + j.ToString();
                //Point newPoint = new Point(i, j);
                /*
                if (currGameLevel.listLevelTilesOrder.Any(item => (item.tileLocationX == i && item.tileLocationY == j)))
                {
                    byte byteResult = currGameLevel.listLevelTilesOrder.Find(item => (item.tileLocationX == i && item.tileLocationY == j)).colorId;
                    newImage2.color = new Color(((float)currGameLevel.listPossibleColorsOfTiles[byteResult].colR)/255, ((float)currGameLevel.listPossibleColorsOfTiles[byteResult].colG) / 255, ((float)currGameLevel.listPossibleColorsOfTiles[byteResult].colB) / 255);
                    newImage2.GetComponent<SetColorInside>().SetColor(newImage2.color);
                }
                else
                {
                    newImage2.color = new Color(((float)currGameLevel.colorTilesNeutral.colR) / 255, ((float)currGameLevel.colorTilesNeutral.colG) / 255, ((float)currGameLevel.colorTilesNeutral.colB) / 255);
                    newImage2.GetComponent<SetColorInside>().SetColor(newImage2.color);
                }
                */
                newImage2.color = new Color(((float)currGameLevel.colorTilesNeutral.colR) / 255, ((float)currGameLevel.colorTilesNeutral.colG) / 255, ((float)currGameLevel.colorTilesNeutral.colB) / 255);
                newImage2.GetComponent<SetColorInside>().SetColor(newImage2.color);
                //newImage.color = Color.red;
                newImage2.transform.position = new Vector3((fOffsetWidth / 2) + (iMaxVerticalHint + i) * fBasicTileScale, (fOffsetHeight / 2) + j * fBasicTileScale);
                newImage2.GetComponent<RectTransform>().sizeDelta = new Vector2(fBasicTileScale, fBasicTileScale);
                newImage2.transform.SetParent(panel.transform, false);
            }
        }
        for(int i = 0; i < currGameLevel.intLevelHeightY; i++)
        {
            for(int j = 0; j < currGameLevel.listVerticlaNumbersHints[i].Count; j++)
            {
                Text tTemp = (Text)Instantiate(textPrefab);
                tTemp.name = "HintVerical_" + i.ToString() + "_" + j.ToString();
                tTemp.text = currGameLevel.listVerticlaNumbersHints[i][j].length.ToString();
                byte colorId = currGameLevel.listVerticlaNumbersHints[i][j].color;
                tTemp.color = new Color(((float)currGameLevel.listPossibleColorsOfTiles[colorId].colR) / 255, ((float)currGameLevel.listPossibleColorsOfTiles[colorId].colG) / 255, ((float)currGameLevel.listPossibleColorsOfTiles[colorId].colB) / 255);
                tTemp.transform.position = new Vector3((fOffsetWidth / 2) + (iMaxVerticalHint - currGameLevel.listVerticlaNumbersHints[i].Count + j) * fBasicTileScale, (fOffsetHeight / 2) + i * fBasicTileScale);
                tTemp.GetComponent<RectTransform>().sizeDelta = new Vector2(fBasicTileScale, fBasicTileScale);
                tTemp.transform.SetParent(panel.transform, false);
            }
        }
        for (int i = 0; i < currGameLevel.intLevelWidthX; i++)
        {
            for (int j = 0; j < currGameLevel.listHorizontalNumberHints[i].Count; j++)
            {
                Text tTemp = (Text)Instantiate(textPrefab);
                tTemp.name = "HintHorizontal_" + i.ToString() + "_" + j.ToString();
                tTemp.text = currGameLevel.listHorizontalNumberHints[i][j].length.ToString();
                byte colorId = currGameLevel.listHorizontalNumberHints[i][j].color;
                tTemp.color = new Color(((float)currGameLevel.listPossibleColorsOfTiles[colorId].colR) / 255, ((float)currGameLevel.listPossibleColorsOfTiles[colorId].colG) / 255, ((float)currGameLevel.listPossibleColorsOfTiles[colorId].colB) / 255);
                tTemp.transform.position = new Vector3((fOffsetWidth / 2) + (iMaxVerticalHint + i) * fBasicTileScale, (fOffsetHeight / 2) + (currGameLevel.intLevelHeightY + currGameLevel.listHorizontalNumberHints[i].Count - 1 - j) * fBasicTileScale);
                tTemp.GetComponent<RectTransform>().sizeDelta = new Vector2(fBasicTileScale, fBasicTileScale);
                tTemp.transform.SetParent(panel.transform, false);
            }
        }
        for (int i = 0; i < currGameLevel.listPossibleColorsOfTiles.Count; i++)
        {
            Image newColorImage = (Image)Instantiate(imagePrefab);
            newColorImage.name = "Color_" + i.ToString();
            if (i > 0)
            {
                newColorImage.color = new Color(((float)currGameLevel.listPossibleColorsOfTiles[i].colR) / 255, ((float)currGameLevel.listPossibleColorsOfTiles[i].colG) / 255, ((float)currGameLevel.listPossibleColorsOfTiles[i].colB) / 255);
            }
            else
            {
                newColorImage.color = Color.red;
            }
            newColorImage.GetComponent<SetColorInside>().SetColor(new Color(((float)currGameLevel.listPossibleColorsOfTiles[i].colR) / 255, ((float)currGameLevel.listPossibleColorsOfTiles[i].colG) / 255, ((float)currGameLevel.listPossibleColorsOfTiles[i].colB) / 255));
            newColorImage.transform.SetParent(colorPanel.transform, false);
        }
        controllMenager.GetComponent<ButtonUpdater>().iMaxX = currGameLevel.intLevelWidthX;
        controllMenager.GetComponent<ButtonUpdater>().iMaxY = currGameLevel.intLevelHeightY;
        controllMenager.GetComponent<ButtonUpdater>().iColorCount = currGameLevel.listPossibleColorsOfTiles.Count;

        GameObject tilesMenager = GameObject.Find("TilesMenager");
        tilesMenager.GetComponent<TilesMenager>().SetLevelDataClass(currGameLevel);
    }
    public class ConvertedColors
    {
        public Color colorTilesNeutral { get; }
        public Color colorBackground { get; }
        public Color colorMarker { get; }
        public List<Color> listPossibleColorsOfTiles { get; }
        public ConvertedColors()
        {
            colorTilesNeutral = new Color();
            colorBackground = new Color();
            colorMarker = new Color();
            listPossibleColorsOfTiles = new List<Color>();
        }
        public ConvertedColors(GameLevel gameLevel)
        {
            colorTilesNeutral = new Color(((float)gameLevel.colorTilesNeutral.colR) / 255.0f, ((float)gameLevel.colorTilesNeutral.colG) / 255.0f, ((float)gameLevel.colorTilesNeutral.colB) / 255.0f);
            colorBackground = new Color(((float)gameLevel.colorBackground.colR) / 255.0f, (gameLevel.colorBackground.colG) / 255.0f, (gameLevel.colorBackground.colB) / 255.0f);
            colorMarker = new Color(((float)gameLevel.colorMarker.colR) / 255.0f, ((float)gameLevel.colorMarker.colG) / 255.0f, ((float)gameLevel.colorMarker.colB) / 255.0f);
            listPossibleColorsOfTiles = new List<Color>();
            for (int i = 0; i < gameLevel.listPossibleColorsOfTiles.Count; i++)
            {
                listPossibleColorsOfTiles.Add(new Color(((float)gameLevel.listPossibleColorsOfTiles[i].colR) / 255.0f, ((float)gameLevel.listPossibleColorsOfTiles[i].colG) / 255.0f, ((float)gameLevel.listPossibleColorsOfTiles[i].colB) / 255.0f));
            }
        }
    }
    [System.Serializable]
    public class GameLevel
    {
        //public Dictionary<string, string> dictonaryLevelNames  { get; set; }
        public string stringEngName { get; set; }
        public string stringPolName { get; set; }
        public byte intLevelWidthX { get; set; }
        public byte intLevelHeightY { get; set; }
        public OnlyRGB colorTilesNeutral { get; set; }
        public OnlyRGB colorBackground { get; set; }
        public OnlyRGB colorMarker { get; set; }
        public List<OnlyRGB> listPossibleColorsOfTiles { get; set; }
        public List<LevelTilesOrder> listLevelTilesOrder { get; set; }
        public List<List<HintData>> listVerticlaNumbersHints { get; set; }
        public List<List<HintData>> listHorizontalNumberHints { get; set; }
        public GameLevel()
        {
            //dictonaryLevelNames = new Dictionary<string, string>();
            stringEngName = "";
            stringPolName = "";
            intLevelWidthX = 1;
            intLevelHeightY = 1;
            colorTilesNeutral = new OnlyRGB();
            colorBackground = new OnlyRGB();
            colorMarker = new OnlyRGB();
            listPossibleColorsOfTiles = new List<OnlyRGB>();
            listLevelTilesOrder = new List<LevelTilesOrder>();
            listVerticlaNumbersHints = new List<List<HintData>>();
            listHorizontalNumberHints = new List<List<HintData>>();
        }
    }
    public class HintData
    {
        public byte length { get; set; }
        public byte color { get; set; }
        public HintData()
        {
            length = 0;
            color = 0;
        }
        public HintData(byte TileLoc, byte ColorId)
        {
            length = TileLoc;
            color = ColorId;
        }
    }
    public class OnlyRGB
    {
        public byte colR { get; set; }
        public byte colG { get; set; }
        public byte colB { get; set; }
        public OnlyRGB()
        {
            colR = 0;
            colG = 0;
            colB = 0;
        }
        public OnlyRGB(byte ColorR, byte ColorG, byte ColorB)
        {
            colR = ColorR;
            colG = ColorG;
            colB = ColorB;
        }
    }
    public class LevelTilesOrder
    {
        public byte tileLocationX { get; set; }
        public byte tileLocationY { get; set; }
        public byte colorId { get; set; }
        public LevelTilesOrder()
        {
            tileLocationX = 0;
            tileLocationY = 0;
            colorId = 0;
        }
        public LevelTilesOrder(byte TileLocX, byte TileLocY, byte ColorId)
        {
            tileLocationX = TileLocX;
            tileLocationY = TileLocY;
            colorId = ColorId;
        }
    }
}
