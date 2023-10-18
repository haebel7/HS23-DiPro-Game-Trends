using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIfactory : MonoBehaviour
{
    public Button ressourceTab;
    public Button equipmentTab;
    public VisualElement tablinks;
    private VisualElement root;
    private bool ressourceActive = true;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        ressourceTab = root.Q<Button>("inventory-tab");
        equipmentTab = root.Q<Button>("equipment-tab");
        tablinks = root.Q<VisualElement>("tablinks");
        ressourceTab.clicked += ressourceTabPressed;
        equipmentTab.clicked += equipmentTabPressed;
        updateTabs();
    }

    void updateTabs()
    {
        if (ressourceActive)
        {
            activateRessource();
            deactivateEquipment();
        }
        else
        {
            activateEquipment();
            deactivateRessource();
        }
    }

    void deactivateEquipment()
    {
        VisualElement equipmentWrapper = root.Q<VisualElement>("equipment-wrapper");
        equipmentWrapper.style.opacity = 0.1f;
        equipmentTab.style.opacity = 0.1f;
    }

    void activateEquipment()
    {      
        VisualElement equipmentWrapper = root.Q<VisualElement>("equipment-wrapper");
        equipmentWrapper.style.opacity = 1f;
        equipmentTab.style.opacity = 1f;
        equipmentWrapper.BringToFront();
        tablinks.BringToFront();
        //Debug.Log(equipmentWrapper.parent.IndexOf(equipmentWrapper));
    }

    void deactivateRessource()
    {
        VisualElement ressourceWrapper = root.Q<VisualElement>("inventory-wrapper");
        ressourceWrapper.style.opacity = 0.1f;
        ressourceTab.style.opacity = 0.1f;
    }

    void activateRessource()
    {
        VisualElement ressourceWrapper = root.Q<VisualElement>("inventory-wrapper");
        ressourceWrapper.style.opacity = 1f;
        ressourceTab.style.opacity = 1f;
        ressourceWrapper.BringToFront();
        tablinks.BringToFront();
        //Debug.Log(inventoryWrapper.parent.IndexOf(inventoryWrapper));
    }

    void ressourceTabPressed()
    {
        ressourceActive = true;
        updateTabs();
    }

    void equipmentTabPressed()
    {
        ressourceActive = false;
        updateTabs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
