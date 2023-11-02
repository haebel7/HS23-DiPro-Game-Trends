using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryAnnouncer : MonoBehaviour
{
    private FadeEffect fade;

    public void Start()
    {
        Debug.Log("entred victory scene");
        fade = GameObject.Find("FadeInOut").GetComponent<FadeEffect>();
        StartVictory();
    }

    private void StartVictory()
    {
        fade.StartEffect();
    }
}
