using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeNodeEditor
{
    [CreateAssetMenu(fileName = "RessourceData", menuName = "ScriptableObjects/Ressource", order = 1)]

    public class Ressource : ScriptableObject
    {
        public string _name;
        public int ownedAmount;
        public Sprite icon;

        public void decrementCount(int value)
        { 
            if (canDecrement(value))
            {
                ownedAmount -= value;
            }
        }
        public bool canDecrement(int value)
        {
            if (ownedAmount - value >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void incrementCount(int value)
        {
            ownedAmount += value;
        }
    }
}
