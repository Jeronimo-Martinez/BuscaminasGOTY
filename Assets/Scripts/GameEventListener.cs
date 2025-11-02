using UnityEngine;
using UnityEngine.Events;
public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;
    public UnityEvent response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(OnEventRaised);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(OnEventRaised);

    }
    
    private void OnEventRaised()
    {
        response.Invoke();
    }

}
