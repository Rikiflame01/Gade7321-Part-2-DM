using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements the Minimax algorithm with alpha-beta pruning for the game.
/// </summary>
public class Minimax : MonoBehaviour
{
    #region Fields

    [Header("References")]
    [SerializeField, Tooltip("Reference to the utility function manager.")]
    private UtilityFunctionManager utilityFunction;

    [SerializeField, Tooltip("Reference to the piece capture handler.")]
    private PieceCaptureHandler captureHandler;

    #endregion

    #region Public Methods

    /// <summary>
    /// Minimax algorithm with alpha-beta pruning.
    /// </summary>
    /// <param name="board">The game board.</param>
    /// <param name="depth">The search depth.</param>
    /// <param name="isMaximizingPlayer">Whether the current player is maximizing or minimizing.</param>
    /// <param name="currentPlayerColour">The current player's colour.</param>
    /// <param name="alpha">Alpha value for pruning.</param>
    /// <param name="beta">Beta value for pruning.</param>
    /// <returns>The best evaluation score and move.</returns>
    public (float, Vector2) MinimaxFunction(string[,] board, int depth, bool isMaximizingPlayer, string currentPlayerColour, float alpha, float beta)
    {
        string opponentColor = currentPlayerColour == "Red" ? "Blue" : "Red";

        if (depth == 0 || IsGameOver(board))
        {
            return (utilityFunction.EvaluateBoard(board, currentPlayerColour), Vector2.negativeInfinity);
        }

        List<Vector2> possibleMoves = GetAllPossibleMoves(board);
        float bestEval = isMaximizingPlayer ? float.MinValue : float.MaxValue;
        Vector2 bestMove = Vector2.negativeInfinity;

        foreach (var move in possibleMoves)
        {
            string[,] newBoard = ApplyMove(board, move, currentPlayerColour);
            float eval = MinimaxFunction(newBoard, depth - 1, !isMaximizingPlayer, currentPlayerColour, alpha, beta).Item1;

            if (isMaximizingPlayer)
            {
                if (eval > bestEval)
                {
                    bestEval = eval;
                    bestMove = move;
                }
                alpha = Mathf.Max(alpha, eval);
            }
            else
            {
                if (eval < bestEval)
                {
                    bestEval = eval;
                    bestMove = move;
                }
                beta = Mathf.Min(beta, eval);
            }

            if (beta <= alpha)
            {
                break; // Alpha-beta pruning
            }
        }

        return (bestEval, bestMove);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Generates all possible moves for the current board state.
    /// </summary>
    /// <param name="board">The game board.</param>
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

    /// <summary>
    /// Applies a move to the board.
    /// </summary>
    /// <param name="board">The game board.</param>
    /// <param name="move">The move to apply.</param>
    /// <param name="currentPlayerColour">The current player's colour.</param>
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
    /// Checks if the game is over.
    /// </summary>
    /// <param name="board">The game board.</param>
    /// <returns>True if the game is over, false otherwise.</returns>
    private bool IsGameOver(string[,] board)
    {
        foreach (var piece in board)
        {
            if (piece == "_") return false;
        }
        return true;
    }

    #endregion
}
