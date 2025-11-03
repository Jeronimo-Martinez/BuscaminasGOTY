using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReaccionMinijuego : MonoBehaviour
{
    public TMP_Text TextoGuia;
    public GameObject objetivo; // Imagen que aparecerá y tendrá que reaccionar
    public float tiempoMin = 1f; // Tiempo mínimo para que aparezca
    public float tiempoMax = 5f; // Tiempo máximo para que aparezca
    private bool esperandoClick = false;
    
    void OnEnable()
    {
        TextoGuia.gameObject.SetActive(true);
        objetivo.SetActive(false);
        StartCoroutine(EsperarMomentoAleatorio());
    }

    IEnumerator EsperarMomentoAleatorio()
    {
        float tiempo = Random.Range(tiempoMin, tiempoMax);
        yield return new WaitForSeconds(tiempo);

        // Mostrar la imagen y desactivar el texto guia
        TextoGuia.gameObject.SetActive(false);
        objetivo.SetActive(true);
        esperandoClick = true;

        // Si no hace click en 1 segundo chao pescao
        yield return new WaitForSeconds(1f);

        if (esperandoClick)
        {
            // Si no reacciona
            NotificarDerrota(); // Game Over
        }
    }

    public void OnClickObjetivo()
    {
        if (esperandoClick)
        {
            esperandoClick = false;
            objetivo.SetActive(false);
            SceneManager.UnloadSceneAsync("Minijuegos"); //Si lo hizo bien sigue jugando 
        }
    }

    private void NotificarDerrota()
    {
        var gm = Object.FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.GameOver();
        }
        else
        {
            SceneManager.LoadScene("GameOver");
        }
        var scene = SceneManager.GetSceneByName("Minijuegos");
        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync("Minijuegos");
        }
    }
}
