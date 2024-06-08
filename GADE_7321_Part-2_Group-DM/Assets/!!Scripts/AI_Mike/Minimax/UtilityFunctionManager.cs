using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityFunctionManager : MonoBehaviour
{

    [SerializeField] private PieceCaptureHandler captureHandler;
    
    public float UtilityFunction(string[,] board, string currentPlayerColour) //Add a default value
    {
        const float w1 = 1, w2 = 0.6f, w3 = 1, w4 = 0.7f, w5 = 1;

        int pieceCount = PieceCount(board, currentPlayerColour);
        int diagonalControl = DiagonalControl(board, currentPlayerColour);
        int vulnerability = Vulnerability(board, currentPlayerColour);
        int strategicPositions = StrategicPositions(board, currentPlayerColour);
        int highValuePosition = HighValuePosition(board, currentPlayerColour);

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

    private int DiagonalControl(string[,] board, string color) //Do I not need the position getting evaluated?
    {
        //regular top left, top right bottom, bottom right = 5

        bool isHighDiagonalControl =
            board[0, 0] == "_" || board[0, 4] == "_" || board[4, 0] == "_" || board[4, 4] == "_";

        if (isHighDiagonalControl) return 100;
        
        return 0; // Placeholder
    }

    private int Vulnerability(string[,] board, string color)
    {
        // bool vulnerable = HasTrap();
        // if (vulnerable) return 100;
        //Use trapping for vulnerability of move
        //What is the best move for the opposite colour, so the AI won't move there
        return 0; // Placeholder
    }

    private int StrategicPositions(string[,] board, string color)
    {
        //regular top left, top right bottom, bottom right = 5
        List<Vector2> edgePieces = new List<Vector2>();

        for (int i = 0; i < 5; i++)
        {
            edgePieces.Add(new Vector2(0,i)); //top row
            edgePieces.Add(new Vector2(i, 0)); //left column
            edgePieces.Add(new Vector2(4, i)); //bottom row
            edgePieces.Add(new Vector2(i, 4)); //right column
        }
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
    
    private bool HasTrap(string[,] board, int x, int y, string colour)
    {
        for (int i = 0; i < 4; i++)
        {
            int dx = captureHandler.Directions[i, 0];
            int dy = captureHandler.Directions[i, 1];

            for (int direction = -1; direction <= 1; direction += 2)
            {
                List<Vector2> diagonalPositions = new List<Vector2>();
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    int newX = x + direction * dx * j;
                    int newY = y + direction * dy * j;

                    if (!captureHandler.IsInBounds(newX, newY, board.GetLength(0)))
                        break;

                    diagonalPositions.Add(new Vector2(newX, newY));
                }

                if (captureHandler.CheckTrapsOnly(diagonalPositions, board, colour))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
