using UnityEngine;
using TMPro;
public class OperacionesAleatorias : MonoBehaviour
{
    public TMP_Text preguntaText;
    private int respuestaCorrecta;
    void OnEnable()
    {
        GenerarPreguntas(); // Cada vez que el panel se active, genera una pregunta
    }
    public void GenerarPreguntas()
    {


        int a = Random.Range(1, 10);
        int b = Random.Range(1, 10);
        int tipo = Random.Range(0, 4);
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
                respuestaCorrecta = a / b;
                preguntaText.text = $"{a} ÷ {b} = ?";
                break;
        }
    }
     public bool VerificarRespuesta(string input)
    {
        int respuesta;
        if(int.TryParse(input, out respuesta))
        {
            return respuesta == respuestaCorrecta;
        }
        return false;
    }


}
