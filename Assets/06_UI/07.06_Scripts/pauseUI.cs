using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class pauseUI : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField]private GameObject pauseUIObj;
    public Button backToStartBtn;
    public Button quitGameBtn;
    private bool doOnce = true;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }

    void backToStartBtnPressed()
    {
        SceneManager.LoadScene("StartScreen");
        isPaused = false;
        Time.timeScale = 1;
        pauseUIObj.SetActive(false);
    }

    void quitGameBtnPressed()
    {
        Application.Quit();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        if (isPaused)
        {
            pauseUIObj.SetActive(true);
            if (doOnce)
            {
                var root = pauseUIObj.GetComponent<UIDocument>().rootVisualElement;
                backToStartBtn = root.Q<Button>("backToStart");
                quitGameBtn = root.Q<Button>("quitGame");

                backToStartBtn.clicked += backToStartBtnPressed;
                quitGameBtn.clicked += quitGameBtnPressed;
                doOnce = false;
            }
        }
        else
        {
            pauseUIObj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            TogglePause();
        }
    }
}
