using RuntimeNodeEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class giveRessources : MonoBehaviour
{
    public VisualTreeAsset vta;
    public IntegerField ironCount;
    public IntegerField copperCount;
    public IntegerField ironRefinedCount;
    public IntegerField copperRefinedCount;
    public Button giveIron;
    public Button giveCopper;
    public RessourceInventar ressourceInventar;
    private VisualElement root;
    private List<Ressource> res;

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

        res = ressourceInventar.getListOfRessources();
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
        res[indexIron].incrementCount(50);
        ironCount.value = res[indexIron].ownedAmount;
    }

    void giveCopperButtonPressed()
    {
        res[indexCopper].incrementCount(50);
        copperCount.value = res[indexCopper].ownedAmount;
    }

    public int getIronCount()
    {
        return res[indexIron].ownedAmount;
    }

    public int getCopperCount()
    {
        return res[indexCopper].ownedAmount;
    }

    private void Update()
    {
        ironCount.value = res[indexIron].ownedAmount;
        copperCount.value = res[indexCopper].ownedAmount;
        ironRefinedCount.value = res[indexRefinedIron].ownedAmount;
        copperRefinedCount.value = res[indexRefinedCopper].ownedAmount;

    }
}
