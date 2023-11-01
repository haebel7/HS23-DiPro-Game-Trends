using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIfactory : MonoBehaviour
{
    public Button openMachinesButton;
    private VisualElement root;
    public VisualElement machinesList;
    public VisualElement machineWrapper;
    public VisualElement containerWrapper;
    private bool machinesOpen = false;
    public Sprite machineButtonBackground;
    public Sprite machineWrapperBackground;
   // public VisualTreeAsset tabs;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        machinesList = root.Q<VisualElement>("machines-list");
        openMachinesButton = root.Q<Button>("openMachines");
        machineWrapper = root.Q<VisualElement>("machines-wrapper");
        containerWrapper = root.Q<VisualElement>("container-wrapper");
        openMachinesButton.clicked += toggleMachines;
        //VisualElement UItabs = tabs.Instantiate();
       // containerWrapper.Add(UItabs);
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

}
