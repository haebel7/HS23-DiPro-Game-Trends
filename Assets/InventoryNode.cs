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
            SetHeader("Inventory");

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
            incomingValues = new List<Ressource>();

            foreach (var c in _incomingOutputs)
            {
                incomingValues.Add(c.GetValue<Ressource>());
            }

            try 
                {
                    Debug.Log(incomingValues[0].ownedAmount); 
                } 
            catch (Exception e)
                { 
                    Debug.LogError(e);
                }
            moveToInventory();
            }

        public void moveToInventory()
        {
            foreach (Ressource res in incomingValues)
            {
                try
                {
                    if (res.ownedAmount > 0)
                    {
                        int index = ressourceInventar.getRessourceIndex(res._name);
                        Ressource r = ressourceInventar.getListOfRessources()[index];
                        r.incrementCount(res.ownedAmount);
                        res.ownedAmount = 0;
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
            if (fixedUpdateCount % 50 == 0 && incomingValues != null)
            {
                OnConnectedValueUpdated();
            }

            fixedUpdateCount %= 10000;
            fixedUpdateCount++;
        }
    }
}
