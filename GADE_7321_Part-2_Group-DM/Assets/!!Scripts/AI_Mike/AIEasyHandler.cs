using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIEasyHandler : MonoBehaviour
{
    public UnityEvent<Vector2> onAIPlacePiece;
    
    public void PlacePiece(BoardMove boardPiece)
    {
        var board = boardPiece.board;
        
        Debug.Log("AI is placing piece");
        
        //Check if possible capture

        if (IsBoardFull(board))
        {
            return;
        }

        List<Vector2> possiblesMoves = GetAllPossibleMoves(board);
        
        //Get random move if capture not available

        Vector2 randomMove = possiblesMoves[Random.Range(0, possiblesMoves.Count)];
        
        Debug.Log(randomMove);
        
        onAIPlacePiece?.Invoke(randomMove);
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
}
