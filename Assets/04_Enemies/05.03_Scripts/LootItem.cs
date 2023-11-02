using RuntimeNodeEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItem : MonoBehaviour
{
    [SerializeField]
    private Ressource resourceObject;
    [SerializeField]
    private RessourceInventar inventory;

    private Ressource thisResource { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        thisResource = Instantiate(resourceObject);
        thisResource.ownedAmount = Random.Range(5, 20);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inventory.getListOfRessources()[inventory.getRessourceIndex(thisResource._name)].incrementCount(thisResource.ownedAmount);
            Destroy(gameObject);
        }
    }
}