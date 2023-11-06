using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class victoryButtonUI : MonoBehaviour
{
    public Button goToFactoryButton;
    private VisualElement root;
    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        goToFactoryButton = root.Q<Button>("goToFactory");
        goToFactoryButton.clicked += goToFactoryPressed;
    }

    void goToFactoryPressed()
    {
        GetComponent<SceneChanger>().OnGoToFactory();
    }
}
