using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeNodeEditor
{
    [CreateAssetMenu(fileName = "RessourceData", menuName = "Ressource", order = 1)]

    public class Ressource : ScriptableObject
    {
        public string _name;
        public int ownedAmount;

        public void decrementCount(int value)
        { 
            if (ownedAmount - value >= 0)
            {
                ownedAmount -= value;
            }
        }

        public void incrementCount(int value)
        {
            ownedAmount += value;
        }
    }
}
