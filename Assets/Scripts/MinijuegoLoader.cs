using UnityEngine;

public class MinijuegoLoader : MonoBehaviour
{
   public GameObject miniGamePanel;

    void Start()
    {
        if (miniGamePanel != null)
        {
            miniGamePanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("MiniGamePanel no asignado en el inspector.");
        }
    }
}
