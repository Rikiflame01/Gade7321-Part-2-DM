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
    int[,] _directions = new int[,] { {1, 1}, {1, -1}, {-1, 1}, {-1, -1} };
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

    public void GetDiagonals(string[,] board, int x, int y, string currentPlayerColor)
    {
        int size = board.GetLength(0);

        List<Vector2> diagonalsOfCurrentMove = new List<Vector2>();
        
        for (int i = 0; i < 4; i++)
        {
            int dx = _directions[i, 0];
            int dy = _directions[i, 1];
            
            for (int j = 0; j < 5; j++)
            {
                // Check the first diagonal position
                int x1 = x + dx * j;
                int y1 = y + dy * j;

                // Check the second diagonal position
                int x2 = x + -1 * (dx * j);
                int y2 = y + -1 * (dy * j);
                
                //if (!IsInBounds(x1, y1, 5) || !IsInBounds(x2, y2, 5)) continue;
                if (IsInBounds(x1, y1, 5))
                {
                    Vector2 diagonal = new Vector2(x1, y1);

                    if (!diagonalsOfCurrentMove.Contains(diagonal))
                    {
                        diagonalsOfCurrentMove.Add(diagonal);
                    }
                    
                }

                if (IsInBounds(x2, y2, 5))
                {
                    Vector2 diagonal = new Vector2(x2, y2);

                    if (!diagonalsOfCurrentMove.Contains(diagonal))
                    {
                        diagonalsOfCurrentMove.Add(diagonal);
                    }
                }
            }

            //Sorting diagonal by x and y - if x of Vector a is equal to x of vector b then check if y less
            diagonalsOfCurrentMove.Sort((a, b) =>
            {
                if (a.x == b.x)
                    return a.y.CompareTo(b.y);
                return a.x.CompareTo(b.x);
            });
            
            CheckDiagonals(diagonalsOfCurrentMove, board);
            diagonalsOfCurrentMove.Clear();
            diagonalsOfCurrentMove = emptyList;
        }

        ShowBoard(board);
    }

    private void MiniPatch(string[,] board)
    {
        if (board[3, 3] != "_")
        {
            if (board[2, 2] == board[4, 4] && board[2, 2] != "_")
            {
                board[3, 3] = board[2, 2];
                ChangePieceVisual(3,3, board[3,3] == "Blue");
            }
        }
    }
    
    private void MiniPatch2(string[,] board)
    {
        if (board[3, 1] != "_")
        {
            if (board[2, 2] == board[4, 0] && board[2, 2] != "_")
            {
                board[3, 1] = board[2, 2];
                ChangePieceVisual(3,1, board[3,1] == "Blue");
            }
        }
    }

    private void CheckDiagonals(List<Vector2> diagonalsOfCurrentMove, string[,] board)
    {
        //This method checks the diagonals for potential captures
        
        //List<string> colours = new List<string>();
        for (int i = 0; i < diagonalsOfCurrentMove.Count; i++)
        {
            int x1 =(int) diagonalsOfCurrentMove[i].x;
            int y1 = (int)diagonalsOfCurrentMove[i].y;
            
            string currentColour = board[x1, y1];
            
            int count = 0;
            List<Vector2> colours = new List<Vector2>();
            string firstColour = "";
            
            if (currentColour != "_")
            {
                count = 0;
                for (int j = 1; j < diagonalsOfCurrentMove.Count; j++)
                {
                    if (i + j >= diagonalsOfCurrentMove.Count) return;
                    
                    int x2 = (int) diagonalsOfCurrentMove[i+j].x;
                    int y2 = (int) diagonalsOfCurrentMove[i+j].y;
                    string nextColour = board[x2, y2];
                    
                    if (j == 0) firstColour = nextColour;
                    if (nextColour != "_" /*&& nextColour != firstColour*/)
                    {
                        count++;
                        colours.Add(new Vector2(x2,y2));
                        if (count == 2) //If 3 pieces check capture
                        {
                            int difference = Mathf.Abs(x1 - x2);
                            if (difference > 2) return;
                            string test = board[(int)colours[0].x, (int)colours[0].y];
                            if (currentColour == nextColour && currentColour != board[(int)colours[0].x, (int)colours[0].y] && test != "_" ) //condition
                            {
                                //Change board array
                                board[(int)colours[0].x, (int)colours[0].y] = currentColour;
                                //Change board visual
                                ChangePieceVisual((int)colours[0].x, (int)colours[0].y, currentColour == "Blue");
                                //Change Data
                                data.UpdatePieces(currentColour, 1);
                                data.UpdatePieces(data.GetOppositeColour(currentColour), -1);
                                colours.Clear();
                                return;
                            }
                        }

                        if (count == 3) //if four pieces check capture of two pieces
                        {
                            
                            if (colours.Count < 2) return;
                            if (currentColour == nextColour && 
                                currentColour != board[(int)colours[0].x, (int)colours[0].y] &&
                                currentColour != board[(int)colours[1].x, (int)colours[1].y])
                            {
                                //Change board array
                                board[(int)colours[0].x, (int)colours[0].y] = currentColour;
                                board[(int)colours[1].x, (int)colours[1].y] = currentColour;
                                //Change board visual
                                ChangePieceVisual((int)colours[0].x, (int)colours[0].y, currentColour == "Blue");
                                ChangePieceVisual((int)colours[1].x, (int)colours[1].y, currentColour == "Blue");
                                //Change Data
                                data.UpdatePieces(currentColour, 2);
                                data.UpdatePieces(data.GetOppositeColour(currentColour), -2);
                                colours.Clear();
                                return;
                            }
                        }

                        if (count == 4) //if 5 pieces check capture of 3 pieces
                        {
                            if (colours.Count < 3) return;
                            if (currentColour == nextColour && 
                                currentColour != board[(int)colours[0].x, (int)colours[0].y] &&
                                currentColour != board[(int)colours[1].x, (int)colours[1].y] &&
                                currentColour != board[(int)colours[2].x, (int)colours[2].y])
                            {
                                //Change board array
                                board[(int)colours[0].x, (int)colours[0].y] = currentColour;
                                board[(int)colours[1].x, (int)colours[1].y] = currentColour;
                                board[(int)colours[2].x, (int)colours[2].y] = currentColour;
                                //Change board visual
                                ChangePieceVisual((int)colours[0].x, (int)colours[0].y, currentColour == "Blue");
                                ChangePieceVisual((int)colours[1].x, (int)colours[1].y, currentColour == "Blue");
                                ChangePieceVisual((int)colours[2].x, (int)colours[2].y, currentColour == "Blue");
                                //Change Data
                                data.UpdatePieces(currentColour, 3);
                                data.UpdatePieces(data.GetOppositeColour(currentColour), -3);
                                colours.Clear();
                                return;
                            }
                        }
                    }
                }
            }
            colours.Clear();
        }
        MiniPatch(board); //Fix up some diagonal capture logic
        MiniPatch2(board);
    }

    string  GetOppositeColour(string colour) //Utility method
    {
        var oppositeColour = colour == "Blue" ? "Red" : "Blue";
        return oppositeColour;
    }
    

    private bool IsInBounds(int x, int y, int size) //Check for in bounds
    {
        return (x >= 0 && y >= 0) && (x < size && y < size);
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
