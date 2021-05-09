using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempLoadLevels : MonoBehaviour
{
    public List<TextAsset> textAssetList;
    public GameObject LoadingPanel;
    // Start is called before the first frame update
    void Start()
    {
        RefreshDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RefreshDisplay()
    {
        AddButtons();
    }
    //public Transform contentPanelLevelsTemp;
    public void AddButtons()
    {        
        for(int i = 0; i < textAssetList.Count; i++)
        {
            //Debug.Log(textAssetList[i].name);
        }
        /*
        for (int i = 0; i < levelPackList.Count; i++)
        {
            GameObject newButton = buttonObjectPoolLevelsPack.GetObject();
            newButton.transform.SetParent(contentPanelLevelsPack);
            string tempText = "NoName";
            if (curLangSet == "PL")
            {
                tempText = levelPackList[i].nameOfPack_Pol;
            }
            else
            {
                tempText = levelPackList[i].nameOfPack_Eng;
            }
            SampleButton sampleButton = newButton.GetComponent<SampleButton>();
            sampleButton.Setup(tempText, this, i, 0);
        }
        */
    }
    public void LoadTextAsset(int levelID)
    {
        //Debug.Log(textAssetList[levelID].name);
        PlayerPrefs.SetString("LevelToLoad", textAssetList[levelID].text);
        StartCoroutine(LoadLevelAsynchornous());
    }

    IEnumerator LoadLevelAsynchornous()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("LevelCommon");
        LoadingPanel.SetActive(true);
        while (operation.isDone == false)
        {
            yield return null;
        }
    }
}
