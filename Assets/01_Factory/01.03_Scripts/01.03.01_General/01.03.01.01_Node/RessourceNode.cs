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
        private List<TMP_Dropdown.OptionData> dropdownList;

        public override void Setup()
        {
            Register(outputSocketLeft);
            Register(outputSocketRight);
            SetHeader("Ressource");

            dropdownList = new List<TMP_Dropdown.OptionData>();
            foreach (Ressource r in ressourceInventar.getListOfRessources())
            {
                dropdownList.Add(new TMP_Dropdown.OptionData(r._name));
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

        public override void OnSerialize(Serializer serializer)
        {
            serializer.Add("dropdown", dropdown.value.ToString());
        }

        public override void OnDeserialize(Serializer serializer)
        {
            var value = serializer.Get("dropdown");
            dropdown.SetValueWithoutNotify(int.Parse(value));

            HandleFieldValue();
        }
    }
}
