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
        // Tomar el estado global al entrar a la escena de Game Over
        ganoJuego = GameOverState.GanoJuego;
        MostrarResultado(ganoJuego);
        // Asigna los eventos de los botones
        if (botonMenuPrincipal != null)
            botonMenuPrincipal.onClick.AddListener(IrMenuPrincipal);

        if (botonCerrar != null)
            botonCerrar.onClick.AddListener(CerrarJuego);
        
    }

   
    public void MostrarResultado(bool ganoJuego)
    {
        this.ganoJuego = ganoJuego;
        if (labelResultado != null)
        {
            if (ganoJuego)
            {
                labelResultado.text = "!GANASTE!";
                if (background != null)
                    background.color = new Color(0.20f,0.77f,0.45f,1.0f);
            }
            else
            {
                labelResultado.text = "PERDISTE!";
                if (background != null)
                    background.color = new Color(0.86f,0.19f,0.20f,1.0f);
            }
        }
    }

    public void Show(bool gano)
    {
        gameObject.SetActive(true);
        MostrarResultado(gano);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
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