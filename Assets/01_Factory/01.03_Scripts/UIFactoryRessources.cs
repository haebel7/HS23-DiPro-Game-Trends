using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RuntimeNodeEditor
{
    public class UIFactoryRessources : MonoBehaviour
    {
        [SerializeField] private RessourceInventar ressourceInventar;
        private VisualElement InventoryWrapper;

        // Start is called before the first frame update
        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            InventoryWrapper = root.Q<VisualElement>("item-wrapper");
            foreach (var ressource in ressourceInventar.getListOfRessources())
            {

            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
