using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab;   // Prefab de la casilla (Tile)
    [SerializeField] private Transform gameHolder;   // Contenedor visual de las casillas

    private Tile[,] grid;     // Matriz2D de casillas (requisito del enunciado)
    private int width;        // Ancho del tablero
    private int height;       // Alto del tablero
    private int numMines;     // Número de minas (por ahora solo guardado)
    private int[,] minasMatriz; // Matriz de minas recibida del menú

    private readonly float tileSize = 1f;  // Tamaño visual de cada casilla
    public bool primerClick = true;

    [Header("UI Panels")]
    [SerializeField] private GameOverUI gameOverPanel; // Panel que reutiliza victoria y derrota

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
                tile.col = col;
                tile.row = row;
                tile.gameManager = this; // Asignar referencia al GameManager



            }
        }
        AjustarCamara();  // Ajusta automáticamente el zoom

        // cambiar las casillas por los numeros correspondientes
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                grid[col, row].mineCount = NumerosMinas(col, row);
            }
        }
        Debug.Log($" Tablero generado: {width}x{height} ({contarMinas()} minas)");
    }

    private int contarMinas()
    {
        int count = 0;
        foreach (int v in minasMatriz)
            if (v == 1) count++;
        return count;
    }

    private void AjustarCamara()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning(" No se encontro la camara'");
            return;
        }

        // Centrar la cámara en el tablero
        cam.transform.position = new Vector3(0, 0, -10);

        // Calcular el aspecto del tablero
        float aspect = (float)Screen.width / Screen.height;

        // El tamaño ortográfico base debe cubrir la mitad de la altura
        float halfHeight = height / 2f;
        float halfWidth = width / 2f / aspect;

        // Ajustar el zoom: elige el mayor valor entre ancho o alto
        cam.orthographicSize = Mathf.Max(halfHeight, halfWidth);

        // Añadir un pequeño margen
        cam.orthographicSize += 1f;
    }
    // asigna los numeros de minas a las casillas que no son minas 
    public int NumerosMinas(int col , int row)
    {
        int count = 0;

        // Revisar las 8 casillas vecinas
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                // Saltar la casilla central
                if (x == 0 && y == 0)
                    continue;

                int checkCol = col + x;
                int checkRow = row + y;

                // Validar que la posición esté dentro de los límites
                if (checkCol >= 0 && checkCol < width &&
                    checkRow >= 0 && checkRow < height)
                {
                    if (grid[checkCol, checkRow].isMine)
                        count++;
                }
            }
        }

        return count;
    }
    public void ClickVecinos(int col, int row)
    {
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                // Saltar la casilla central
                if (x == 0 && y == 0)
                    continue;

                int checkCol = col + x;
                int checkRow = row + y;

                // Validar límites
                if (checkCol >= 0 && checkCol < width &&
                    checkRow >= 0 && checkRow < height)
                {
                    Tile vecino = grid[checkCol, checkRow];
                    if (vecino.active && !vecino.isMine)
                    {
                        vecino.ClickedTile();

                        // Si no hay minas alrededor, propagar el clic recursivamente
                        if (vecino.mineCount == 0)
                            ClickVecinos(checkCol, checkRow);
                    }
                }
            }
        }
    }

    public void GameOver() // TOdo : cambiar a abrir un minijuego
    {
        Debug.Log("Game Over Has clickeado una mina.");
        // Revelar todas las minas
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                Tile tile = grid[col, row];
                tile.ShowGameOverState();
              
            }
        }
        // Cargar escena GameOver con estado de derrota
        GameOverState.GanoJuego = false;
        SceneManager.LoadScene("GameOver");
    }

    public void CheckGameOver()
    {
        int count = 0;
        int totalTiles = width * height;
        int nonMineTiles = totalTiles - numMines;
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                Tile tile = grid[col, row];
                if (tile.active)
                {
                    count++;
                }
            }
        }
        if (count == numMines)
        {
            Debug.Log("¡Felicidades! Has ganado el juego.");
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Tile tile = grid[col, row];
                    tile.active = false; // Desactivar todas las casillas
                    tile.SetFlaggedIfMine(); // Marcar las minas con bandera
                }
            }
            // Aquí puedes agregar lógica adicional para manejar la victoria
            GameOverState.GanoJuego = true;
            SceneManager.LoadScene("GameOver");
        }
    }

    // NOTA (principalmente para el profe) : Este metodo fue generado por chat.gpt, intente cambiar la logica varias veces para evitar stackOverflow en tableros grandes pero no fui capaz.
    public void AsegurarPrimerClickSeguro(int col, int row)
    {
        // ---------- helpers locales ----------
        bool Dentro(int c, int r) => c >= 0 && c < width && r >= 0 && r < height;

        // Cuenta minas actuales en la grilla (seguro y actualizado)
        int ContarMinasActuales()
        {
            int contador = 0;
            for (int r = 0; r < height; r++)
                for (int c = 0; c < width; c++)
                    if (grid[c, r].isMine) contador++;
            return contador;
        }

        // Intenta reubicar una mina desde (fromC,fromR) a alguna celda disponible.
        // preferredFilter: si true, prioriza celdas que no quedan adyacentes a (targetCol,targetRow)
        bool TryRelocateMine(int fromC, int fromR, int targetCol, int targetRow, bool preferNotAdjacent)
        {
            // crear lista de candidatos
            List<(int c, int r)> candidatos = new List<(int, int)>(width * height);
            for (int r = 0; r < height; r++)
            {
                for (int c = 0; c < width; c++)
                {
                    if (!grid[c, r].isMine && !(c == fromC && r == fromR) && !(c == targetCol && r == targetRow))
                        candidatos.Add((c, r));
                }
            }

            if (candidatos.Count == 0)
                return false;

            // Si preferimos no adyacentes, particionamos candidatos en "no adyacentes" y "otros"
            if (preferNotAdjacent)
            {
                List<(int c, int r)> noAdy = new List<(int, int)>();
                List<(int c, int r)> otros = new List<(int, int)>();
                foreach (var item in candidatos)
                {
                    int dc = Mathf.Abs(item.c - targetCol);
                    int dr = Mathf.Abs(item.r - targetRow);
                    if (dc > 1 || dr > 1) noAdy.Add(item);
                    else otros.Add(item);
                }

                List<(int c, int r)> pool = noAdy.Count > 0 ? noAdy : otherShuffle(otros);
                if (pool.Count == 0 && otros.Count == 0)
                    return false;

                // elegir destino aleatorio del pool
                var dest = pool[UnityEngine.Random.Range(0, pool.Count)];
                // mover la mina
                grid[fromC, fromR].isMine = false;
                minasMatriz[fromR, fromC] = 0;

                grid[dest.c, dest.r].isMine = true;
                minasMatriz[dest.r, dest.c] = 1;

                return true;
            }
            else
            {
                // elegir candidato aleatorio
                var dest = candidatos[UnityEngine.Random.Range(0, candidatos.Count)];
                grid[fromC, fromR].isMine = false;
                minasMatriz[fromR, fromC] = 0;

                grid[dest.c, dest.r].isMine = true;
                minasMatriz[dest.r, dest.c] = 1;
                return true;
            }
        }

        // auxiliar para devolver lista aleatoria (no modifica original)
        List<(int c, int r)> otherShuffle(List<(int c, int r)> list)
        {
            var copy = new List<(int, int)>(list);
            for (int i = 0; i < copy.Count; i++)
            {
                int j = UnityEngine.Random.Range(i, copy.Count);
                var tmp = copy[i];
                copy[i] = copy[j];
                copy[j] = tmp;
            }
            return copy;
        }
        // ---------- end helpers ----------

        int totalCells = width * height;
        int minasActuales = ContarMinasActuales();

        // Si el tablero está tan lleno que no hay dónde mover minas, salir seguro
        if (minasActuales >= totalCells - 1)
        {
            Debug.LogWarning("⚠️ Tablero demasiado saturado: no se puede garantizar un primer clic totalmente libre.");
            // Aun así, aseguramos que la celda clickeada no sea una mina si es posible:
            if (grid[col, row].isMine)
            {
                // intentar mover esa mina a cualquier lugar disponible (sin preferencia)
                bool moved = TryRelocateMine(col, row, col, row, false);
                if (!moved)
                {
                    // no se pudo mover -> la casilla sigue siendo mina; no intentamos más
                    Debug.LogWarning("No se pudo reubicar la mina del primer clic en tablero saturado.");
                }
                else
                {
                    RecalcularNumeros();
                }
            }
            return;
        }

        // Si la casilla clickeada es mina, quitamos la mina y la reubicamos preferiblemente
        if (grid[col, row].isMine)
        {
            // quitamos temporalmente la mina de la casilla del primer clic
            grid[col, row].isMine = false;
            minasMatriz[row, col] = 0;

            // Intentamos reubicar la mina preferentemente en una celda que NO quede adyacente al primer clic
            bool relocated = TryRelocateMine(col, row, col, row, true);

            if (!relocated)
            {
                // Si falló la reubicación preferente, intentamos una reubicación sin preferencia
                relocated = TryRelocateMine(col, row, col, row, false);
                if (!relocated)
                {
                    // No se pudo reubicar (muy improbable porque chequeamos saturación), por seguridad
                    grid[col, row].isMine = true;
                    minasMatriz[row, col] = 1;
                    Debug.LogWarning("⚠️ No se pudo reubicar mina del primer clic (inconsistencia inesperada).");
                    return;
                }
            }
        }

        // Ahora intentamos asegurar área 3x3 libre (si es factible), moviendo minas adyacentes si es posible.
        // No recursivo, máximo intentos limitados.
        const int MAX_RELOCATION_ATTEMPTS = 1000;
        int attempts = 0;
        bool changed = true;

        // Construimos una lista de vecinos adyacentes (incluye diagonales)
        List<(int c, int r)> vecinos = new List<(int, int)>();
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                int nc = col + x;
                int nr = row + y;
                if (Dentro(nc, nr) && !(nc == col && nr == row))
                    vecinos.Add((nc, nr));
            }
        }

        // Repetimos hasta que no haya minas en la zona o se agoten intentos
        while (changed && attempts < MAX_RELOCATION_ATTEMPTS)
        {
            attempts++;
            changed = false;

            // Recalcular números localmente antes de decisiones
            RecalcularNumeros();

            // Recolectar minas dentro de la zona 3x3 (excepto la celda central que ya no debe ser mina)
            List<(int c, int r)> minasEnZona = new List<(int, int)>();
            foreach (var v in vecinos)
                if (grid[v.c, v.r].isMine)
                    minasEnZona.Add(v);

            if (minasEnZona.Count == 0)
                break; // zona limpia

            // Intentar mover cada mina encontrada a una celda disponible no adyacente al centro
            foreach (var m in minasEnZona)
            {
                if (attempts >= MAX_RELOCATION_ATTEMPTS) break;
                bool moved = TryRelocateMine(m.c, m.r, col, row, true);
                if (moved)
                {
                    changed = true;
                }
                attempts++;
            }
        }

        // Recalcular números una última vez tras todas las operaciones
        RecalcularNumeros();

        // Nota: si no fue posible limpiar completamente la zona (por saturación),
        // dejaremos la casilla central libre de mina (si se pudo) y no intentamos más reubicaciones.
        // Esto evita ciclos infinitos y stack overflow.
    }
    private void RecalcularNumeros()
    {
        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < width; c++)
            {
                grid[c, r].mineCount = NumerosMinas(c, r);
            }
        }
    }
    public void ReiniciarTablero()
    {
        Debug.Log(" Reiniciando tablero...");

        // 1️ Eliminar las casillas existentes del contenedor visual
        foreach (Transform child in gameHolder)
        {
            Destroy(child.gameObject);
        }

        // 2️ Resetear variables
        primerClick = true;

        // Si la matriz de minas vino del menú, reutilízala o genera una nueva
        if (minasMatriz == null)
        {
            minasMatriz = GeneradorMinas.GenerarMinas(10, 10, 10);
        }

        // 3️ Recrear la grilla y sus números
        CreateGameBoard();

        Debug.Log("Tablero reiniciado correctamente.");
    }
}