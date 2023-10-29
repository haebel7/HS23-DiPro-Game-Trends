using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeNodeEditor
{
    [CreateAssetMenu(fileName = "RecepieData", menuName = "ScriptableObjects/Recepie", order = 1)]
    public class Recipe : ScriptableObject
    {
        public List<Ressource> InputRessources;
        public List<Ressource> OutputRessources;
        // in seconds
        public float processingTime;

        public List<Ressource> getInputRessources()
        {
            return InputRessources;
        }

        public List<Ressource> getOutputRessources() 
        { 
            return OutputRessources;
        }

        public float getProcessingTime()
        {
            return processingTime;
        }
    }
}
