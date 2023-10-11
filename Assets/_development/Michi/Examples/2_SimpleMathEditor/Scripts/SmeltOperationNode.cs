using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
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
        public TMP_Text inputRessource;
        public TMP_Text outputRessource;
        public RessourceInventar ressourceInventory;
        public List<FurnaceRecepie> recepieList;

        private List<IOutput> _incomingOutputs;
        private List<Ressource> incomingValues;
        private int fixedUpdateCount = 0;
        private int tranferAmount = 2;
        private int internalInventory = 50;

        public override void Setup()
        {
            _incomingOutputs = new List<IOutput>();

            Register(inputSocket);
            Register(outputSocket);

            SetHeader("Furnace");
            inputText.text = "0";
            outputText.text = "0";
            inputRessource.text = null;
            outputRessource.text = null;

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
            if (int.Parse(inputText.text) < internalInventory)
            {
                transfer(tranferAmount);
            }
        }

        private void transfer(int amount)
        {
            if (incomingValues.Count > 0)
            {
                int index = ressourceInventory.getRessourceIndex(incomingValues[0]._name);
                Ressource res = ressourceInventory.getListOfRessources()[index];
                if (res.canDecrement(amount) && (inputSocket.ressources[0]._name == res._name || inputSocket.ressources[0]._name == null))
                {
                    addDisplayInput(amount);
                    inputSocket.ressources[0]._name = res._name;
                    res.decrementCount(amount);
                }
            }
        }

        private void moveToOutput(int value)
        {
            string res = CalcRessource(inputSocket.ressources[0]._name);

            outputSocket.ressource._name = res;
            outputSocket.ressource.ownedAmount += value;

            outputText.text = outputSocket.ressource.ownedAmount.ToString();

            outputRessource.text = res;

            outputSocket.SetValue(outputSocket.getRessource());
            addDisplayInput(-value);
        }

        private void addDisplayInput(int value)
        {
            int valueNow = int.Parse(inputText.text);
            int newValue = valueNow + value;
            inputText.text = newValue.ToString();
            Debug.Log(valueNow);
            inputSocket.ressources[0].ownedAmount = newValue;

            if (newValue == 0)
            {
                inputSocket.ressources[0]._name = null;
                OnConnectedValueUpdated();
            }
            inputRessource.text = inputSocket.ressources[0]._name;
        }

        private string CalcRessource(string s)
        {
            foreach (FurnaceRecepie fure in recepieList)
            {
                if (fure.InputRessources[0]._name == s)
                {
                    return fure.OutputRessources[0]._name;
                }
            }
            return null;
        }

        private void FixedUpdate()
        {
            int inputAmount = int.Parse(inputText.text);
            if (inputAmount > 0)
            {
                if (fixedUpdateCount%10 == 0 && inputAmount < internalInventory)
                {
                    if (inputAmount + tranferAmount <= internalInventory)
                    {
                        transfer(tranferAmount);
                    }
                    else
                    {
                        int newTransferAmount = inputAmount + tranferAmount - internalInventory;
                        transfer(newTransferAmount);
                    }
                }
                else if (fixedUpdateCount % 25 == 0 && int.Parse(outputText.text) < internalInventory)
                {
                    if ((outputSocket.ressource._name == CalcRessource(inputRessource.text) || outputSocket.ressource._name == null) && (inputSocket.ressources[0].ownedAmount > 0 || outputSocket.ressource.ownedAmount == 0))
                    { 
                        moveToOutput(1);
                    }
                }
            }
            if (outputSocket.ressource.ownedAmount <= 0 && outputRessource.text != null)
            {
                outputText.text = "0";
                outputRessource.text = null;
            }

            fixedUpdateCount %= 10000;
            fixedUpdateCount++;
            
        }
    }
}
