using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class uicontroller : MonoBehaviour
{
    public Button startButton;
    public Button addText;
    public Label output;
    public UIDocument smallEle;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Q<Button>("startButton");
        addText = root.Q<Button>("addText");
        output = root.Q<Label>("output");

        startButton.clicked += startButtonPressed;
        addText.clicked += addTextPressed;
    }

    void startButtonPressed()
    {
        SceneManager.LoadScene("MainScene");
    }

    void addTextPressed()
    {
        output.text += "Hello World<br>";
        output.style.display = DisplayStyle.Flex;
    }
}
