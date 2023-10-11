using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;
using Button = UnityEngine.UI.Button;

namespace RuntimeNodeEditor.Examples
{   
    public class RessourceNode : Node
    {
        public TMP_InputField prio;
        public SocketOutput outputSocketLeft;
        public SocketOutput outputSocketRight;
        public Button buttonAdd1;
        public Button buttonAdd10;
        public Button buttonAdd_1;
        public Button buttonAdd_10;
        public TMP_Dropdown dropdown;
        public RessourceInventar ressourceInventar;

        public override void Setup()
        {
            Register(outputSocketLeft);
            Register(outputSocketRight);
            SetHeader("Ressource");

            prio.text = "0";
            
            buttonAdd1.onClick.AddListener(delegate { priority(int.Parse(buttonAdd1.GetComponentInChildren<TextMeshProUGUI>().text)); });
            buttonAdd10.onClick.AddListener(delegate { priority(int.Parse(buttonAdd10.GetComponentInChildren<TextMeshProUGUI>().text)); });
            buttonAdd_1.onClick.AddListener(delegate { priority(int.Parse(buttonAdd_1.GetComponentInChildren<TextMeshProUGUI>().text)); });
            buttonAdd_10.onClick.AddListener(delegate { priority(int.Parse(buttonAdd_10.GetComponentInChildren<TextMeshProUGUI>().text)); });
            
            dropdown.AddOptions(new List<TMP_Dropdown.OptionData>()
            {
                new TMP_Dropdown.OptionData("Iron"),
                new TMP_Dropdown.OptionData("Copper")
            });

            dropdown.onValueChanged.AddListener(selected =>
            {
                HandleFieldValue();
            });

            //prio.onEndEdit.AddListener(HandleFieldValue);

            HandleFieldValue();
        }

        private void HandleFieldValue()
        {            
            outputSocketLeft.SetValue(getRessource());
            outputSocketRight.SetValue(getRessource());
        }

        private void priority(int number)
        {
            int countNow = int.Parse(prio.text);
            int countNew = countNow + number;
            prio.text = countNew.ToString();
            HandleFieldValue();
        }

        public int getPriority()
        {
            return int.Parse(prio.text);
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
