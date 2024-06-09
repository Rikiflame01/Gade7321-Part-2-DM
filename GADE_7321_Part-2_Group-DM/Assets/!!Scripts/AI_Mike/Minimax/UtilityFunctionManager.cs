using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the utility functions for evaluating the game board.
/// </summary>
public class UtilityFunctionManager : MonoBehaviour
{
    #region Fields

    [Header("References")]
    [SerializeField, Tooltip("Reference to the piece capture handler.")]
    private PieceCaptureHandler captureHandler;

    [Header("Weights")]
    [SerializeField, Tooltip("Weight for piece count.")]
    private float weightPieceCount = 1f;

    [SerializeField, Tooltip("Weight for mobility.")]
    private float weightMobility = 1.5f;

    [SerializeField, Tooltip("Weight for corner control.")]
    private float weightCornerControl = 3f;

    [SerializeField, Tooltip("Weight for edge stability.")]
    private float weightEdgeStability = 1.2f;

    [SerializeField, Tooltip("Weight for positional advantage.")]
    private float weightPositional = 2f;

    [SerializeField, Tooltip("Weight for capturing potential.")]
    private float weightCapturingPotential = 2.5f;

    #endregion

    #region Public Methods

    /// <summary>
    /// Evaluates the board based on various factors and returns a utility value.
    /// </summary>
    /// <param name="board">The game board.</param>
    /// <param name="currentPlayerColour">The current player's color.</param>
    /// <returns>The utility value of the board for the current player.</returns>
    public float EvaluateBoard(string[,] board, string currentPlayerColour)
    {
        string opponentColour = currentPlayerColour == "Red" ? "Blue" : "Red";

        int pieceCount = PieceCount(board, currentPlayerColour) - PieceCount(board, opponentColour);
        int mobility = Mobility(board, currentPlayerColour) - Mobility(board, opponentColour);
        int cornerControl = CornerControl(board, currentPlayerColour) - CornerControl(board, opponentColour);
        int edgeStability = EdgeStability(board, currentPlayerColour) - EdgeStability(board, opponentColour);
        int positionalWeighting = PositionalWeighting(board, currentPlayerColour) - PositionalWeighting(board, opponentColour);
        int pieceCapturingPotential = PieceCapturingPotential(board, currentPlayerColour) - PieceCapturingPotential(board, opponentColour);

        float value = weightPieceCount * pieceCount + weightMobility * mobility + weightCornerControl * cornerControl +
                      weightEdgeStability * edgeStability + weightPositional * positionalWeighting +
                      weightCapturingPotential * pieceCapturingPotential;

        Debug.Log($"UF = {value}, Pieces: {pieceCount}, Mobility: {mobility}, Corners: {cornerControl}, Edges: {edgeStability}, Positional: {positionalWeighting}, Capturing Potential: {pieceCapturingPotential}");
        return value;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Counts the pieces of the specified color on the board.
    /// </summary>
    /// <param name="board">The game board.</param>
    /// <param name="colour">The color of the pieces to count.</param>
    /// <returns>The count of pieces of the specified color.</returns>
    private int PieceCount(string[,] board, string colour)
    {
        int count = 0;
        foreach (var piece in board)
        {
            if (piece == colour) count++;
        }
        return count;
    }

    /// <summary>
    /// Calculates the mobility of the specified player (number of possible moves).
    /// </summary>
    /// <param name="board">The game board.</param>
    /// <param name="colour">The player's color.</param>
    /// <returns>The number of possible moves for the player.</returns>
    private int Mobility(string[,] board, string colour)
    {
        return GetAllPossibleMoves(board).Count;
    }

    /// <summary>
    /// Counts the corners controlled by the specified player.
    /// </summary>
    /// <param name="board">The game board.</param>
    /// <param name="colour">The player's color.</param>
    /// <returns>The count of corners controlled by the player.</returns>
    private int CornerControl(string[,] board, string colour)
    {
        int count = 0;
        int size = board.GetLength(0);
        if (board[0, 0] == colour) count++;
        if (board[0, size - 1] == colour) count++;
        if (board[size - 1, 0] == colour) count++;
        if (board[size - 1, size - 1] == colour) count++;
        return count;
    }

    /// <summary>
    /// Counts the edges controlled by the specified player.
    /// </summary>
    /// <param name="board">The game board.</param>
    /// <param name="colour">The player's color.</param>
    /// <returns>The count of edges controlled by the player.</returns>
    private int EdgeStability(string[,] board, string colour)
    {
        int count = 0;
        int size = board.GetLength(0);

        for (int i = 0; i < size; i++)
        {
            if (board[0, i] == colour) count++;
            if (board[size - 1, i] == colour) count++;
            if (board[i, 0] == colour) count++;
            if (board[i, size - 1] == colour) count++;
        }

        return count;
    }

    /// <summary>
    /// Calculates the positional weighting based on the specified player's pieces.
    /// </summary>
    /// <param name="board">The game board.</param>
    /// <param name="colour">The player's color.</param>
    /// <returns>The positional weighting value.</returns>
    private int PositionalWeighting(string[,] board, string colour)
    {
        int[,] weights = {
            { 4, 3, 2, 3, 4 },
            { 3, 1, 1, 1, 3 },
            { 2, 1, 1, 1, 2 },
            { 3, 1, 1, 1, 3 },
            { 4, 3, 2, 3, 4 }
        };

        int value = 0;
        int size = board.GetLength(0);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (board[i, j] == colour)
                {
                    value += weights[i, j];
                }
            }
        }
        return value;
    }

    /// <summary>
    /// Calculates the capturing potential for the specified player.
    /// </summary>
    /// <param name="board">The game board.</param>
    /// <param name="colour">The player's color.</param>
    /// <returns>The capturing potential value.</returns>
    private int PieceCapturingPotential(string[,] board, string colour)
    {
        int potential = 0;
        List<Vector2> possibleMoves = GetAllPossibleMoves(board);

        foreach (var move in possibleMoves)
        {
            string[,] newBoard = ApplyMove(board, move, colour);
            potential += PieceCount(newBoard, colour);
        }

        return potential;
    }

    /// <summary>
    /// Applies a move to the board and returns the new board state.
    /// </summary>
    /// <param name="board">The current board state.</param>
    /// <param name="move">The move to apply.</param>
    /// <param name="currentPlayerColour">The current player's color.</param>
    /// <returns>The new board state after the move.</returns>
    private string[,] ApplyMove(string[,] board, Vector2 move, string currentPlayerColour)
    {
        int x = (int)move.x;
        int y = (int)move.y;

        string[,] newBoard = (string[,])board.Clone();
        newBoard[x, y] = currentPlayerColour;
        captureHandler.GetDiagonals(newBoard, x, y, currentPlayerColour, false); // Update the board with captured pieces
        return newBoard;
    }

    /// <summary>
    /// Generates all possible moves for the current board state.
    /// </summary>
    /// <param name="board">The current board state.</param>
    /// <returns>A list of possible moves.</returns>
    private List<Vector2> GetAllPossibleMoves(string[,] board)
    {
        List<Vector2> possibleMoves = new List<Vector2>();

        // High-weighting zones (corners)
        List<Vector2> highWeightingZones = new List<Vector2>
        {
            new Vector2(0, 0), new Vector2(0, 4),
            new Vector2(4, 0), new Vector2(4, 4)
        };

        foreach (var zone in highWeightingZones)
        {
            if (board[(int)zone.x, (int)zone.y] == "_")
            {
                possibleMoves.Add(zone);
            }
        }

        // Edge zones
        if (possibleMoves.Count == 0)
        {
            List<Vector2> edgeZones = new List<Vector2>
            {
                new Vector2(0, 1), new Vector2(0, 2), new Vector2(0, 3),
                new Vector2(1, 0), new Vector2(1, 4),
                new Vector2(2, 0), new Vector2(2, 4),
                new Vector2(3, 0), new Vector2(3, 4),
                new Vector2(4, 1), new Vector2(4, 2), new Vector2(4, 3)
            };

            foreach (var zone in edgeZones)
            {
                if (board[(int)zone.x, (int)zone.y] == "_")
                {
                    possibleMoves.Add(zone);
                }
            }
        }

        // Center zones
        if (possibleMoves.Count == 0)
        {
            for (int i = 1; i < board.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < board.GetLength(1) - 1; j++)
                {
                    if (board[i, j] == "_")
                    {
                        possibleMoves.Add(new Vector2(i, j));
                    }
                }
            }
        }

        return possibleMoves;
    }

    #endregion
}
