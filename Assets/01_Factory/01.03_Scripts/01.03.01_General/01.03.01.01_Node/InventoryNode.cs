using RuntimeNodeEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            incomingValues = new List<Ressource>();

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
                    Debug.Log(index);
                    Ressource r = ressourceInventar.getListOfRessources()[index];

                    if (res.ownedAmount > 0 && res.ownedAmount != r.ownedAmount)
                    {
                        r.incrementCount(res.ownedAmount);
                        res.ownedAmount = 0;
                        res._name = null;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        private void FixedUpdate()
        {
            if (fixedUpdateCount % 50 == 0 && incomingValues != null && incomingValues[0].ownedAmount > 0)
            {
                OnConnectedValueUpdated();
            }

            fixedUpdateCount %= 10000;
            fixedUpdateCount++;
        }
    }
}
