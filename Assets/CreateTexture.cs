using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CreateTexture : MonoBehaviour
{
    // Start is called before the first frame update
    public RawImage rawImageFinished;
    public AspectRatioFitter arf;
    public Text textNameAndSize;
    public void MakeTexture(LoadFromText.GameLevel currLevel)
    {
        /*
        var myTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        
        // set the pixel values
        myTexture.SetPixel(0, 0, Color.red);
        myTexture.SetPixel(1, 0, Color.green);
        myTexture.SetPixel(0, 1, Color.blue);
        myTexture.SetPixel(1, 1, Color.black);
        
        // Apply all SetPixel calls
        myTexture.Apply();
        rawImageFinished.texture = myTexture;
        */
        LoadFromText.ConvertedColors currConvertedLevel = new LoadFromText.ConvertedColors(currLevel);
        Texture2D myTexture = new Texture2D(currLevel.intLevelWidthX, currLevel.intLevelHeightY);
        myTexture.filterMode = FilterMode.Point;
        for (int i = 0; i < currLevel.intLevelWidthX; i++)
        {
            for(int j = 0; j <currLevel.intLevelHeightY; j++)
            {
                if(currLevel.listLevelTilesOrder.Any(item => (item.tileLocationX == i && item.tileLocationY == j)))
                {
                    myTexture.SetPixel(i, j, currConvertedLevel.listPossibleColorsOfTiles[currLevel.listLevelTilesOrder.First(item => (item.tileLocationX == i && item.tileLocationY == j)).colorId]);
                }
                else
                {
                    myTexture.SetPixel(i, j, currConvertedLevel.colorTilesNeutral);
                }
            }
        }
        myTexture.Apply();
        rawImageFinished.texture = myTexture;
        arf.aspectRatio = ((float)currLevel.intLevelWidthX) / ((float)currLevel.intLevelHeightY);
        textNameAndSize.text = currLevel.stringEngName + " [" + currLevel.intLevelWidthX.ToString() + "," + currLevel.intLevelHeightY.ToString() + "]";
    }
}
