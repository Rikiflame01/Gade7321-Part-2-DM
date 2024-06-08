using System.Collections.Generic;
using UnityEngine;

public class UtilityFunctionManager : MonoBehaviour
{
    [SerializeField] private PieceCaptureHandler captureHandler;

    public float UtilityFunction(string[,] board, string currentPlayerColour)
    {
        const float w1 = 1, w2 = 2f, w3 = 1.5f, w4 = 0.7f, w5 = 2;

        int pieceCount = PieceCount(board, currentPlayerColour);
        int diagonalControl = DiagonalControl(board, currentPlayerColour);
        int vulnerability = Vulnerability(board, currentPlayerColour);
        int strategicPositions = StrategicPositions(board, currentPlayerColour);
        int highValuePosition = HighValuePosition(board, currentPlayerColour);

        float value = w1 * pieceCount + w2 * diagonalControl - (w3 * vulnerability) + w4 * strategicPositions + w5 * highValuePosition;
    
        Debug.Log($"UF = {value}, Pieces: {pieceCount}, Diagonal: {diagonalControl}, Vulnerability: {vulnerability}, Strategic: {strategicPositions}, HighValue: {highValuePosition}");
        return value;
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

        return 0;
    }

    private int Vulnerability(string[,] board, string colour)
    {
        int vulnerabilityScore = 0;

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == colour)
                {
                    if (IsMoveVulnerable(board, i, j, colour))
                    {
                        vulnerabilityScore += 100;
                    }
                }
            }
        }

        return vulnerabilityScore;
    }

    private bool IsMoveVulnerable(string[,] board, int x, int y, string colour)
    {
        string opponentColour = GetOppositeColour(colour);
        return HasTrap(board, x, y, opponentColour);
    }

    private int StrategicPositions(string[,] board, string colour)
    {
        int count = 0;

        for (int i = 0; i < 5; i++)
        {
            if (board[0, i] == colour) count++;
            if (board[i, 0] == colour) count++;
            if (board[4, i] == colour) count++;
            if (board[i, 4] == colour) count++;
        }

        return count * 1; // Example multiplier for edge pieces
    }

    private int HighValuePosition(string[,] board, string color)
    {
        int highValueScore = 0;

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == "_")
                {
                    string[,] tempBoard = (string[,])board.Clone();
                    tempBoard[i, j] = color;

                    List<Vector2> diagonalPositions = new List<Vector2>();
                    for (int k = 0; k < 4; k++)
                    {
                        int dx = captureHandler.Directions[k, 0];
                        int dy = captureHandler.Directions[k, 1];

                        for (int direction = -1; direction <= 1; direction += 2)
                        {
                            diagonalPositions.Clear();
                            for (int l = 0; l < board.GetLength(0); l++)
                            {
                                int newX = i + direction * dx * l;
                                int newY = j + direction * dy * l;

                                if (!captureHandler.IsInBounds(newX, newY, board.GetLength(0)))
                                    break;

                                diagonalPositions.Add(new Vector2(newX, newY));
                            }

                            var captureData = captureHandler.CapturePieces(diagonalPositions, tempBoard, color);

                            if (captureData.CapturePositions != null)
                            {
                                int captureCount = captureData.CapturePositions.Count;
                                if (captureCount == 3) highValueScore += 500;
                                else if (captureCount == 2) highValueScore += 200;
                                else if (captureCount == 1) highValueScore += 150;
                            }
                        }
                    }
                }
            }
        }

        return highValueScore;
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

                if (captureHandler.CheckTrapsOnly(diagonalPositions, board, colour, false))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private string GetOppositeColour(string colour)
    {
        return colour == "Blue" ? "Red" : "Blue";
    }
}
