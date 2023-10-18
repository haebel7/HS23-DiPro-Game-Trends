using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeNodeEditor
{
    public class AssemblingNode : Node
    {
        public SocketInput inputSocket1;
        public SocketInput inputSocket2;
        public SocketOutput outputSocket;


        [SerializeField] private TMP_Text inputRessourceNum1;
        [SerializeField] private TMP_Text inputRessourceNum2;
        [SerializeField] private TMP_Text inputRessourceText1;
        [SerializeField] private TMP_Text inputRessourceText2;
        [SerializeField] private TMP_Text outputRessourceNum;
        [SerializeField] private TMP_Text outputRessourceText;
        [SerializeField] private TMP_Text processingTimeText;

        [SerializeField] private TMP_Dropdown dropdown;

        [SerializeField] private Button retrieveButton;
        public RessourceInventar ressourceInventar;

        [SerializeField] private int internalInputInventory;
        [SerializeField] private int internalOutputInventory;
        [SerializeField] private int processingTime;

        public List<Recipe> recipeList;
        private Ressource copyInputRessource1; 
        private Ressource copyInputRessource2;

        private List<IOutput> _incomingOutputs;
        private List<SocketInput> _listOfInputs;
        private List<Ressource> incomingValues;
        private SocketInput socketInput;
        private int index = 0;
        private int fixedUpdateCount = 0;
        private int timer = 0;
        private bool timerIsRunning = false;
        public override void Setup()
        {
            _incomingOutputs = new List<IOutput>();
            _listOfInputs = new List<SocketInput>();

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

            retrieveButton.onClick.AddListener(retrieveInput);

            OnConnectionEvent += OnConnection;
            OnDisconnectEvent += OnDisconnect;

            List<TMP_Dropdown.OptionData> dropdownList = new List<TMP_Dropdown.OptionData>();
            foreach (Recipe r in recipeList)
            {
                dropdownList.Add(new TMP_Dropdown.OptionData(r.name));
            }

            dropdown.AddOptions(dropdownList);

            dropdown.onValueChanged.AddListener(selected =>
            {
                if (getRecipe().getOutputRessources()[0]._name == outputSocket.ressource._name || outputSocket.ressource._name == null)
                {
                    timer = processingTime;
                }
                else
                {
                    timer = 0;
                    processingTimeText.text = "0 sec";
                }
            });
        }

        // Function that is called after you connect a Node to this one
        public void OnConnection(SocketInput input, IOutput output)
        {
            socketInput = input;
            output.ValueUpdated += OnConnectedValueUpdated;
            _incomingOutputs.Add(output);
            _listOfInputs.Add(input);

            OnConnectedValueUpdated();
        }

        // Function that is called after you disconnect any of your linked Nodes
        public void OnDisconnect(SocketInput input, IOutput output)
        {
            output.ValueUpdated -= OnConnectedValueUpdated;
            _incomingOutputs.Remove(output);
            _listOfInputs.Remove(input);
            index = 0;

            OnConnectedValueUpdated();
        }

        // Saves the given values from every connected Output Socket in a List
        private void OnConnectedValueUpdated()
        {
            incomingValues = new List<Ressource>();
            index = 0;

            foreach (var c in _incomingOutputs)
            {
                Ressource res = c.GetValue<Ressource>();
                incomingValues.Add(res);

                if (_listOfInputs[index].name == "SocketInput1" && inputSocket1.ressources.Count > 0)
                {
                    int inputAmount = inputSocket1.ressources[0].ownedAmount;
                    bool b1 = inputSocket1.ressources[0]._name == res.name || inputSocket1.ressources[0]._name == null;
                    if (inputAmount < internalInputInventory && b1 && inputSocket1.ressources[0]._name == inputRessourceText1.text)
                    {
                        Debug.Log(inputSocket1.ressources[0]._name + ", " + res.name);
                        inputSocket1.ressources[0].incrementCount(internalInputInventory - inputAmount);
                        inputSocket1.ressources[0]._name = res._name;
                        transferToInput(internalInputInventory - inputAmount, inputRessourceNum1, inputRessourceText1, res, inputSocket1);
                    }
                }
                else if (inputSocket2.ressources.Count > 0)
                {
                    int inputAmount = inputSocket2.ressources[0].ownedAmount;
                    bool b1 = inputSocket2.ressources[0]._name == res.name || inputSocket2.ressources[0]._name == null;
                    if (inputAmount < internalInputInventory && b1 && inputSocket2.ressources[0]._name == inputRessourceText2.text)
                    {
                        Debug.Log(inputSocket2.ressources[0]._name + ", " + res.name);
                        inputSocket2.ressources[0].incrementCount(internalInputInventory - inputAmount);
                        inputSocket2.ressources[0]._name = res._name;
                        transferToInput(internalInputInventory - inputAmount, inputRessourceNum2, inputRessourceText2, res, inputSocket2);
                    }
                }
                index++;
            }
        }

        private void transferToInput(int amount, TMP_Text textNum, TMP_Text textRessource, Ressource r, SocketInput socketInput)
        {
            if (r.canDecrement(amount))
            {
                r.decrementCount(amount);
                textNum.text = socketInput.ressources[0].ownedAmount.ToString();

                textRessource.text = r._name;
            }
        }

        private Recipe getRecipe()
        {
            foreach (Recipe r in recipeList)
            {
                if (r.name == dropdown.options[dropdown.value].text)
                {
                    return r;
                }
            }
            return null;
        }

        private bool checkOutputName(Recipe recipe)
        {
            return outputSocket.ressource._name == null || outputSocket.ressource._name == recipe.getOutputRessources()[0]._name;
        }
        
        private void moveToOutput()
        {
            Recipe recipe = getRecipe();
            int outputCount = outputSocket.ressource.ownedAmount + recipe.getOutputRessources().Count;
            if (checkIfCanDoRecipe() && outputCount <= internalOutputInventory && checkOutputName(recipe))
            {
                for (int i = 0; i <= 1; i++)
                {
                    Ressource r = getRessourceName(i);
                    if (inputSocket1.ressources[0]._name == r._name)
                    {
                        int amount = r.ownedAmount;
                        string name = r._name;
                        inputSocket1.ressources[0].decrementCount(inputSocket1.ressources[0].ownedAmount - amount);
                        inputSocket1.ressources[0]._name = name;
                        inputRessourceNum1.text = inputSocket1.ressources[0].ownedAmount.ToString();
                        inputRessourceText1.text = name;

                        if (r.ownedAmount == 0)
                        {
                            inputRessourceText1.text = null;
                            inputSocket1.ressources[0]._name = null;
                        }
                    }
                    if (inputSocket2.ressources[0]._name == r._name)
                    {
                        int amount = r.ownedAmount;
                        string name = r._name;
                        inputSocket2.ressources[0].decrementCount(inputSocket2.ressources[0].ownedAmount - amount);
                        inputSocket2.ressources[0]._name = name;
                        inputRessourceNum2.text = inputSocket2.ressources[0].ownedAmount.ToString();
                        inputRessourceText2.text = name;

                        if (r.ownedAmount == 0)
                        {
                            inputRessourceText2.text = null;
                            inputSocket2.ressources[0]._name = null;
                        }
                    }
                }
                outputSocket.ressource.incrementCount(recipe.getOutputRessources().Count);
                outputSocket.ressource._name = recipe.getOutputRessources()[0]._name;
                outputSocket.SetValue(outputSocket.getRessource());
                outputRessourceNum.text = outputSocket.ressource.ownedAmount.ToString();
                outputRessourceText.text = outputSocket.ressource._name;

            }
        }

        private Ressource getRessourceName(int i)
        {
            switch (i)
            {
                case 0: return copyInputRessource1;
                case 1: return copyInputRessource2;
                default: return null;
            }
        }

        private bool checkIfCanDoRecipe()
        {
            Recipe recipe = getRecipe();
            if (recipe != null)
            {
                //Copy of Input Ressources
                copyInputRessource1 = Instantiate(inputSocket1.ressources[0]);
                copyInputRessource2 = Instantiate(inputSocket2.ressources[0]);

                foreach (Ressource r in recipe.getInputRessources())
                {
                    if (copyInputRessource1._name == r._name)
                    {
                        copyInputRessource1.ownedAmount--;
                    }
                    else if (copyInputRessource2._name == r._name)
                    {
                        copyInputRessource2.ownedAmount--;
                    }
                    else
                    {
                        return false;
                    }
                }

                // Checks if you have enough ressources in the input
                if (copyInputRessource1.ownedAmount >= 0 && copyInputRessource2.ownedAmount >= 0)
                {
                    return true;
                }
                else 
                { 
                    return false; 
                }
            }
            else
            {
                return false;
            }

        }

        private void updateProcessingTimeText()
        {
            bool b = outputSocket.ressource.ownedAmount < internalOutputInventory;
            if (timerIsRunning && b)
            {
                if (timer <= 0)
                {
                    timerIsRunning = false;
                    if (checkIfCanDoRecipe())
                    {
                        moveToOutput();
                    }
                }
                else
                {
                    timer--;
                    float f = timer / 10f;
                    processingTimeText.text = f.ToString() + " sec";
                }
            }
            else if (!timerIsRunning && b && inputSocket1.ressources[0].ownedAmount > 0 && inputSocket2.ressources[0].ownedAmount > 0 && checkIfCanDoRecipe() && checkOutputName(getRecipe()))
            {
                timerIsRunning = true;
                timer = processingTime;
                float f = timer / 10f;
                processingTimeText.text = f.ToString() + " sec";
            }
        }

        private void retrieveInput()
        {
            index = 0;
            //Debug.Log(inputSocket1.ressources[0].ownedAmount + ", " + inputSocket2.ressources[0].ownedAmount);
            if (inputSocket1.ressources[0].ownedAmount > 0)
            {
                foreach (Ressource r in ressourceInventar.getListOfRessources()) {
                    if (r._name == inputRessourceText1.text)
                    {
                        Debug.Log(inputRessourceNum1.text);
                        r.incrementCount(int.Parse(inputRessourceNum1.text));
                        inputRessourceNum1.text = "0";
                        inputRessourceText1.text = null;
                        inputSocket1.ressources[0].ownedAmount = 0;
                        inputSocket1.ressources[0]._name = null;
                    }
                }
            }

            if (inputSocket2.ressources[0].ownedAmount > 0)
            {
                foreach (Ressource r in ressourceInventar.getListOfRessources())
                {
                    if (r._name == inputRessourceText2.text)
                    {
                        Debug.Log(inputRessourceNum2.text);
                        r.incrementCount(int.Parse(inputRessourceNum2.text));
                        inputRessourceNum2.text = "0";
                        inputRessourceText2.text = null;
                        inputSocket2.ressources[0].ownedAmount = 0;
                        inputSocket2.ressources[0]._name = null;
                    }
                }
            }
            OnConnectedValueUpdated();
        }

        private void FixedUpdate()
        {
            if (outputSocket.ressource.ownedAmount <= 0 && outputRessourceText.text != null)
            {
                outputRessourceNum.text = "0";
                outputRessourceText.text = null;
            }
            Debug.Log(inputSocket1.ressources[0].ownedAmount + ", " + inputSocket1.ressources[0]._name + ", " + inputSocket2.ressources[0].ownedAmount + ", " + inputSocket2.ressources[0]._name);

            if (fixedUpdateCount % 5 == 0)
            {
                updateProcessingTimeText();

                if (fixedUpdateCount % 50 == 0)
                {
                    OnConnectedValueUpdated();
                }
            }
            fixedUpdateCount++;
            fixedUpdateCount %= 10000;
        }
    }
}
