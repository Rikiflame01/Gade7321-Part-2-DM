using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityFunctionManager : MonoBehaviour
{
    public int UtilityFunction(string[,] board, string currentPlayerColor)
    {
        int w1 = 1, w2 = 1, w3 = 1, w4 = 1, w5 = 1;

        int pieceCount = PieceCount(board, currentPlayerColor);
        int diagonalControl = DiagonalControl(board, currentPlayerColor);
        int vulnerability = Vulnerability(board, currentPlayerColor);
        int strategicPositions = StrategicPositions(board, currentPlayerColor);
        int highValuePosition = HighValuePosition(board, currentPlayerColor);

        return w1 * pieceCount + w2 * diagonalControl - w3 * vulnerability + w4 * strategicPositions + w5 * highValuePosition;
    }

    private int PieceCount(string[,] board, string color)
    {
        int count = 0;
        foreach (var piece in board)
        {
            if (piece == color) count++;
        }
        return count;
    }

    private int DiagonalControl(string[,] board, string color)
    {
        // Custom logic to calculate diagonal control
        return 0; // Placeholder
    }

    private int Vulnerability(string[,] board, string color)
    {
        // Custom logic to calculate vulnerability
        return 0; // Placeholder
    }

    private int StrategicPositions(string[,] board, string color)
    {
        // Custom logic to calculate strategic positions
        return 0; // Placeholder
    }

    private int HighValuePosition(string[,] board, string color)
    {
        // Custom logic to calculate high value positions
        return 0; // Placeholder
    }
}
