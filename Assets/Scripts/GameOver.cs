using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI labelResultado;      // Texto de "GANASTE" o "PERDISTE"
    [SerializeField] private Button botonMenuPrincipal;
    [SerializeField] private Button botonCerrar;
    [SerializeField] private Image background;
    private bool ganoJuego;

    private void Awake()
    {
        MostrarResultado(ganoJuego);
        // Asigna los eventos de los botones
        if (botonMenuPrincipal != null)
            botonMenuPrincipal.onClick.AddListener(IrMenuPrincipal);

        if (botonCerrar != null)
            botonCerrar.onClick.AddListener(CerrarJuego);
        
    }

   
    public void MostrarResultado(bool ganoJuego)
    {
        ganoJuego = false;
        if (labelResultado != null)
        {
            if (ganoJuego)
            {
                labelResultado.text = "!GANASTE!";
                background.color = new Color(0.20f, 0.77f, 0.45f, 1.0f);
            }
            else
            {
                labelResultado.text = "PERDISTE!";
                background.color = new Color(0.86f, 0.19f, 0.20f, 1.0f);
            }
        }
    }

    private void IrMenuPrincipal()
    {
        SceneManager.LoadScene("Menu");
    }

    private void CerrarJuego()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}