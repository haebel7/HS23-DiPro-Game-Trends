using RuntimeNodeEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace RuntimeNodeEditor
{
    public class InventoryNode : Node
    {
        public SocketInput inputSocket1;
        public SocketInput inputSocket2;
        public SocketInput inputSocket3;
        public RessourceInventar ressourceInventar;

        private List<IOutput> _incomingOutputs;
        private List<Ressource> incomingValues;
        private int fixedUpdateCount = 0;
        public override void Setup()
        {
            _incomingOutputs = new List<IOutput>();
            incomingValues = new List<Ressource>();

            Register(inputSocket1);
            Register(inputSocket2);
            Register(inputSocket3);
            SetHeader("Storage");

            OnConnectionEvent += OnConnection;
            OnDisconnectEvent += OnDisconnect;
        }

        // Function that is called after you connect a Node to this one
        public void OnConnection(SocketInput input, IOutput output)
        {
            output.ValueUpdated += OnConnectedValueUpdated;
            _incomingOutputs.Add(output);
            OnConnectedValueUpdated();
        }

        // Function that is called after you disconnect any of your linked Nodes
        public void OnDisconnect(SocketInput input, IOutput output)
        {
            output.ValueUpdated -= OnConnectedValueUpdated;
            _incomingOutputs.Remove(output);
            OnConnectedValueUpdated();
        }

        // Saves the given values from every connected Output Socket in a List
        private void OnConnectedValueUpdated()
        {
            incomingValues.Clear();

            foreach (var c in _incomingOutputs)
            {
                incomingValues.Add(c.GetValue<Ressource>());
            }
            moveToInventory();
            }

        // Moves the given Values from the Input Socket to your Inventory
        public void moveToInventory()
        {
            foreach (Ressource res in incomingValues)
            {
                try
                {
                    int index = ressourceInventar.getRessourceIndex(res._name);
                    if (index >= 0)
                    {
                        Ressource r = ressourceInventar.getListOfRessources()[index];

                        if (res.ownedAmount > 0)
                        {
                            r.incrementCount(res.ownedAmount);
                            res.ownedAmount = 0;
                            res._name = null;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        public override void OnSerialize(Serializer serializer)
        {

        }

        public override void OnDeserialize(Serializer serializer)
        {
            OnConnectedValueUpdated();
        }

        private void FixedUpdate()
        {
            if (incomingValues.Count > 0)
            {
                if (fixedUpdateCount % 50 == 0 && incomingValues != null && incomingValues[0].ownedAmount > 0)
                {
                    OnConnectedValueUpdated();
                }
            }

            fixedUpdateCount %= 10000;
            fixedUpdateCount++;
        }
    }
}
