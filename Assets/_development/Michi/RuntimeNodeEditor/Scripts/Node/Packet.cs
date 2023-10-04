using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeNodeEditor
{
    public abstract class Packet : MonoBehaviour
    {
        public int itemCount { get; set; }
        public string ressource { get; set; }
        public int stackSize { get; set; }
        //private int priority;

        public void setAttributes(int itemCount, string ressource, int stackSize)
        {
            this.itemCount = itemCount;
            this.ressource = ressource;
            this.stackSize = stackSize;
            //this.priority = priority;
        }
    }
}
