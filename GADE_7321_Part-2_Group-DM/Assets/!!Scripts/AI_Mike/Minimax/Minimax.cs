using System.Collections.Generic;
using UnityEngine;

public class Minimax : MonoBehaviour
{
    [SerializeField] private UtilityFunctionManager utilityFunction;
    [SerializeField] private PieceCaptureHandler captureHandler;

    public (float, Vector2) MinimaxFunction(string[,] board, int depth, bool isMaximizingPlayer, string currentPlayerColour, float alpha, float beta)
    {
        string opponentColor = currentPlayerColour == "Red" ? "Blue" : "Red";

        if (depth == 0 || IsGameOver(board))
        {
            return (utilityFunction.UtilityFunction(board, currentPlayerColour), Vector2.negativeInfinity); // return utility value and invalid move
        }

        if (isMaximizingPlayer)
        {
            float maxEval = float.MinValue;
            Vector2 bestMove = Vector2.negativeInfinity;
            foreach (var move in GetAllPossibleMoves(board))
            {
                string[,] newBoard = ApplyMove(board, move, currentPlayerColour);
                float eval = MinimaxFunction(newBoard, depth - 1, false, currentPlayerColour, alpha, beta).Item1;
                if (eval > maxEval)
                {
                    maxEval = eval;
                    bestMove = move;
                }
                alpha = Mathf.Max(alpha, eval);
            }
            return (maxEval, bestMove);
        }
        else
        {
            float minEval = float.MaxValue;
            Vector2 bestMove = Vector2.negativeInfinity;
            foreach (var move in GetAllPossibleMoves(board))
            {
                string[,] newBoard = ApplyMove(board, move, opponentColor);
                float eval = MinimaxFunction(newBoard, depth - 1, true, currentPlayerColour, alpha, beta).Item1;
                if (eval < minEval)
                {
                    minEval = eval;
                    bestMove = move;
                }
                beta = Mathf.Min(beta, eval);
            }
            return (minEval, bestMove);
        }
    }

    private List<Vector2> GetAllPossibleMoves(string[,] board)
    {
        List<Vector2> possibleMoves = new List<Vector2>();
        int size = board.GetLength(0);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (board[i, j] == "_")
                {
                    possibleMoves.Add(new Vector2(i, j));
                }
            }
        }
        return possibleMoves;
    }

    private string[,] ApplyMove(string[,] board, Vector2 move, string currentPlayerColour)
    {
        int size = board.GetLength(0);
        int x = (int)move.x;
        int y = (int)move.y;

        string[,] newBoard = (string[,])board.Clone();
        newBoard[x, y] = currentPlayerColour;
        captureHandler.GetDiagonals(newBoard, x, y, currentPlayerColour, false); // Update the board with captured pieces
        return newBoard;
    }

    private bool IsGameOver(string[,] board)
    {
        foreach (var piece in board)
        {
            if (piece == "_") return false;
        }
        return true;
    }
}
