using System;
using System.Collections;
using System.Collections.Generic;
using __Scripts;
using __Scripts.Board;
using UnityEngine;

public class PieceCaptureHandler : MonoBehaviour
{
    [SerializeField] private List<BoardPiece> pieces;
    [SerializeField] private GameStateData data;
    
    // Directions vectors for diagonals: northeast, northwest, southeast, southwest
    int[,] _directions = new int[,] { {1, 1}, {1, -1}, {-1, 1}, {-1, -1} };

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
        int size = board.GetLength(0); // Assuming the board is square
        string oppositeColour = currentPlayerColor == "Blue" ? "Red" : "Blue";
        
        for (int i = 0; i < _directions.GetLength(0); i++)
        {
            int dx = _directions[i, 0];
            int dy = _directions[i, 1];

            // Check the first diagonal position
            int x1 = x + dx;
            int y1 = y + dy;

            // Check the second diagonal position
            int x2 = x + -1 * dx;
            int y2 = y + -1 * dy;

            // Check bounds and conditions for capture
            if (IsInBounds(x1, y1, size) && IsInBounds(x2, y2, size))
            {
                if (board[x1, y1] == oppositeColour && board[x2, y2] == oppositeColour)
                {
                    Debug.Log("***New Capture:***");
                    board[x, y] = oppositeColour;  // Capture the piece
                    data.UpdatePieces(oppositeColour, 1);
                    data.UpdatePieces(data.GetOppositeColour(currentPlayerColor), -1);
                    ChangePieceVisual(x, y, oppositeColour == "Blue");
                }
                
            }
        }
        
        ShowBoard(board);
    }

    public void CheckIfCapturedPiece(string[,] board, int x, int y, string currentPlayerColor)
    {
        Debug.LogWarning("Piece capture");
        int size = board.GetLength(0);  // Assuming the board is square
        
        for (int i = 0; i < _directions.GetLength(0); i++)
        {
            int dx = _directions[i, 0];
            int dy = _directions[i, 1];

            // Check the first diagonal position
            int nx = x + dx;
            int ny = y + dy;

            // Check the second diagonal position
            int nnx = x + 2 * dx;
            int nny = y + 2 * dy;

            // Check bounds and conditions for capture
            if (IsInBounds(nx, ny, size) && IsInBounds(nnx, nny, size))
            {
                if (board[nx, ny] != currentPlayerColor && board[nx, ny] != "_" && board[nnx, nny] == currentPlayerColor)
                {
                    board[nx, ny] = currentPlayerColor;  // Capture the piece
                    data.UpdatePieces(currentPlayerColor, 1);
                    data.UpdatePieces(data.GetOppositeColour(currentPlayerColor), -1);
                    Debug.Log($"Piece Captured at {x} and {y} piece: {currentPlayerColor}");
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
