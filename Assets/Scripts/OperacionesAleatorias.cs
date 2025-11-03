using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
public class OperacionesAleatorias : MonoBehaviour
{
    public TMP_Text preguntaText;
    public TMP_InputField respuestaUsuario;
    public string tableroNombre = "Tablero";
    private float respuestaCorrecta;
    void OnEnable()
    {
        GenerarPreguntas(); // Cada vez que el panel se active, genera una pregunta
        respuestaUsuario.text = "";
    }
    public void GenerarPreguntas()
    {


        int a = UnityEngine.Random.Range(1, 10);
        int b = UnityEngine.Random.Range(1, 10);
        int tipo = UnityEngine.Random.Range(0, 4);
        switch (tipo)
        {
            case 0:
                respuestaCorrecta = a + b;
                preguntaText.text = $"{a} + {b} = ?";
                break;
            case 1:
                respuestaCorrecta = a - b;
                preguntaText.text = $"{a} - {b} = ?";
                break;
            case 2:
                respuestaCorrecta = a * b;
                preguntaText.text = $"{a} × {b} = ?";
                break;
            case 3:
                if (b == 0) b = 1;
                float respuestaAproximada = a / b;
                respuestaCorrecta = Mathf.Round(respuestaAproximada * 10f) / 10f;
                preguntaText.text = $"{a} ÷ {b} = ?";

                break;
        }
    }
    public void OnAccept()
    {
        float respuesta;
        if (float.TryParse(respuestaUsuario.text, out respuesta))
        {
            if (Mathf.Approximately(respuesta, respuestaCorrecta))
            {
                //  Vuelve al tablero
                SceneManager.UnloadSceneAsync("Minijuegos");
            }
            else
            {
                // Gay(Hector) Over
                Debug.Log("Iasdqwwqd");
            }
        }
    }


}
