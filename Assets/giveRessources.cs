using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class giveRessources : MonoBehaviour
{
    private VisualElement root;
    public VisualTreeAsset vta;
    public IntegerField ironCount;
    public IntegerField copperCount;
    public Button giveIron;
    public Button giveCopper;
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        giveIron = root.Q<Button>("giveIron");
        giveCopper = root.Q<Button>("giveCopper");
        ironCount = root.Q<IntegerField>("ironCount");
        copperCount = root.Q<IntegerField>("copperCount");

        giveIron.clicked += giveIronButtonPressed;
        giveCopper.clicked += giveCopperButtonPressed;
    }

    void giveIronButtonPressed()
    {
        ironCount.value += 50;
    }

    void giveCopperButtonPressed()
    {
        copperCount.value += 50;
    }

    public int getIronCount()
    {
        return ironCount.value;
    }

    public int getCopperCount()
    {
        return copperCount.value;
    }

    public void decrementIronCount(int number)
    {
        ironCount.value -= number;
    }

    public void decrementCopperCount(int number)
    {
        copperCount.value -= number;
    }
}
