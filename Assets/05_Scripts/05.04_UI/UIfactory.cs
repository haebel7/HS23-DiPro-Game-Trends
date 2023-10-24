using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIfactory : MonoBehaviour
{
    public Button ressourceTab;
    public Button equipmentTab;
    public Button openMachinesButton;
    public VisualElement tablinks;
    private VisualElement root;
    public VisualElement machinesList;
    public VisualElement machineWrapper;
    private bool ressourceActive = true;
    private bool machinesOpen = false;
    public Sprite machineButtonBackground;
    public Sprite machineWrapperBackground;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        machinesList = root.Q<VisualElement>("machines-list");
        ressourceTab = root.Q<Button>("inventory-tab");
        equipmentTab = root.Q<Button>("equipment-tab");
        openMachinesButton = root.Q<Button>("openMachines");
        tablinks = root.Q<VisualElement>("tablinks");
        machineWrapper = root.Q<VisualElement>("machines-wrapper");
        ressourceTab.clicked += ressourceTabPressed;
        equipmentTab.clicked += equipmentTabPressed;
        openMachinesButton.clicked += toggleMachines;
        updateTabs();
    }

    void toggleMachines()
    {
        if (machinesOpen)
        {
            closeMachines();
        }
        else
        {
            openMachines();
        }
    }

    void closeMachines()
    {
        machineWrapper.style.backgroundImage = new StyleBackground(StyleKeyword.None);
        openMachinesButton.style.backgroundImage = new StyleBackground(machineButtonBackground);
        machinesList.style.width = Length.Percent(0);
        machinesList.style.visibility = Visibility.Hidden;
        machinesOpen = false;
    }

    void openMachines()
    {
        machineWrapper.style.backgroundImage = new StyleBackground(machineWrapperBackground);
        openMachinesButton.style.backgroundImage = new StyleBackground(StyleKeyword.None);
        machinesList.style.width = Length.Auto();
        machinesList.style.visibility = Visibility.Visible;
        machinesOpen = true;
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
