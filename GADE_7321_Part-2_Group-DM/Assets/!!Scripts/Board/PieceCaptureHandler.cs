using System;
using System.Collections;
using System.Collections.Generic;
using __Scripts.Board;
using UnityEngine;

public class PieceCaptureHandler : MonoBehaviour
{
    [SerializeField] private List<BoardPiece> pieces;

    private void Start()
    {
        BoardPiece.OnPieceSpawn += PopulatePieces;
    }

    private void OnDestroy()
    {
        BoardPiece.OnPieceSpawn -= PopulatePieces;
    }

    public void CheckIfPieceCaptured(string[,] board, int x, int y, string currentPlayerColor)
    {
        Debug.LogWarning("Piece capture");
        int size = board.GetLength(0);  // Assuming the board is square

        // Directions vectors for diagonals: northeast, northwest, southeast, southwest
        int[,] directions = new int[,] { {1, 1}, {1, -1}, {-1, 1}, {-1, -1} };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int dx = directions[i, 0];
            int dy = directions[i, 1];

            // Check the first diagonal position
            int nx = x + dx;
            int ny = y + dy;

            // Check the second diagonal position
            int nnx = x + 2 * dx;
            int nny = y + 2 * dy;

            // Check bounds and conditions for capture
            if (IsInBounds(nx, ny, size) && IsInBounds(nnx, nny, size))
            {
                if (board[nx, ny] != currentPlayerColor && board[nx, ny] != "" && board[nnx, nny] == currentPlayerColor)
                {
                    board[nx, ny] = currentPlayerColor;  // Capture the piece
                    ChangePieceVisual(nx, ny, currentPlayerColor == "Blue");
                }
            }
        }
        
        ShowBoard(board);
    }

    private bool IsInBounds(int x, int y, int size)
    {
        return x >= 0 && y >= 0 && x < size && y < size;
    }

    public void PopulatePieces(BoardPiece piece)
    {
        pieces.Add(piece);
    }

    private void ShowBoard(string[,] board)
    {
        Debug.Log("Show Captured Board");
        string debugBoard = "";
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                debugBoard += board[i, j];
            }

            debugBoard += "\n";
        }
        
        Debug.Log(debugBoard);
    }

    private void ChangePieceVisual(int x, int y, bool isBlue)
    {
        foreach (var piece in pieces)
        {
            if (piece.Coordinates.x == x && piece.Coordinates.y == y)
            {
                piece.ChangePieceColour(isBlue);
            }
        }
    }
}
