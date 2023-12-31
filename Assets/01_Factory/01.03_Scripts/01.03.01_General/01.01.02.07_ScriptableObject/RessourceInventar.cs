using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace RuntimeNodeEditor
{
    [CreateAssetMenu(fileName = "RessourceInventar", menuName = "ScriptableObjects/RessourceInventar", order = 1)]
    public class RessourceInventar : ScriptableObject
    {
        public List<Ressource> ressources = new List<Ressource>();

        public int getRessourceIndex(string _name)
        {
            int index = 0;
            foreach (Ressource x in ressources)
            {
                if (x._name == _name)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public List<Ressource> getListOfRessources()
        {
            return ressources;
        }
    }
}
