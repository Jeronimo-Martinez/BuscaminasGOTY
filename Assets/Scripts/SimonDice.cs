using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimonDice : MonoBehaviour
{
    [Header("Panel")]
    public GameObject PanelBotones;

    [Header("UI")]
    public TMP_Text TextoGuia;
    public TMP_Text TextoTuTurno;
    public Button[] botones;

    [Header("Timer")]
    public float tiempoTotalTurno = 5f;
    private float tiempoRestante;
    private bool timerActivo = false;

    private List<int> secuenciaSimon = new List<int>();
    private List<int> secuenciaJugador = new List<int>();
    private bool esperandoJugador = false;
    private int longitudSecuencia = 6;

    void Start()
    {
        // Generar secuencia fija de 6 pasos
        secuenciaSimon.Clear();
        for (int i = 0; i < longitudSecuencia; i++)
        {
            int paso = Random.Range(0, botones.Length);
            secuenciaSimon.Add(paso);
        }

        EmpezarRonda();
    }

    void Update()
    {
        if (timerActivo && esperandoJugador)
        {
            tiempoRestante -= Time.deltaTime;
            TextoTuTurno.text = $"Tu turno! {tiempoRestante:F1}s";

            if (tiempoRestante <= 0f)
            {
                timerActivo = false;
                esperandoJugador = false;
                TextoTuTurno.text = "Tiempo agotado!";
                SceneManager.LoadScene("Menu"); // Game Over
            }
        }
    }

    public void EmpezarRonda()
    {
        if (PanelBotones != null)
            PanelBotones.SetActive(true);

        TextoGuia.gameObject.SetActive(true);
        esperandoJugador = false;
        secuenciaJugador.Clear();

        // Mostrar la secuencia
        StartCoroutine(MostrarSecuencia());
    }

    IEnumerator MostrarSecuencia()
    {
        foreach (int i in secuenciaSimon)
        {
            botones[i].image.color = Color.white;
            yield return new WaitForSeconds(0.5f);
            RestaurarColorBoton(i);
            yield return new WaitForSeconds(0.3f);
        }

        TextoGuia.gameObject.SetActive(false);
        TextoTuTurno.gameObject.SetActive(true);
        esperandoJugador = true;

        tiempoRestante = tiempoTotalTurno;
        timerActivo = true;
    }

    void RestaurarColorBoton(int i)
    {
        switch (i)
        {
            case 0: botones[i].image.color = Color.red; break;
            case 1: botones[i].image.color = Color.blue; break;
            case 2: botones[i].image.color = Color.green; break;
            case 3: botones[i].image.color = Color.yellow; break;
        }
    }

    public void PresionarBoton(int index)
    {
        if (!esperandoJugador) return;

        secuenciaJugador.Add(index);

        // Validación paso por paso
        for (int i = 0; i < secuenciaJugador.Count; i++)
        {
            if (secuenciaJugador[i] != secuenciaSimon[i])
            {
                esperandoJugador = false;
                timerActivo = false;
                SceneManager.LoadScene("Menu"); // Game Over
                return;
            }
        }

        // Si completó los 6 pasos correctamente
        if (secuenciaJugador.Count == secuenciaSimon.Count)
        {
            esperandoJugador = false;
            timerActivo = false;
            SceneManager.LoadScene("Tablero"); // Vuelve al tablero
        }
    }
}
