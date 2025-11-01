using System;
using System.Collections.Generic;

public class GeneradorMinas
{
	public static int[,] GenerarMinas(int filas, int columnas, int minas)
	{
		if (minas > filas * columnas)
			throw new ArgumentException("El n�mero de minas excede el tama�o de la matriz.");

		int[,] matriz = new int[filas, columnas];
		Random rand = new Random();

		HashSet<int> posiciones = new HashSet<int>(); // Evita que 2 minas est�n en la misma posici�n

		// Generar posiciones paras las minas 
		while (posiciones.Count < minas)
		{
			int pos = rand.Next(0, filas * columnas);
			posiciones.Add(pos);
		}

		// se a�aden las minas , represenradas por 1 a la matriz de 0(por defecto) 
		foreach (int pos in posiciones)
		{
			int fila = pos / columnas;
			int columna = pos % columnas;
			matriz[fila, columna] = 1;
		}

		return matriz;
	}
}