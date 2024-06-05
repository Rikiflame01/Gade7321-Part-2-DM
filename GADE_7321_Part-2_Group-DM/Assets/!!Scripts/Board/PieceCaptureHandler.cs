using System;
using System.Collections;
using System.Collections.Generic;
using __Scripts;
using __Scripts.Board;
using Unity.VisualScripting;
using UnityEngine;

public class PieceCaptureHandler : MonoBehaviour
{
    [SerializeField] private List<FaceBoard> pieces;
    [SerializeField] private GameStateData data;
    
    // Directions vectors for diagonals: northeast, northwest, southeast, southwest
    private int[,] _directions = new int[,]
    {
        { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }
    };
    private string[,] test = new string[5, 5];

    private List<Vector2> emptyList = new List<Vector2>();

    private void Start()
    {
        FaceBoard.OnPieceSpawn += PopulatePieces;
        
        //GetDiagonals(test, 2,2, "Red");
        
        List<Vector2> edgePieces = new List<Vector2>();

        for (int i = 0; i < 5; i++) //Set edge pieces
        {
            edgePieces.Add(new Vector2(0, i));
            edgePieces.Add(new Vector2(i, 0));
            edgePieces.Add(new Vector2(4, i));
            edgePieces.Add(new Vector2(i, 4));
        }
    }

    private void OnDestroy()
    {
        FaceBoard.OnPieceSpawn -= PopulatePieces;
    }

    public void CheckDiagonalTake(BoardMove boardMove)
    {
        string[,] board = boardMove.board;
        int x = boardMove.x;
        int y = boardMove.y;
        string currentPlayerColour = boardMove.playerTurn;
        
        GetDiagonals(board, x, y, currentPlayerColour);
    }

    public void GetDiagonals(string[,] board, int x, int y, string currentPlayerColour)
    {
        int size = board.GetLength(0);

        for (int i = 0; i < 4; i++)
        {
            int dx = _directions[i, 0];
            int dy = _directions[i, 1];

            // Check in both positive and negative directions
            for (int direction = -1; direction <= 1; direction += 2)
            {
                List<Vector2> diagonalPositions = new List<Vector2>();
                for (int j = 0; j < size; j++)
                {
                    int newX = x + direction * dx * j;
                    int newY = y + direction * dy * j;

                    if (!IsInBounds(newX, newY, size))
                        break;

                    diagonalPositions.Add(new Vector2(newX, newY));
                }
                CheckDiagonals(diagonalPositions, board, currentPlayerColour);
            }
        }
    }

    private void CheckDiagonals(List<Vector2> diagonalPositions, string[,] board, string currentPlayerColour)
    {
        int size = diagonalPositions.Count;

        // Debug output for diagonals
        Debug.Log("Diagonal Positions:");
        foreach (var pos in diagonalPositions)
        {
            Debug.Log($"({pos.x}, {pos.y}) - {board[(int)pos.x, (int)pos.y]}");
        }

        for (int i = 0; i < size - 2; i++)
        {
            string firstPieceColor = board[(int)diagonalPositions[i].x, (int)diagonalPositions[i].y];
            if (firstPieceColor == "_" || firstPieceColor != currentPlayerColour)
                continue;

            for (int j = i + 2; j < size; j++)
            {
                string lastPieceColor = board[(int)diagonalPositions[j].x, (int)diagonalPositions[j].y];
                if (lastPieceColor != currentPlayerColour)
                    continue;

                bool validCapture = true;
                List<Vector2> capturePositions = new List<Vector2>();

                for (int k = i + 1; k < j; k++)
                {
                    string middlePieceColor = board[(int)diagonalPositions[k].x, (int)diagonalPositions[k].y];
                    if (middlePieceColor == "_" || middlePieceColor == currentPlayerColour)
                    {
                        validCapture = false;
                        break;
                    }
                    capturePositions.Add(diagonalPositions[k]);
                }

                if (validCapture)
                {
                    foreach (var pos in capturePositions)
                    {
                        board[(int)pos.x, (int)pos.y] = currentPlayerColour;
                        ChangePieceVisual((int)pos.x, (int)pos.y, currentPlayerColour == "Blue");
                    }
                }
            }
        }
    }

    string  GetOppositeColour(string colour) //Utility method
    {
        var oppositeColour = colour == "Blue" ? "Red" : "Blue";
        return oppositeColour;
    }
    

    private bool IsInBounds(int x, int y, int size) //Check for in bounds
    {
        return x >= 0 && y >= 0 && x < size && y < size;
    }

    public void PopulatePieces(FaceBoard piece) //Get all FaceBoards
    {
        pieces.Add(piece);
    }

    private void ShowBoard(string[,] board) //Show board 2D array for debugging
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

        debugBoard += $" board: {data.currentBoard}";
        Debug.Log(debugBoard);
    }

    private void ChangePieceVisual(int x, int y, bool isBlue) //changing visual of Board Piece
    {
        foreach (var piece in pieces)
        {
            if (piece.Coordinates.x == x && piece.Coordinates.y == y && piece.isActiveAndEnabled)
            {
                //Debug.Log($"Changing piece colour, piece {piece.Coordinates} and face: {piece.face}");
                piece.ChangePieceColour(isBlue);
            }
        }
    }
}
