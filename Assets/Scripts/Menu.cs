using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    const int principiante = 0; // indice del modo personalizado
    const int intermedio = 1; // indice del modo personalizado
    const int experto = 2; // indice del modo personalizado
    const int personalizado = 3; // indice del modo personalizado
    [SerializeField] private GameObject panelPersonalizado;
    [SerializeField] private GameObject errorMinas;
    [SerializeField] private GameObject errorTexto;

    // Inputs dentro del panelPersonalizado
    [SerializeField] private TMP_InputField inputFilas;
    [SerializeField] private TMP_InputField inputColumnas;
    [SerializeField] private TMP_InputField inputMinas;

    public GetValueDropdown selector;
    public int modoSelecionado;
    public static int[,] matrizGenerada;
    public void GoToScene(string sceneName)
    { 
        selector.GetDropdownValue();
        modoSelecionado = selector.modoSeleccionado;

        int filas = 0, columnas = 0, minas = 0;

        // se genera la matriz correspondiente 
        if (modoSelecionado != personalizado)
        {
            switch (modoSelecionado)
            {
                case principiante:
                    filas = 8; columnas = 8; minas = 10;
                    break;
                case intermedio:
                    filas = 16; columnas = 16; minas = 40;
                    break;
                case experto:
                    filas = 16; columnas = 30; minas = 99;
                    break;
                default:
                    Debug.LogWarning("Modo no reconocido");  // TOdo: manejar error
                    break;

            }
            matrizGenerada = GeneradorMinas.GenerarMinas(filas, columnas, minas);

            Debug.Log(matrizGenerada);

            SceneManager.LoadScene(sceneName);
        }
        else
        {
            panelPersonalizado.SetActive(true);
            
        }
            
    }
    public void ConfirmarPersonalizado(string sceneName)
    {
        errorMinas.SetActive(false);
        errorTexto.SetActive(false);

        int filas = 0, columnas = 0, minas = 0;

        // Validar formato numérico 
        if (!int.TryParse(inputFilas.text, out filas) ||
            !int.TryParse(inputColumnas.text, out columnas) ||
            !int.TryParse(inputMinas.text, out minas))
        {
            errorTexto.SetActive(true);
            Debug.LogError("Error: Los campos deben contener solo números enteros válidos.");
            return;
        }

        // se validan los valores 
        if (filas <= 0 || columnas <= 0 || minas <= 0)
        {
            errorTexto.SetActive(true);
            Debug.LogError("Error: valores no positivos.");
            return;
        }

        if (minas > filas * columnas)
        {
            errorMinas.SetActive(true);
            Debug.LogError(" Error: El número de minas excede el tamaño de la matriz.");
            return;
        }

        try
        {
            filas = int.Parse(inputFilas.text); 
            columnas = int.Parse(inputColumnas.text); 
            minas = int.Parse(inputMinas.text);
            matrizGenerada = GeneradorMinas.GenerarMinas(filas, columnas, minas);
            Debug.Log($"Modo Personalizado: {filas}x{columnas} con {minas} minas");

            panelPersonalizado.SetActive(false);
            SceneManager.LoadScene(sceneName);
        }
        catch (System.Exception e)
        {
            errorTexto.SetActive(true);
            Debug.LogError(" Error inesperado al generar la matriz: " + e.Message);
        }
    }
    

    public void CancelarPersonalizado()
    {
        panelPersonalizado.SetActive(false);
        errorMinas.SetActive(false);
        errorTexto.SetActive(false);
    }
public void Quit()
    {
        Application.Quit();
        Debug.Log("La app se ha cerrado");
    }
}
