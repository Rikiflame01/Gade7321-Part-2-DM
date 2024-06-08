using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityFunctionManager : MonoBehaviour
{
    [SerializeField] private PieceCaptureHandler captureHandler;

    public float UtilityFunction(string[,] board, string currentPlayerColour, int x, int y)
    {
        const float w1 = 1, w2 = 0.6f, w3 = 1, w4 = 0.7f, w5 = 1;

        int pieceCount = PieceCount(board, currentPlayerColour);
        int diagonalControl = DiagonalControl(board, currentPlayerColour);
        int vulnerability = Vulnerability(board, currentPlayerColour, x, y);
        int strategicPositions = StrategicPositions(board, currentPlayerColour);
        int highValuePosition = HighValuePosition(board, currentPlayerColour);

        return w1 * pieceCount + w2 * diagonalControl - w3 * vulnerability + w4 * strategicPositions + w5 * highValuePosition;
    }

    private int PieceCount(string[,] board, string colour)
    {
        int count = 0;
        foreach (var piece in board)
        {
            if (piece == colour) count++;
        }
        return count;
    }

    private int DiagonalControl(string[,] board, string colour)
    {
        bool isHighDiagonalControl =
            board[0, 0] == colour || board[0, 4] == colour || board[4, 0] == colour || board[4, 4] == colour;

        if (isHighDiagonalControl) return 100;

        return 0; // Placeholder
    }

    private int Vulnerability(string[,] board, string colour, int x, int y)
    {
        bool vulnerable = HasTrap(board, x, y, colour);
        if (vulnerable) return 100;
        return 0;
    }

    private int StrategicPositions(string[,] board, string colour)
    {
        int count = 0;
        List<Vector2> edgePieces = new List<Vector2>();

        for (int i = 0; i < 5; i++)
        {
            if (board[0, i] == colour) count++; // top row
            if (board[i, 0] == colour) count++; // left column
            if (board[4, i] == colour) count++; // bottom row
            if (board[i, 4] == colour) count++; // right column
        }

        return count;
    }

    private int HighValuePosition(string[,] board, string color)
    {
        int count = 0;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == "_")
                {
                    List<Vector2> diagonalPositions = new List<Vector2>();
                    for (int k = 0; k < 4; k++)
                    {
                        int dx = captureHandler.Directions[k, 0];
                        int dy = captureHandler.Directions[k, 1];

                        for (int direction = -1; direction <= 1; direction += 2)
                        {
                            for (int l = 0; l < board.GetLength(0); l++)
                            {
                                int newX = i + direction * dx * l;
                                int newY = j + direction * dy * l;

                                if (!captureHandler.IsInBounds(newX, newY, board.GetLength(0)))
                                    break;

                                diagonalPositions.Add(new Vector2(newX, newY));
                            }
                        }
                    }

                    var captureData = captureHandler.CapturePieces(diagonalPositions, board, color);

                    if (captureData.CapturePositions == null)
                    {
                        Debug.LogError($"Capture positions null");
                        return 0;
                    }
                    
                    int captureCount = captureData.CapturePositions.Count;
                    if (captureCount == 3) count += 100;
                    else if (captureCount == 2) count += 90;
                    else if (captureCount == 1) count += 80;
                }
            }
        }
        Debug.LogError($"Count is: {count}");
        return count;
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
