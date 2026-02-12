using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using MagmaLabs;
using MagmaLabs.Economy;
using MagmaLabs.UI;
using MagmaLabs.Economy.Security;
using MagmaLabs.Editor;


public class LevelButton : MonoBehaviour
{
    const int DEBUG_INFO_LEVEL = 2;
    [SerializeField] private ProgressionNode levelData;
    [SerializeField] private GameObject wrapper;
    [SerializeField] private TMPEnhanced score;


    

    void Start()
    {
        StartCoroutine(LateStart());
    }
    IEnumerator LateStart()
    {
        yield return null;
        try
        {
            string serialized = SecureProfileStats.instance.LoadString("level_" + levelData.name);
            ProgressionNode node = (ProgressionNode)ScriptableObject.CreateInstance(typeof(ProgressionNode));

            node.LoadFromSerialized(serialized);

            levelData = node;

            DebugEnhanced.LogInfoLevel("Successfully loaded object", 2, DEBUG_INFO_LEVEL);
            DebugEnhanced.LogInfoLevel("Level Status: " + levelData.status.ToString(), 2, DEBUG_INFO_LEVEL);
        }catch(Exception e)
        {
            DebugEnhanced.LogInfoLevel("No saved object detected, saving object", 2, DEBUG_INFO_LEVEL);
            string serialized = levelData.Serialize();
            SecureProfileStats.instance.SaveString("level_" + levelData.name, serialized);
        }

        levelData.RefreshInstance();


        
        if(levelData.status == NodeStatus.Locked){
            wrapper.SetActive(false);
        }else{
            wrapper.SetActive(true);
            score.SetText(levelData.GetTag("highScore"));

        }


    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(levelData.status == NodeStatus.Locked){
            wrapper.SetActive(false);
        }else{
            wrapper.SetActive(true);
            score.SetText(levelData.GetTag("highScore"));

        }
    }
    public void OnClick()
    {

        SceneManager.LoadScene(levelData.name);
        LevelUIManager.instance.OpenLevel();
    }

}
