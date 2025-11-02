using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject
{
    private event UnityAction listeners;
    public void Raise()
    {
        listeners?.Invoke();
    }  
     public void RegisterListener(UnityAction listener)
     {
        listeners += listener;
     }
    public void UnregisterListener(UnityAction listener)
    {
        listeners -= listener;
    }

}
