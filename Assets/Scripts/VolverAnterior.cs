using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorEscenas : MonoBehaviour
{
    public static string escenaAnterior;

    private void Awake()
    {
        // Guardar la escena actual cada vez que se carga
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            escenaAnterior = SceneManager.GetActiveScene().name;
        };
    }

    public void CargarEscena(string nombreEscena)
    {
        escenaAnterior = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nombreEscena);
    }

    public void VolverEscenaAnterior()
    {
        if (!string.IsNullOrEmpty(escenaAnterior))
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            Debug.LogWarning("No hay escena anterior registrada.");
        }
    }
}
