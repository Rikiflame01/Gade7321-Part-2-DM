using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MinimaxHandler : MonoBehaviour
{
    [Header("References: ")]
    [SerializeField] private Minimax minimax;
    [SerializeField] private PieceCaptureHandler captureHandler;
    [SerializeField] private string aiColour = "Red";

    [Header("Settings")] public int miniMaxDepth = 4;

    public UnityEvent<MoveData> onAIPlacePiece;

    public void PlacePiece(BoardMove boardPiece)
    {
        var board = boardPiece.board;
        Debug.Log("AI is calculating move using Minimax");

        if (IsBoardFull(board))
        {
            return;
        }

        // Call the Minimax function to get the best move
        var result = minimax.MinimaxFunction(board, miniMaxDepth, true, aiColour, float.MinValue, float.MaxValue);
        Vector2 bestMove = result.Item2;

        if (bestMove != Vector2.negativeInfinity)
        {
            int x = (int)bestMove.x;
            int y = (int)bestMove.y;
            Debug.Log($"AI performing move at ({x}, {y})");
            SubmitAIMove(new Vector2(x, y));
        }
        else
        {
            Debug.LogWarning("No valid moves found by AI.");
        }
    }

    private void SubmitAIMove(Vector2 move)
    {
        FaceBoard faceBoard = captureHandler.GetPiece((int)move.x, (int)move.y);
        MoveData boardData = new MoveData()
        {
            Position = faceBoard.GetBoardPiece().transform.position,
            Piece = faceBoard.GetBoardPiece(),
            Coordinate = faceBoard.Coordinates,
            AITurn = false
        };
        onAIPlacePiece?.Invoke(boardData);
    }

    private bool IsBoardFull(string[,] board)
    {
        foreach (var piece in board)
        {
            if (piece == "_") return false;
        }
        return true;
    }
}