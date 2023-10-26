using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RuntimeNodeEditor.Examples
{   
    public class RessourceNode : Node
    {
        public SocketOutput outputSocketLeft;
        public SocketOutput outputSocketRight;
        public TMP_Dropdown dropdown;
        public RessourceInventar ressourceInventar;

        public override void Setup()
        {
            Register(outputSocketLeft);
            Register(outputSocketRight);
            SetHeader("Ressource");

            List<TMP_Dropdown.OptionData> dropdownList = new List<TMP_Dropdown.OptionData>();
            foreach (Ressource r in ressourceInventar.getListOfRessources())
            {
                dropdownList.Add(new TMP_Dropdown.OptionData(r.name));
            }
            

            dropdown.AddOptions(dropdownList);

            dropdown.onValueChanged.AddListener(selected =>
            {
                HandleFieldValue();
            });

            HandleFieldValue();
        }

        private void HandleFieldValue()
        {            
            outputSocketLeft.SetValue(getRessource());
            outputSocketRight.SetValue(getRessource());
        }

        public Ressource getRessource()
        {
            int index = ressourceInventar.getRessourceIndex(dropdown.options[dropdown.value].text);
            Ressource res = ressourceInventar.getListOfRessources()[index];
            return res;
        }

        /*public override void OnSerialize(Serializer serializer)
        {
            serializer.Add("ressource", getRessourceCount(getRessource()).ToString());
        }

        public override void OnDeserialize(Serializer serializer)
        {
            var value = serializer.Get("ressource");
            prio.SetTextWithoutNotify(value);

            HandleFieldValue(value);
        }*/
    }
}
