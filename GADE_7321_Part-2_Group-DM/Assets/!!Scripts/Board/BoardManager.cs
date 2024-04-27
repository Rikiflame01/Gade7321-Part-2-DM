using System;
using System.Collections;
using System.Collections.Generic;
using __Scripts.Board;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private GameStateData gameStateData;

    [SerializeField] private PieceSpawner _pieceSpawner;

    private int _numPiecesOnBoard;

    private string[,] boardOne = new string[5, 5];
    private string[,] boardTwo = new string[5, 5];
    private string[,] boardThree = new string[5, 5];
    private string[,] boardFour = new string[5, 5];
    private string[,] boardFive = new string[5, 5];
    private string[,] boardSix = new string[5, 5];

    private void Awake()
    {
        InitialiseBoards();
    }

    private void Start()
    {
        
    }

    public void TryPlacePiece(Vector3 position, BoardPiece boardPiece)
    {
        
    }
    
    #region  Board Setup

    void InitialiseBoards()
    {
        SetupBoard(boardOne);
        SetupBoard(boardTwo);
        SetupBoard(boardThree);
        SetupBoard(boardFour);
        SetupBoard(boardFive);
        SetupBoard(boardSix);
    }

    void SetupBoard(string[,] board)
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = "_";
            }
        }
    }

    #endregion
    
}
