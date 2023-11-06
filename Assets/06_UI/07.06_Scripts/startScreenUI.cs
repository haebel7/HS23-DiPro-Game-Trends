using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class startScreenUI : MonoBehaviour
{
    public Button newGameBtn;
    public Button quitGameBtn;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        newGameBtn = root.Q<Button>("newGame");
        quitGameBtn = root.Q<Button>("quitGame");

        newGameBtn.clicked += newGameBtnPressed;
        quitGameBtn.clicked += quitGameBtnPressed;
    }

    void newGameBtnPressed()
    {
        //SceneManager.LoadScene("07.02.001_Main_Dungeon");
        GetComponent<SceneChanger>().OnGoToDungeon();
    }

    void quitGameBtnPressed() {
        Application.Quit();
    }
}

