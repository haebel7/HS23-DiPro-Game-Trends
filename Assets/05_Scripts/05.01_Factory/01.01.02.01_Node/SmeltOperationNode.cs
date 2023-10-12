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
        public TMP_Text processingTimeText;
        public RessourceInventar ressourceInventory;
        public int internalInputInventory;
        public int internalOutputInventory;
        public int processingTime;
        public List<FurnaceRecepie> recepieList;

        private List<IOutput> _incomingOutputs;
        private List<Ressource> incomingValues;
        private int fixedUpdateCount = 0;
        private int timer = 0;
        private bool timerIsRunning = false;

        // Setup is called when first creating the Node
        public override void Setup()
        {
            _incomingOutputs = new List<IOutput>();

            Register(inputSocket);
            Register(outputSocket);

            SetHeader("Furnace");

            inputText.text = "0";
            outputText.text = "0";
            processingTimeText.text = "0";
            inputRessource.text = null;
            outputRessource.text = null;

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

            int inputAmount = int.Parse(inputText.text);
            if (inputAmount < internalInputInventory)
            {
                transferToInput(internalInputInventory - inputAmount);
            }
        }

        // Gets Ressources from the Inventory and moves them in to the Input
        private void transferToInput(int amount)
        {
            if (incomingValues.Count > 0)
            {
                int index = ressourceInventory.getRessourceIndex(incomingValues[0]._name);
                Ressource res = ressourceInventory.getListOfRessources()[index];
                if (res.canDecrement(amount) && (inputSocket.ressources[0]._name == res._name || inputSocket.ressources[0]._name == null))
                {
                    updateInput(amount);
                    inputSocket.ressources[0]._name = res._name;
                    res.decrementCount(amount);
                }
                else if (!res.canDecrement(amount) && (inputSocket.ressources[0]._name == res._name || inputSocket.ressources[0]._name == null))
                {
                    int newAmount = res.ownedAmount;
                    updateInput(newAmount);
                    inputSocket.ressources[0]._name = res._name;
                    res.decrementCount(newAmount);
                }
            }
        }

        // Moves Ressources from the Input to the Output. Additionally updates the Output Socket
        private void moveToOutput(int value)
        {
            string res = CalcRessource(inputSocket.ressources[0]._name);

            outputSocket.ressource._name = res;
            outputSocket.ressource.ownedAmount += value;

            outputText.text = outputSocket.ressource.ownedAmount.ToString();

            outputRessource.text = res;

            outputSocket.SetValue(outputSocket.getRessource());
            updateInput(-value);
        }

        // Update the Input Text and Count
        private void updateInput(int value)
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

        // Check from the Recipe List which Ressource should be smelted
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

        private void updateProcessingTimeText()
        {
            if (timerIsRunning && outputSocket.ressource.ownedAmount < internalOutputInventory)
            {
                if (timer <= 0)
                {
                    timerIsRunning = false;
                }
                else
                {
                    timer--;
                    float f = timer / 50f;
                    f = (float)(Math.Round(f * 10f) * 0.1f);
                    processingTimeText.text = f.ToString() + " sec";
                }
            }
            else if (int.Parse(inputText.text) > 0 && !timerIsRunning)
            {
                timerIsRunning = true;
                timer = processingTime;
                float f = timer / 50f;
                f = (float)(Math.Round(f * 10f) * 0.1f);
                processingTimeText.text = f.ToString() + " sec";
            }
        }


        private void FixedUpdate()
        {
            int inputAmount = int.Parse(inputText.text);
            //int timer = processingTime;
            if (inputAmount > 0)
            {
                if (fixedUpdateCount%50 == 0 && inputAmount < internalInputInventory)
                {
                    transferToInput(internalInputInventory - inputAmount);                
                }
                else if (timer <= 0 && int.Parse(outputText.text) < internalOutputInventory && !timerIsRunning)
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

            updateProcessingTimeText();

            fixedUpdateCount %= 10000;
            fixedUpdateCount++;
            
        }
    }
}
