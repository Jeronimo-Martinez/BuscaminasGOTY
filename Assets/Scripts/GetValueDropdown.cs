using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GetValueDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    const int principiante = 0; // indice del modo personalizado
    const int intermedio = 1; // indice del modo personalizado
    const int experto = 2; // indice del modo personalizado
    const int personalizado = 3; // indice del modo personalizado

    // atributos publicos a otros scripts 
    public static int[,] matrizGenerada;
    public int modoSeleccionado;
    public void GetDropdownValue()

    {
        int indice = dropdown.value;
        string opcion = dropdown.options[indice].text;
        modoSeleccionado = indice;
        Debug.Log(opcion+":"+indice); // se muestra en la consola para debug 
        
        int filas = 0, columnas = 0, minas = 0;

        // se genera la matriz correspondiente 
        switch (indice)
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
            case personalizado:
                filas = 10; columnas = 10; minas = 20; // puedes pedir valores personalizados
                break;
            default:
                Debug.LogWarning("Modo no reconocido");  // TOdo: manejar error
                break;
        }
        matrizGenerada = GeneradorMinas.GenerarMinas(filas, columnas, minas);

        Debug.Log(matrizGenerada);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
