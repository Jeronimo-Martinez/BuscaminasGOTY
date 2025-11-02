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
    private int[,] minasMatriz; // Matriz de minas recibida del menú

    private readonly float tileSize = 1f;  // Tamaño visual de cada casilla

    void Start()
    {
        minasMatriz = Menu.matrizGenerada;
        numMines = contarMinas();

        if (minasMatriz == null)
        {
            Debug.LogWarning("⚠️ No se recibió matriz desde el menú. Se generará una por defecto.");
            minasMatriz = GeneradorMinas.GenerarMinas(10, 10, 10); // fallback
        }

        // Obtener dimensiones reales
        height = minasMatriz.GetLength(0);
        width = minasMatriz.GetLength(1);

        CreateGameBoard();
        //  Nivel difícil según el enunciado: 16x30 con 99 minas
        
    }

    public void CreateGameBoard()
    {

        // Inicializar la matriz (estructura requerida por el enunciado)
        grid = new Tile[width, height];

        // Crear las casillas en la escena
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                // Instanciar el prefab de la casilla
                Transform tileTransform = Instantiate(tilePrefab, gameHolder);
                //tileTransform.parent = gameHolder;

                // Calcular la posición centrada del tablero
                float xIndex = col - ((width - 1) / 2.0f);
                float yIndex = row - ((height - 1) / 2.0f);
                tileTransform.localPosition = new Vector2(xIndex * tileSize, yIndex * tileSize);

                // Obtener el componente Tile y guardarlo en la matriz
                Tile tile = tileTransform.GetComponent<Tile>();
                grid[col, row] = tile;

                // Asignar si la casilla es mina o no según la matriz recibida
                tile.isMine = (minasMatriz[row, col] == 1); 


            }
         

}
    }
    private int contarMinas()
    {
        int count = 0;
        foreach (int v in minasMatriz)
            if (v == 1) count++;
        return count;
    }
}
