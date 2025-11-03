using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
public class OperacionesAleatorias : MonoBehaviour
{
    public TMP_Text timerText; //Para poner un timer visual
    public float tiempoLimite = 5f; // 5 segundos
    private float tiempoRestante;
    private bool jugando = false;
    public TMP_Text preguntaText; //Para que genere el texto visualmente
    public TMP_InputField respuestaUsuario;
    public string tableroNombre = "Tablero"; //Para llamar al tablero donde está jugando si responde bien
    private float respuestaCorrecta;
    public GameManager gameManager;

    void OnEnable()
    {
        tiempoRestante = tiempoLimite; // Reinicia el timer
        jugando = true; //Variable para saber si le queda tiempo o no
        GenerarPreguntas(); // Cada vez que el panel se active, genera una pregunta
        respuestaUsuario.text = "";
    }
    void Update()
    {
        if (!jugando) return;
        // Redondear para mostrar 1 decimal
        float tiempoMostrado = Mathf.Max(tiempoRestante, 0f);
        timerText.text = tiempoMostrado.ToString("F1"); // Muestra 1 decimal
        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0f)
        {
            jugando = false;
            SceneManager.LoadScene("Menu"); // reemplaza con tu escena de Game Over
        }
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
                while (a % b != 0 && b != 1)  // busca una división "limpia"
        b = UnityEngine.Random.Range(1, 10);

    float respuestaAproximada = (float)a / b;
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
            jugando = false;
            if (Mathf.Approximately(respuesta, respuestaCorrecta))
            {
                //  Vuelve al tablero
                SceneManager.UnloadSceneAsync("Minijuegos");

            }
            else
            {
                // Gay(Hector) Over
                SceneManager.LoadScene("Menu");
            }
        }
    }


}
