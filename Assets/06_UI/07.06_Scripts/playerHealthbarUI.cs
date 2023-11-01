using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class playerHealthbarUI : MonoBehaviour
{
    [SerializeField] private HealthObject health;
    private VisualElement root;
    private VisualElement healthbar;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        healthbar = root.Q<VisualElement>("healthbar");
        initHealthbar();
    }

    public void updateHealthbar()
    {

    }

    private void initHealthbar()
    {

    }
}
