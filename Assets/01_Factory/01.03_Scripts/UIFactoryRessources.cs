using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RuntimeNodeEditor
{
    public class UIFactoryRessources : MonoBehaviour
    {
        public RessourceInventar ressourceInventar;
        private VisualElement InventoryWrapper;
        public VisualTreeAsset item;

        // Start is called before the first frame update
        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            InventoryWrapper = root.Q<VisualElement>("item-wrapper");
            foreach (var ressource in ressourceInventar.getListOfRessources())
            {
                VisualElement UIitem = item.Instantiate();
                InventoryWrapper.Add(UIitem);
                UIitem.AddToClassList("item");
                Label itemNameUI = UIitem.Q<Label>("item-name");
                itemNameUI.text = ressource.name;
                VisualElement iconUI = UIitem.Q<VisualElement>("icon");
                iconUI.style.backgroundImage = new StyleBackground(ressource.icon);
                Label itemCountUI = UIitem.Q<Label>("item-count");
                int ownedAmount = ressource.ownedAmount;
                string formattedAmount;
                if (ownedAmount >= 1000)
                {
                    formattedAmount = (ownedAmount / 1000).ToString() + "K";
                }
                else
                {
                    formattedAmount = ownedAmount.ToString();
                }

                itemCountUI.text = formattedAmount;
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
