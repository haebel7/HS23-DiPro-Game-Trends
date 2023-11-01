using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class payerHealthbarUI : MonoBehaviour
{
    [SerializeField] private HealthObject health;
    private VisualElement root;
    private VisualElement healthbar;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        healthbar = root.Q<VisualElement>("healthbar");
        initHealthbar(health.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        updateHealthbar(health.currentHealth);
    }

    private void updateHealthbar(int currentHealth)
    {

    }

    private void initHealthbar(int maxHealth)
    {

    }
}
