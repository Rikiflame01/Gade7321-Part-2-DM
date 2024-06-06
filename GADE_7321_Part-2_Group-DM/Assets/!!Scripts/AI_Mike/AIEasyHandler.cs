using System.Collections;
using System.Collections.Generic;
using __Scripts.Board;
using UnityEngine;
using UnityEngine.Events;

public class AIEasyHandler : MonoBehaviour
{
    public UnityEvent<MoveData> onAIPlacePiece;
    
    [SerializeField] private PieceCaptureHandler captureHandler;
    
    public void PlacePiece(BoardMove boardPiece)
    {
        var board = boardPiece.board;
        //FaceBoard faceBoard = null;
        
        Debug.Log("AI is placing piece");
        
        //Check if possible capture

        if (IsBoardFull(board))
        {
            return;
        }

        Vector2 captureMove = GetCaptureMove(board, boardPiece.playerTurn);

        if (captureMove != new Vector2(5, 5))
        {
            SubmitAIMove(captureMove);
            Debug.Log($"Capturing Piece at: {captureMove}");
            return;
        }

        List<Vector2> possiblesMoves = GetAllPossibleMoves(board);

        Vector2 randomMove = possiblesMoves[Random.Range(0, possiblesMoves.Count)];
        
        Debug.Log($"Random Move at: {randomMove}, from {boardPiece.playerTurn}");
        SubmitAIMove(randomMove);
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
                    possibleMoves.Add(new Vector2(i, j)); // Convert (i, j) to a single index
                }
            }
        }
        return possibleMoves;
    }
    
    private Vector2 GetCaptureMove(string[,] board, string currentPlayerColor) //Make a copy of the board and check for captures
    {
        List<Vector2> possibleMoves = GetAllPossibleMoves(board);
        foreach (var move in possibleMoves)
        {
            string[,] simulatedBoard = (string[,])board.Clone();
            simulatedBoard[(int)move.x, (int)move.y] = currentPlayerColor;
            if (HasCapture(simulatedBoard, (int)move.x, (int)move.y, currentPlayerColor))
            {
                return move;
            }
        }
        return new Vector2( 5, 5);
    }
    
    private bool HasCapture(string[,] board, int x, int y, string currentPlayerColor)
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

                if (captureHandler.CheckCapturesOnly(diagonalPositions, board))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
