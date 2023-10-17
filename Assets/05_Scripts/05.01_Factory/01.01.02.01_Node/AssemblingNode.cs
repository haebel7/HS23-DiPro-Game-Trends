using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeNodeEditor
{
    public class AssemblingNode : Node
    {
        public SocketInput inputSocket1;
        public SocketInput inputSocket2;
        public SocketOutput outputSocket;

        public TMP_Text inputRessourceNum1;
        public TMP_Text inputRessourceNum2;
        public TMP_Text inputRessourceText1;
        public TMP_Text inputRessourceText2;
        public TMP_Text outputRessourceNum;
        public TMP_Text outputRessourceText;
        public TMP_Text processingTimeText;

        public TMP_Dropdown dropdown;

        public Button trashButton;

        public int internalInputInventory;
        public int internalOutputInventory;
        public int processingTime;

        public List<Recipe> recipeList;

        private List<IOutput> _incomingOutputs;
        private List<Ressource> incomingValues;
        private int fixedUpdateCount = 0;
        private int timer = 0;
        private bool timerIsRunning = false;
        public override void Setup()
        {
            _incomingOutputs = new List<IOutput>();

            Register(inputSocket1);
            Register(inputSocket2);
            Register(outputSocket);

            SetHeader("Assembling");

            inputRessourceNum1.text = "0";
            inputRessourceNum2.text = "0";
            outputRessourceNum.text = "0";
            processingTimeText.text = "0";
            inputRessourceText1.text = null;
            inputRessourceText2.text = null;
            outputRessourceText.text = null;

            trashButton.onClick.AddListener(trashInput);

            OnConnectionEvent += OnConnection;
            OnDisconnectEvent += OnDisconnect;

            dropdown.AddOptions(new List<TMP_Dropdown.OptionData>()
            {
                new TMP_Dropdown.OptionData("Circuit"),
                new TMP_Dropdown.OptionData("Cringe")
            });

            dropdown.onValueChanged.AddListener(selected =>
            {
                //HandleFieldValue();
            });

            //HandleFieldValue();
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
            int counter = 0;

            foreach (var c in _incomingOutputs)
            {
                Ressource res = c.GetValue<Ressource>();
                incomingValues.Add(res);

                if (counter == 0)
                {
                    int inputAmount = int.Parse(inputRessourceNum1.text);
                    if (inputAmount < internalInputInventory)
                    {

                        transferToInput(internalInputInventory - inputAmount, inputRessourceNum1, inputRessourceText1, res);
                    }
                }
                else if (counter == 1)
                {
                    int inputAmount = int.Parse(inputRessourceNum2.text);
                    if (inputAmount < internalInputInventory)
                    {
                        transferToInput(internalInputInventory - inputAmount, inputRessourceNum2, inputRessourceText2, res);
                    }
                }
                counter++;
            }
        }

        private void transferToInput(int amount, TMP_Text textNum, TMP_Text textRessource, Ressource r)
        {
            if (r.canDecrement(amount))
            {
                updateInput(amount, r);
            }
            textNum.text = amount.ToString();
            r.ownedAmount -= amount;

            textRessource.text = r._name;
        }

        private void updateInput(int amount, Ressource r)
        {

        }

        private void updateProcessingTimeText()
        {
            /*if (timerIsRunning && outputSocket.ressource.ownedAmount < internalOutputInventory)
            {
                if (timer <= 0)
                {
                    timerIsRunning = false;
                    bool b1 = (outputSocket.ressource._name == CalcRessource(inputRessource.text) || outputSocket.ressource._name == null);
                    bool b2 = (inputSocket.ressources[0].ownedAmount > 0 || outputSocket.ressource.ownedAmount == 0);
                    if (b1 && b2)
                    {
                        moveToOutput(1);
                    }
                }
                else
                {
                    timer--;
                    float f = timer / 10f;
                    processingTimeText.text = f.ToString() + " sec";
                }
            }
            else if (int.Parse(inputText.text) > 0 && !timerIsRunning && outputSocket.ressource.ownedAmount < internalOutputInventory)
            {
                timerIsRunning = true;
                timer = processingTime;
                float f = timer / 10f;
                processingTimeText.text = f.ToString() + " sec";
            }*/
        }

        private void trashInput()
        {
            inputRessourceNum1.text = "0";
            inputRessourceNum2.text = "0";
            inputRessourceText1.text = null;
            inputRessourceText2.text = null;
        }

        private void FixedUpdate()
        {
            if (fixedUpdateCount % 5 == 0)
            {
                updateProcessingTimeText();

                int inputAmount1 = int.Parse(inputRessourceNum1.text);
                int inputAmount2 = int.Parse(inputRessourceNum2.text);

                if (fixedUpdateCount % 50 == 0)
                {
                    if (inputAmount1 < internalInputInventory && inputAmount1 > 0)
                    {
                        try
                        {
                            int a = internalInputInventory - inputAmount1;
                            transferToInput(a, inputRessourceNum1, inputRessourceText1, incomingValues[0]);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                    }
                    else if (inputAmount2 < internalInputInventory && inputAmount2 > 0)
                    {
                        try
                        {
                            int a = internalInputInventory - inputAmount2;
                            transferToInput(a, inputRessourceNum2, inputRessourceText2, incomingValues[1]);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                    }
                }
            }
        }
    }
}
