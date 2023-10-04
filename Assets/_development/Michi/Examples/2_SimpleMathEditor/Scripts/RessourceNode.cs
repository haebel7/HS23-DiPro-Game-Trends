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
    public class Packet : MonoBehaviour
    {
        private int itemCount;
        private string ressource;
        //private int priority;

        public void setAttributes(int itemCount, string ressource)
        {
            this.itemCount = itemCount;
            this.ressource = ressource;
            //this.priority = priority;
        }

        public int getItemCount() { return this.itemCount; }
        public string getRessource() {  return this.ressource; }
    }
    
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
        private VisualElement root;

        public override void Setup()
        {
            root = GameObject.Find("UIDocument").GetComponent<UIDocument>().rootVisualElement;
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
                HandleFieldValue(getPriority().ToString());
            });

            prio.onEndEdit.AddListener(HandleFieldValue);

            HandleFieldValue(prio.text);
        }

        private void HandleFieldValue(string prio)
        {
            string ressource = getRessource();
            
            outputSocketLeft.SetValue(ressource);
            outputSocketRight.SetValue(ressource);
        }

        private void priority(int number)
        {
            int countNow = int.Parse(prio.text);
            int countNew = countNow + number;
            prio.text = countNew.ToString();
            HandleFieldValue(countNew.ToString());
        }

        public int getPriority()
        {
            return int.Parse(prio.text);
        }

        public string getRessource()
        {
            return dropdown.options[dropdown.value].text;
        }

        public int getRessourceCount(string ressource)
        {
            switch(ressource)
            {
                case "Iron": return root.Q<IntegerField>("ironCount").value;
                case "Copper": return root.Q<IntegerField>("copperCount").value;
                default: return -1;
            }
        }

        public override void OnSerialize(Serializer serializer)
        {
            serializer.Add("ressource", getRessourceCount(getRessource()).ToString());
        }

        public override void OnDeserialize(Serializer serializer)
        {
            var value = serializer.Get("ressource");
            prio.SetTextWithoutNotify(value);

            HandleFieldValue(value);
        }
    }
}
