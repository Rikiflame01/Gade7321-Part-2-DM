using System.Collections.Generic;
using UnityEngine;

public class Minimax : MonoBehaviour
{
    [SerializeField] private UtilityFunctionManager utilityFunction;
    [SerializeField] private PieceCaptureHandler captureHandler;

    public (float, int) MinimaxFunction(string[,] board, int depth, bool isMaximizingPlayer, string currentPlayerColor, float alpha, float beta)
    {
        string opponentColor = currentPlayerColor == "Red" ? "Blue" : "Red";

        if (depth == 0 || IsGameOver(board))
        {
            return (utilityFunction.UtilityFunction(board, currentPlayerColor, -1, -1), -1); // return utility value and invalid move
        }

        if (isMaximizingPlayer)
        {
            float maxEval = float.MinValue;
            int bestMove = -1;
            foreach (var move in GetAllPossibleMoves(board, currentPlayerColor))
            {
                string[,] newBoard = ApplyMove(board, move, currentPlayerColor);
                float eval = MinimaxFunction(newBoard, depth - 1, false, currentPlayerColor, alpha, beta).Item1;
                if (eval > maxEval)
                {
                    maxEval = eval;
                    bestMove = move;
                }
                alpha = Mathf.Max(alpha, eval);
                if (beta <= alpha)
                {
                    break; // beta cut-off
                }
            }
            return (maxEval, bestMove);
        }
        else
        {
            float minEval = float.MaxValue;
            int bestMove = -1;
            foreach (var move in GetAllPossibleMoves(board, opponentColor))
            {
                string[,] newBoard = ApplyMove(board, move, opponentColor);
                float eval = MinimaxFunction(newBoard, depth - 1, true, currentPlayerColor, alpha, beta).Item1;
                if (eval < minEval)
                {
                    minEval = eval;
                    bestMove = move;
                }
                beta = Mathf.Min(beta, eval);
                if (beta <= alpha)
                {
                    break; // alpha cut-off
                }
            }
            return (minEval, bestMove);
        }
    }

    private List<int> GetAllPossibleMoves(string[,] board, string currentPlayerColor)
    {
        List<int> possibleMoves = new List<int>();
        int size = board.GetLength(0);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (board[i, j] == "_")
                {
                    possibleMoves.Add(i * size + j); // Convert (i, j) to a single index
                }
            }
        }
        return possibleMoves;
    }

    private string[,] ApplyMove(string[,] board, int move, string currentPlayerColor)
    {
        int size = board.GetLength(0);
        int x = move / size;
        int y = move % size;

        string[,] newBoard = (string[,])board.Clone();
        newBoard[x, y] = currentPlayerColor;
        //captureHandler.GetDiagonals(newBoard, x, y, currentPlayerColor); // Update the board with captured pieces
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
