using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityFunctionManager : MonoBehaviour
{
    public float UtilityFunction(string[,] board, string currentPlayerColor)
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
        //regular top left, top right bottom, bottom right = 5
        
        return 0; // Placeholder
    }

    private int Vulnerability(string[,] board, string color)
    {
        //Use trapping for vulnerability of move
        //What is the best move for the opposite colour, so the AI won't move there
        return 0; // Placeholder
    }

    private int StrategicPositions(string[,] board, string color)
    {
        //regular top left, top right bottom, bottom right = 5
        //other edge pieces = 3
        return 0; // Placeholder
    }

    private int HighValuePosition(string[,] board, string color)
    {
        //If an AI player can capture 3 pieces = very high value position
        // " 2 = high value
        //" 1 = moderate value
        return 0; // Placeholder
    }
}
