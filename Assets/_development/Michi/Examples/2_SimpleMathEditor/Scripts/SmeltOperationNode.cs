using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RuntimeNodeEditor.Examples
{
    public class SmeltOperationNode : Node
    {
        public SocketInput inputSocket;
        public SocketOutput outputSocket;
        public TMP_Text inputText;
        public TMP_Text outputText;

        private List<IOutput> _incomingOutputs;
        //private int trans;
        private List<Packet> incomingValues;
        //private int internalInventory = 100;

        public override void Setup()
        {
            _incomingOutputs = new List<IOutput>();

            Register(inputSocket);
            Register(outputSocket);

            SetHeader("Furnace");

            OnConnectionEvent += OnConnection;
            OnDisconnectEvent += OnDisconnect;
        }

        public void OnConnection(SocketInput input, IOutput output)
        {
            output.ValueUpdated += OnConnectedValueUpdated;
            _incomingOutputs.Add(output);

            OnConnectedValueUpdated();
        }

        public void OnDisconnect(SocketInput input, IOutput output)
        {
            output.ValueUpdated -= OnConnectedValueUpdated;
            _incomingOutputs.Remove(output);

            OnConnectedValueUpdated();
        }

        private void OnConnectedValueUpdated()
        {

            incomingValues = new List<Packet>();
            foreach (var c in _incomingOutputs)
            {
                incomingValues.Add(c.GetValue<Packet>());
            }
            
            DisplayInput(incomingValues[0].getItemCount());
        }

        private void DisplayInput(int value)
        {
            inputText.text = value.ToString();
        }
    }
}
