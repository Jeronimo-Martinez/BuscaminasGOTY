using UnityEngine;

public class MinijuegoLoader : MonoBehaviour
{
    public GameObject[] minijuegos;
    
       void Start()
    {
        if (minijuegos.Length == 0)
        {
            Debug.LogWarning("No hay minijuegos asignados en el inspector.");
            return;
        }

        // Desactivar todos los minijuegos
        foreach (var m in minijuegos)
            m.SetActive(false);

        // Elegir uno al azar
        int index = Random.Range(0, minijuegos.Length);
        minijuegos[index].SetActive(true);
    }
    
}
