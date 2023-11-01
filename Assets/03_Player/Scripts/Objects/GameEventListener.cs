using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent myEvent;
    public UnityEvent response;

    private void OnEnable()
    {
        myEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        myEvent.UnregisterListener(this);
    }
    public void OnEventRaised()
    {
        response.Invoke();
    }
}