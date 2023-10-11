using RuntimeNodeEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class giveRessources : MonoBehaviour
{
    public IntegerField ironCount;
    public IntegerField copperCount;
    public IntegerField ironRefinedCount;
    public IntegerField copperRefinedCount;
    public Button giveIron;
    public Button giveCopper;
    public RessourceInventar ressourceInventar;
    private VisualElement root;
    private List<Ressource> listOfRes;

    private int indexIron;
    private int indexCopper;
    private int indexRefinedIron;
    private int indexRefinedCopper;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        giveIron = root.Q<Button>("giveIron");
        giveCopper = root.Q<Button>("giveCopper");
        ironCount = root.Q<IntegerField>("ironCount");
        copperCount = root.Q<IntegerField>("copperCount");
        ironRefinedCount = root.Q<IntegerField>("ironRefinedCount");
        copperRefinedCount = root.Q<IntegerField>("copperRefinedCount");

        listOfRes = ressourceInventar.getListOfRessources();
        indexIron = ressourceInventar.getRessourceIndex("Iron");
        indexCopper = ressourceInventar.getRessourceIndex("Copper");
        indexRefinedIron = ressourceInventar.getRessourceIndex("IronRefined");
        indexRefinedCopper = ressourceInventar.getRessourceIndex("CopperRefined");

        giveIron.clicked += giveIronButtonPressed;
        giveCopper.clicked += giveCopperButtonPressed;

        Update();
    }

    void giveIronButtonPressed()
    {
        listOfRes[indexIron].incrementCount(50);
        ironCount.value = listOfRes[indexIron].ownedAmount;
    }

    void giveCopperButtonPressed()
    {
        listOfRes[indexCopper].incrementCount(50);
        copperCount.value = listOfRes[indexCopper].ownedAmount;
    }

    public int getIronCount()
    {
        return listOfRes[indexIron].ownedAmount;
    }

    public int getCopperCount()
    {
        return listOfRes[indexCopper].ownedAmount;
    }

    private void Update()
    {
        ironCount.value = listOfRes[indexIron].ownedAmount;
        copperCount.value = listOfRes[indexCopper].ownedAmount;
        ironRefinedCount.value = listOfRes[indexRefinedIron].ownedAmount;
        copperRefinedCount.value = listOfRes[indexRefinedCopper].ownedAmount;
    }
}