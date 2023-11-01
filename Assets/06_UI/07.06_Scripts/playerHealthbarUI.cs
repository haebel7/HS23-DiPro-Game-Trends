using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class playerHealthbarUI : MonoBehaviour
{
    [SerializeField] private HealthObject health;
    private VisualElement root;
    private VisualElement healthbar;
    private List<VisualElement> healthbarPoints;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        healthbar = root.Q<VisualElement>("healthbar");
        healthbarPoints = healthbar.Query<VisualElement>(className: "healpoint").ToList();
        initHealthbar();
    }

    public void updateHealthbar()
    {
        int numberOfHealthPoints = (int) MathF.Ceiling((float)healthbarPoints.Count / (float)health.maxHealth * (float)health.currentHealth);
        if(numberOfHealthPoints < 0)
        {
            numberOfHealthPoints = 0;
        }
        for (var i = healthbarPoints.Count - numberOfHealthPoints - 1; i >= 0; i--)
        {
            Debug.Log(i);
            healthbarPoints[i].style.opacity = 0.2f;
        }
    }

    private void initHealthbar()
    {
        Debug.Log("test");
        foreach (var healthbarPoint in healthbarPoints)
        {
            healthbarPoint.style.opacity = 1f;
        }
    }
}
