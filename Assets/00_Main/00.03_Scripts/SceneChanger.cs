using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void OnGoToDungeon()
    {
        SceneManager.LoadSceneAsync("Assets/00_Main/07.02.002_Main_Dungeon.unity", LoadSceneMode.Single);
    }

    public void OnGoToFactory()
    {
        SceneManager.LoadSceneAsync("Assets/00_Main/07.02.003_FactoryScene.unity", LoadSceneMode.Single);
    }

    public void OnToStart()
    {
        SceneManager.LoadSceneAsync("Assets/00_Main/07.02.001_StartScreen.unity", LoadSceneMode.Single);
    }
}
