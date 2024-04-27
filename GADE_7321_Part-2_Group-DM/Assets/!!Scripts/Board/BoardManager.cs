using System;
using System.Collections;
using System.Collections.Generic;
using __Scripts.Board;
using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent<string[,], int, int, string> OnPiecePlaced;

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
            Debug.LogError("Piece is occupied");
            return;
        }
        
        Debug.Log("Piece is not occupied");
        _pieceSpawner.SpawnSphere(position, _playerTurn, out GameObject piece);
        boardPiece.PlacePiece(piece);
        PlacePieceOnBoard((int)boardPiece.Coordinates.x, (int)boardPiece.Coordinates.y, _playerTurn.ToString());
        ChangePlayerTurn();
    }

    void ChangePlayerTurn() { _playerTurn = _playerTurn == Player.Blue ? Player.Red : Player.Blue; }

    void PlacePieceOnBoard(int x, int y, string piece)
    {
        switch (gameStateData.currentBoard)
        {
            case 1:
                boardOne[x, y] = piece;
                ShowBoard(boardOne);
                OnPiecePlaced?.Invoke(boardOne, x, y, _playerTurn.ToString());
                break;
            case 2:
                boardTwo[x, y] = piece;
                ShowBoard(boardTwo);
                OnPiecePlaced?.Invoke(boardTwo, x, y, _playerTurn.ToString());
                break;
            case 3:
                boardThree[x, y] = piece;
                ShowBoard(boardThree);
                break;
            case 4:
                boardFour[x, y] = piece;
                ShowBoard(boardFour);
                break;
            case 5:
                boardFive[x, y] = piece;
                ShowBoard(boardFive);
                break;
            case 6:
                boardSix[x, y] = piece;
                ShowBoard(boardSix);
                break;
        }
    }

    #region Debugging

    void ShowBoard(string[,] board)
    {
        string debugBoard = "";
        Debug.Log($"Show current board: {gameStateData.currentBoard}");
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                debugBoard += board[i, j] + " ";
            }

            debugBoard += "\n";
        }
        
        Debug.Log(debugBoard);
    }

    #endregion
    
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
