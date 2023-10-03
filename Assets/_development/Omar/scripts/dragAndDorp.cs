using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class dragAndDorp : MonoBehaviour
{
    public VisualElement obj;
    private VisualElement root;

    // Start is called before the first frame update
    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        DragAndDropManipulator manipulator = new(root.Q<VisualElement>("object"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
