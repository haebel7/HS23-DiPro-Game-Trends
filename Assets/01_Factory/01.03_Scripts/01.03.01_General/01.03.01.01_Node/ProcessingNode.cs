using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeNodeEditor
{
    public class ProcessingNode : Node
    {
        //*****************ORDER IS VERY IMPORTAT THAT IT MATCHES********************
        [SerializeField] private List<SocketInput>      _listOfInputs;
        [SerializeField] private List<SocketOutput>     _listOfOutputs;
        [SerializeField] private List<TMP_Text>         _listOfInputTexts;
        [SerializeField] private List<TMP_Text>         _listOfOutputTexts;
        [SerializeField] private List<TMP_Text>         _listOfOutputNums;
        [SerializeField] private List<TMP_Text>         _listOfInputNums;
        [SerializeField] private List<Recipe>           _listOfRecipes;
        //***************************************************************************

        [SerializeField] private TMP_Dropdown           _dropdown;
        [SerializeField] private Button                 _retrieveButton;
        [SerializeField] private RessourceInventar      _ressourceInventory;
        [SerializeField] private string                 _header;
        [SerializeField] private int                    _internalInputInventory;
        [SerializeField] private int                    _internalOutputInventory;
        // in 0.1 sec
        [SerializeField] private int                    _outputMultiplier;
        [SerializeField] private TMP_Text               _processingTimeText;



        private List<IOutput>                           incomingOutputs;
        private List<SocketInput>                       orderOfInputs;
        private List<Ressource>                         incomingValues;
        private List<Ressource>                         recipeInputs;
        private List<Ressource>                         recipeOutputs;
        private List<Ressource>                         ressourceInputsCopy;
        private int                                     processingTime;
        private int                                     fixedUpdateCount = 0;
        private int                                     timer = 0;
        private bool                                    timerIsRunning = false;


        // Setup is called when first creating the Node
        public override void Setup()
        {
            incomingOutputs =       new List<IOutput>();
            orderOfInputs =         new List<SocketInput>();
            incomingValues =        new List<Ressource>();
            recipeInputs =          new List<Ressource>();
            recipeOutputs =         new List<Ressource>();
            ressourceInputsCopy =   new List<Ressource>();

            SetHeader(_header);

            foreach (SocketInput socketInput in _listOfInputs)
            {
                Register(socketInput);
                socketInput.ressources[0]._name = null;
                socketInput.ressources[0].ownedAmount = 0;
            }
            foreach (SocketOutput socketOutput in _listOfOutputs)
            {
                Register(socketOutput);
                socketOutput.ressource._name = null;
                socketOutput.ressource.ownedAmount = 0;
            }
            foreach (TMP_Text t in _listOfInputTexts)
            {
                t.text = null;
            }
            foreach (TMP_Text t in _listOfOutputTexts)
            {
                t.text = null;
            }
            foreach (TMP_Text t in _listOfInputNums)
            {
                t.text = "0";
            }
            foreach (TMP_Text t in _listOfOutputNums)
            {
                t.text = "0";
            }

            _processingTimeText.text = "0 sec";
            processingTime = 0;

            OnConnectionEvent += OnConnection;
            OnDisconnectEvent += OnDisconnect;

            _retrieveButton.onClick.AddListener(retrieveInput);

            // for those Nodes, which have a dropdown; if not skip
            if (_dropdown != null)
            {
                // Fills the Dropdown with options from the given Recipes
                List<TMP_Dropdown.OptionData> dropdownList = new List<TMP_Dropdown.OptionData>();
                foreach (Recipe r in _listOfRecipes)
                {
                    dropdownList.Add(new TMP_Dropdown.OptionData(r.name));
                }

                _dropdown.AddOptions(dropdownList);

                _dropdown.onValueChanged.AddListener(selected =>
                {
                    int index = 0;
                    prepRecipe();
                    foreach (SocketOutput socketOutput in _listOfOutputs)
                    {
                        try
                        {
                            if (recipeOutputs[index]._name == socketOutput.ressource._name || socketOutput.ressource._name == null)
                            {
                                timer = processingTime;
                            }
                            else
                            {
                                timer = 0;
                                _processingTimeText.text = "0 sec";
                            }
                            index++;
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                });
                prepRecipe();
            }
        }

        // Function that is called after you connect a Node to this one
        public void OnConnection(SocketInput input, IOutput output)
        {
            output.ValueUpdated += OnConnectedValueUpdated;
            orderOfInputs.Add(input);
            incomingOutputs.Add(output);

            OnConnectedValueUpdated();
        }

        // Function that is called after you disconnect any of your linked Nodes
        public void OnDisconnect(SocketInput input, IOutput output)
        {
            output.ValueUpdated -= OnConnectedValueUpdated;
            orderOfInputs.Remove(input);
            incomingOutputs.Remove(output);

            OnConnectedValueUpdated();
        }

        // Saves the given values from every connected Output Socket in a List
        private void OnConnectedValueUpdated()
        {
            incomingValues = new List<Ressource>();

            foreach (var c in incomingOutputs)
            {
                Ressource res = c.GetValue<Ressource>();
                incomingValues.Add(res);

            }
            foreach (SocketInput si in _listOfInputs)
            {
                int index = 0;
                foreach (SocketInput osi in orderOfInputs)
                {
                    if (osi.Equals(si))
                    {
                        int inputAmount = si.ressources[0].ownedAmount;
                        if (inputAmount < _internalInputInventory && _internalInputInventory - inputAmount > 0)
                        {
                            transferToInput(_listOfInputs.IndexOf(si), _internalInputInventory - inputAmount, incomingValues[index]);
                        }
                    }
                    index++;
                }
            }
        }

        // Gets Ressources from the last Node and moves them in to the Input
        private void transferToInput(int index, int amount, Ressource r)
        {
            try
            {
                if (incomingValues.Count > 0)
                {
                    string socketInputName = _listOfInputs[index].ressources[0]._name;
                    bool boolNull = socketInputName == null;
                    bool boolEqualName = socketInputName == r._name;
                    if (boolNull || boolEqualName)
                    {
                        if (r.ownedAmount < amount)
                        {
                            amount = r.ownedAmount;
                        }
                        _listOfInputs[index].ressources[0].incrementCount(amount);
                        r.decrementCount(amount);
                        _listOfInputNums[index].text = _listOfInputs[index].ressources[0].ownedAmount.ToString(); 
                        if (boolNull)
                        {
                            string resName = r._name;
                            _listOfInputs[index].ressources[0]._name = resName;
                            _listOfInputTexts[index].text = resName;
                            prepRecipe();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
            }
        }

        // Moves Ressources from the Input to the Output. Additionally updates the Output Socket
        private void moveToOutput(int outputMultiplier)
        {
            int index = 0;
            foreach (SocketInput si in _listOfInputs)
            {
                foreach (Ressource r in recipeInputs)
                {
                    if (si.ressources[0]._name == r._name)
                    {
                        si.ressources[0].decrementCount(r.ownedAmount * outputMultiplier);
                        _listOfInputNums[index].text = si.ressources[0].ownedAmount.ToString();
                        if (si.ressources[0].ownedAmount <= 0 && int.Parse(_listOfInputNums[index].text) <= 0)
                        {
                            si.ressources[0]._name = null;
                            _listOfInputTexts[index].text = null;
                        }
                        break;
                    }
                }
                index++;
            }

            index = 0;
            foreach (SocketOutput so in _listOfOutputs)
            {
                foreach (Ressource r in recipeOutputs)
                {
                    if (so.ressource._name == r._name || so.ressource._name == null)
                    {
                        so.ressource.incrementCount(r.ownedAmount * outputMultiplier);
                        _listOfOutputNums[index].text = so.ressource.ownedAmount.ToString();
                        if (so.ressource._name == null && _listOfOutputTexts[index].text == null)
                        {
                            so.ressource._name = r._name;
                            _listOfOutputTexts[index].text = r._name;
                        }
                        else if (so.ressource._name != _listOfOutputTexts[index].text)
                        {
                            _listOfOutputTexts[index].text = so.ressource._name;
                        }
                        so.SetValue(so.getRessource());
                        break;
                    }
                }
                index++;
            }
        }

        // Checks if the specified Recipe is achievable with the Ressources in the Inputs
        private bool checkIfCanDoRecipe(int howMuchUWantToDo)
        {
            ressourceInputsCopy.Clear();
            if (recipeInputs.Count != 0 && recipeOutputs.Count != 0)
            {
                int index = 0;
                foreach (SocketInput si in _listOfInputs)
                {
                    bool boboo = true;
                    ressourceInputsCopy.Add(Instantiate(si.ressources[0]));
                    foreach (Ressource r in recipeInputs)
                    {
                        if (ressourceInputsCopy[index]._name == r._name)
                        {
                            ressourceInputsCopy[index].ownedAmount -= r.ownedAmount * howMuchUWantToDo;
                            r._name = "done";
                            boboo = true;
                            break;
                        }
                        else
                        {
                            boboo = false;
                        }
                    }
                    if (!boboo)
                    {
                        return false;
                    }
                    index++;
                }
                // Checks if you have enough ressources in the input
                foreach (Ressource r in ressourceInputsCopy)
                {
                    if (r.ownedAmount < 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        // Checks if the Outputinventory is occupied
        private bool checkOutputStatus(int howMuchUWantToDo)
        {
            int index = 0;
            foreach (SocketOutput so in _listOfOutputs)
            {
                bool boolName = so.ressource._name != null || so.ressource._name != recipeOutputs[index]._name;
                if (boolName && so.ressource.ownedAmount + (recipeOutputs[index].ownedAmount * howMuchUWantToDo) > _internalOutputInventory)
                {
                    return false;
                }
                index++;
            }
            return true;
        }

        // Updates the Text, which displays the remaining time of the Task
        private void updateProcessingTimeText()
        {
            if (timerIsRunning && checkOutputStatus(_outputMultiplier))
            {
                if (timer <= 0)
                {
                    timerIsRunning = false;
                    prepRecipe();
                    moveToOutput(_outputMultiplier);
                }
                else
                {
                    timer--;
                    float f = timer / 10f;
                    _processingTimeText.text = f.ToString() + " sec";
                }
            }
            else if (!timerIsRunning && checkIfCanDoRecipe(_outputMultiplier) && checkOutputStatus(_outputMultiplier))
            {
                timerIsRunning = true;
                timer = processingTime;
                float f = timer / 10f;
                _processingTimeText.text = f.ToString() + " sec";
            }
        }

        private Recipe getRecipeInDropdown()
        {
            foreach (Recipe r in _listOfRecipes)
            {
                if (r.name == _dropdown.options[_dropdown.value].text)
                {
                    return r;
                }
            }
            return null;
        }

        // Loads the chosen Recipe in the Dropdown or searches the right Recipe (only with 1 input)
        // and counts how many Ressources is needed for the Recipe.
        // Saves the result in "recipeInputs" and "recipeOutputs" Lists
        private void prepRecipe()
        {
            recipeInputs.Clear();
            recipeOutputs.Clear();

            foreach(SocketInput si in _listOfInputs)
            {
                Ressource res = Instantiate(si.ressources[0]);
                res.ownedAmount = 0;
                res._name = null;
                recipeInputs.Add(res);
            }
            foreach(SocketOutput so in _listOfOutputs)
            {
                Ressource res = Instantiate(so.ressource);
                res.ownedAmount = 0;
                res._name = null;
                recipeOutputs.Add(res);
            }
            // Checks if there is a dropdown, which tells what Recipe it should listen to
            if (_dropdown != null)
            {
                Recipe recipe = getRecipeInDropdown();
                setProcessingTime(recipe);
                //Count Recipe Inputressources
                doPrepRecipeLoop(recipe.getInputRessources(), recipeInputs);
                // Count Recipe Outputressources
                doPrepRecipeLoop(recipe.getOutputRessources(), recipeOutputs);
            }
            // if there is only one input, it only has to find the right recipe and count the in- and output ressources of the recipe
            if (_listOfInputs.Count == 1)
            {
                foreach (Recipe r in _listOfRecipes)
                {
                    if (_listOfInputs[0].ressources[0]._name == r.getInputRessources()[0]._name)
                    {
                        setProcessingTime(r);
                        doPrepRecipeLoop(r.getInputRessources(), recipeInputs);
                        doPrepRecipeLoop(r.getOutputRessources(), recipeOutputs);
                    }
                }
            }
        }

        private void setProcessingTime(Recipe recipe)
        {
            processingTime = (int)Math.Floor(recipe.getProcessingTime() * 10);
            Debug.Log(processingTime);
        }

        // helper function from prepRecipe(). It does the counting of the Recipe 
        private void doPrepRecipeLoop(List<Ressource> recipe, List<Ressource> preppedRecipeList)
        {
            foreach (Ressource r in recipe)
            {
                foreach (Ressource res in preppedRecipeList)
                {
                    if (res._name == r._name || res._name == null)
                    {
                        res.ownedAmount++;
                        if (res._name == null)
                        {
                            res._name = r._name;
                        }
                        break;
                    }
                }
            }
        }

        private Ressource searchRessourceInInventory(Ressource r)
        {
            foreach (Ressource res in _ressourceInventory.getListOfRessources())
            {
                if (res._name == r._name)
                {
                    return res;
                }
            }
            return null;
        }

        private void retrieveInput()
        {
            foreach (SocketInput si in _listOfInputs)
            {
                Ressource r = searchRessourceInInventory(si.ressources[0]);
                r.incrementCount(si.ressources[0].ownedAmount);
                si.ressources[0]._name = null;
                si.ressources[0].ownedAmount = 0;
            }
            foreach (TMP_Text t in _listOfInputTexts)
            {
                t.text = null;
            }
            foreach (TMP_Text t in _listOfInputNums)
            {
                t.text = "0";
            }

            timer = 0;
            _processingTimeText.text = "0 sec";
        }

        private void FixedUpdate()
        {
            if (fixedUpdateCount % 5 == 0)
            {
                int index = 0;
                // if the next connected node sucks out the ressources, the text doesnt update. quick and dirty solution:
                foreach (SocketOutput so in _listOfOutputs)
                {
                    if (so.ressource.ownedAmount <= 0 && _listOfOutputTexts[index].text != null)
                    {
                        _listOfOutputTexts[index].text = null;
                        _listOfOutputNums[index].text = "0";
                    }
                    index++;
                }
                updateProcessingTimeText();
                if (fixedUpdateCount % 50 == 0)
                {
                    OnConnectedValueUpdated();
                }
            }

            fixedUpdateCount %= 10000;
            fixedUpdateCount++;
        }
    }
}
