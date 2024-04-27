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
    private Player _playerTurn;
    
    private string[,] boardOne = new string[5, 5];
    private string[,] boardTwo = new string[5, 5];
    private string[,] boardThree = new string[5, 5];
    private string[,] boardFour = new string[5, 5];
    private string[,] boardFive = new string[5, 5];
    private string[,] boardSix = new string[5, 5];

    private void Awake()
    {
        gameStateData.currentBoard = 1;
        gameStateData.numBluePieces = 0;
        gameStateData.numBluePieces = 0;
        InitialiseBoards();
    }

    private void Start()
    {
        _playerTurn = gameStateData.playerTurn;
    }

    public void TryPlacePiece(Vector3 position, BoardPiece boardPiece)
    {
        if (boardPiece.IsPieceOccupied())
        {
            Debug.Log("Piece is occupied");
            return;
        }
        
        Debug.Log("Piece is not occupied");
        _pieceSpawner.SpawnSphere(position, _playerTurn);
        boardPiece.PlacePiece();
        PlacePieceOnBoard((int)boardPiece.Coordinates.x, (int)boardPiece.Coordinates.y, _playerTurn.ToString());
        ChangePlayerTurn();
    }

    void ChangePlayerTurn() { _playerTurn = _playerTurn == Player.Blue ? Player.Red : Player.Blue; }

    void PlacePieceOnBoard(int x, int y, string piece)
    {
        switch (gameStateData.currentBoard)
        {
            case 0:
                boardOne[x, y] = piece;
                break;
        }
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
