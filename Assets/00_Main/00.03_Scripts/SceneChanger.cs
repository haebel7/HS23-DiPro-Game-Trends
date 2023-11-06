using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //private Scene scene;

    //void Start()
    //{
    //    scene = SceneManager.GetActiveScene();
    //}

    public void OnGoToDungeon()
    {
        SceneManager.LoadSceneAsync("Assets/00_Main/07.02.001_Main_Dungeon.unity", LoadSceneMode.Single);
    }

    public void OnGoToFactory()
    {
        SceneManager.LoadSceneAsync("Assets/00_Main/FactoryScene.unity", LoadSceneMode.Single);
    }

    public void OnToStart()
    {
        SceneManager.LoadSceneAsync("Assets/00_Main/StartScreen.unity", LoadSceneMode.Single);
    }
}
