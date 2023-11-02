using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private Scene scene;

    void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    public void OnButtonClick()
    {
        if (scene.name == "FactoryScene")
        {
            SceneManager.LoadSceneAsync("Assets/00_Main/07.02.001_Main_Dungeon.unity", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadSceneAsync("Assets/01_Factory/01.02_Scenes/FactoryScene.unity", LoadSceneMode.Single);
        }
    }
}
