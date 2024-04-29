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
    [SerializeField] private int gameEndAmount = 120;
    [SerializeField] private PieceSpawner _pieceSpawner;

    public int NumberOfPiecesPlayed => _numPiecesOnBoard;

    private int _numPiecesOnBoard;
    private bool isGameEnd;
    private Player _playerTurn;
    
    private string[,] boardOne = new string[5, 5]; //Front Face on start
    private string[,] boardTwo = new string[5, 5]; //Too the right of one
    private string[,] boardThree = new string[5, 5]; //Opposite of one
    private string[,] boardFour = new string[5, 5]; //Too left of one 
    private string[,] boardFive = new string[5, 5]; // Top of one
    private string[,] boardSix = new string[5, 5]; //Bottom of one

    public UnityEvent<string[,], int, int, string> OnPiecePlaced;
    public UnityEvent OnEndGame;

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
        if (boardPiece.IsPieceOccupied() || isGameEnd)
        {
            Debug.LogError("Piece is occupied, OR game has ended");
            return;
        }
        
        Debug.Log("Piece is not occupied");
        boardPiece.PlacePiece(_pieceSpawner.SpawnSphere(position, _playerTurn));
        PlacePieceOnBoard((int)boardPiece.Coordinates.x, (int)boardPiece.Coordinates.y, _playerTurn.ToString());
        ChangePlayerTurn();
        _numPiecesOnBoard++;
        if (_numPiecesOnBoard >= gameEndAmount)
        {
            isGameEnd = true;
            OnEndGame?.Invoke();
        }
        
        
    }

    void ChangePlayerTurn() { _playerTurn = _playerTurn == Player.Blue ? Player.Red : Player.Blue; }

    void PlacePieceOnBoard(int x, int y, string piece)
    {
        switch (gameStateData.currentBoard)
        {
            case 1:
                
                boardOne[x, y] = piece;
                PlaceEdgePiece(boardFive, boardSix, boardFour, boardTwo,
                    x,y, piece);
                OnPiecePlaced?.Invoke(boardOne, x, y, _playerTurn.ToString());
                ShowBoard(boardFive);
                break;
            case 2:
                boardTwo[x, y] = piece;
                PlaceEdgePiece(boardFive, boardSix, boardOne, boardThree,
                    x,y, piece);
                OnPiecePlaced?.Invoke(boardTwo, x, y, _playerTurn.ToString());
                break;
            case 3:
                boardThree[x, y] = piece;
                PlaceEdgePiece(boardFive, boardSix, boardTwo, boardFour,
                    x,y, piece);
                break;
            case 4:
                boardFour[x, y] = piece;
                PlaceEdgePiece(boardFive, boardSix, boardThree, boardOne,
                    x,y, piece);
                break;
            case 5:
                boardFive[x, y] = piece;
                PlaceEdgePiece(boardOne, boardThree, boardTwo, boardFour,
                    x,y, piece);
                ShowBoard(boardFive);
                break;
            case 6:
                boardSix[x, y] = piece;
                PlaceEdgePiece(boardOne, boardThree, boardTwo, boardFour,
                    x,y, piece);
                break;
        }
    }

    void PlaceEdgePiece(string[,] boardAbove, string[,] boardBelow, string[,] boardLeft, string[,] boardRight, int row, int col, string piece)
    {
        if (row == 0)
        {
            boardAbove[4, col] = piece;
        }

        if (col == 0)
        {
            boardLeft[row, 4] = piece;
        }

        if (row == 4)
        {
            boardBelow[0, col] = piece;
        }

        if (col == 4)
        {
            boardRight[row, 0] = piece;
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

    public void ShowBoard()
    {
        Debug.Log("Board One");
        ShowBoard(boardOne);
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
