using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab;   // Prefab de la casilla (Tile)
    [SerializeField] private Transform gameHolder;   // Contenedor visual de las casillas

    private Tile[,] grid;     // Matriz 2D de casillas (requisito del enunciado)
    private int width;        // Ancho del tablero
    private int height;       // Alto del tablero
    private int numMines;     // Número de minas (por ahora solo guardado)

    private readonly float tileSize = 1f;  // Tamaño visual de cada casilla

    void Start()
    {
        //  Nivel difícil según el enunciado: 16x30 con 99 minas
        CreateGameBoard(10, 10, 1);
    }

    public void CreateGameBoard(int width, int height, int numMines)
    {
        // Guardar los parámetros del tablero
        this.width = width;
        this.height = height;
        this.numMines = numMines;

        // Inicializar la matriz (estructura requerida por el enunciado)
        grid = new Tile[width, height];

        // Crear las casillas en la escena
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                // Instanciar el prefab de la casilla
                Transform tileTransform = Instantiate(tilePrefab);
                tileTransform.parent = gameHolder;

                // Calcular la posición centrada del tablero
                float xIndex = col - ((width - 1) / 2.0f);
                float yIndex = row - ((height - 1) / 2.0f);
                tileTransform.localPosition = new Vector2(xIndex * tileSize, yIndex * tileSize);

                // Obtener el componente Tile y guardarlo en la matriz
                Tile tile = tileTransform.GetComponent<Tile>();
                grid[col, row] = tile;

    
            }
        }
    }
}
