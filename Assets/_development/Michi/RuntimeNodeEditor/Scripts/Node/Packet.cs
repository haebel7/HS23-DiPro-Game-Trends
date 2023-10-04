using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeNodeEditor
{
    public abstract class Packet : MonoBehaviour
    {
        public string ressource { get; set; }
        public int stackSize { get; set; }

        public void setAttributes(string ressource, int stackSize)
        {
            this.ressource = ressource;
            this.stackSize = stackSize;
        }
    }
}
