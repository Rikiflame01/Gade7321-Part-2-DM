using System;
using System.Collections;
using System.Collections.Generic;
using __Scripts.Board;
using __Scripts;
using UnityEngine;
using UnityEngine.Events;

public class BoardManager : MonoBehaviour
{
    //Getting all references and settings
    [Header("References")] 
    [SerializeField] private GameStateData gameStateData;
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private int gameEndAmount = 120;
    [SerializeField] private PieceSpawner _pieceSpawner;

    public int NumberOfPiecesPlayed => _numPiecesOnBoard; 

    private int _numPiecesOnBoard;
    private bool isGameEnd;
    private Player _playerTurn;
    
    //All boards separated for minimax, minimxa will be calculated on board being played
    private string[,] boardOne = new string[5, 5]; //Front Face on start
    private string[,] boardTwo = new string[5, 5]; //Too the right of one
    private string[,] boardThree = new string[5, 5]; //Opposite of one
    private string[,] boardFour = new string[5, 5]; //Too left of one 
    private string[,] boardFive = new string[5, 5]; // Top of one
    private string[,] boardSix = new string[5, 5]; //Bottom of one

    public UnityEvent<string[,], int, int, string> OnPiecePlaced;
    public UnityEvent<int> PiecePlaced;
    public UnityEvent OnEndGame;

    private void Awake()
    {
        //setup
        gameStateData.playerTurn = Player.Blue;
        gameStateData.currentBoard = 0;
        gameStateData.numBluePieces = 0;
        gameStateData.numRedPieces = 0;
        InitialiseBoards();
    }

    private void Start()
    {
        _playerTurn = gameStateData.playerTurn;
    }

    public void TryPlacePiece(Vector3 position, BoardPiece boardPiece, Vector3 coordinates) //Subscription to unity event 
    {
        if (boardPiece.IsPieceOccupied() || isGameEnd)
        {
            Debug.LogError("Piece is occupied, OR game has ended");
            return;
        }
        
        boardPiece.PlacePiece(_pieceSpawner.SpawnSphere(position, _playerTurn));
        PlacePieceOnBoard((int)coordinates.x, (int)coordinates.y, _playerTurn.ToString()); //place piece in 2D array
        gameStateData.UpdatePieces(_playerTurn.ToString(), 1);
        _numPiecesOnBoard++;
        PiecePlaced?.Invoke(gameEndAmount - _numPiecesOnBoard);
        
        ChangePlayerTurn();
        
        if (_numPiecesOnBoard >= gameEndAmount) //Check for game end
        {
            Debug.LogError("Game Ended");
            isGameEnd = true;
            OnEndGame?.Invoke();
        }
    }

    void ChangePlayerTurn() 
    {
        _playerTurn = _playerTurn == Player.Blue ? Player.Red : Player.Blue;
        gameStateData.playerTurn = _playerTurn;
    }

    void PlacePieceOnBoard(int x, int y, string piece)
    {
        switch (gameStateData.currentBoard) //Get board to see which board array to update
        {
            case 0:
                ShowBoard(boardOne);
                boardOne[x, y] = piece;
                PlaceEdgePiece(boardFive, boardSix, boardFour, boardTwo,
                    x,y, piece);
                OnPiecePlaced?.Invoke(boardOne, x, y, _playerTurn.ToString()); //Fire event for capture logic
                ShowBoard(boardFive);
                break;
            case 1:
                boardTwo[x, y] = piece;
                PlaceEdgePiece(boardFive, boardSix, boardOne, boardThree,
                    x,y, piece);
                OnPiecePlaced?.Invoke(boardTwo, x, y, _playerTurn.ToString()); //Fire event for capture logic
                break;
            case 2:
                boardThree[x, y] = piece;
                PlaceEdgePiece(boardFive, boardSix, boardTwo, boardFour,
                    x,y, piece);
                OnPiecePlaced?.Invoke(boardThree, x, y, _playerTurn.ToString()); //Fire event for capture logic
                break;
            case 3:
                boardFour[x, y] = piece;
                PlaceEdgePiece(boardFive, boardSix, boardThree, boardOne,
                    x,y, piece);
                OnPiecePlaced?.Invoke(boardFour, x, y, _playerTurn.ToString()); //Fire event for capture logic
                break;
            case 4:
                boardFive[x, y] = piece;
                PlaceEdgePiece(boardOne, boardThree, boardTwo, boardFour,
                    x,y, piece);
                OnPiecePlaced?.Invoke(boardFive, x, y, _playerTurn.ToString()); //Fire event for capture logic
                ShowBoard(boardFive);
                break;
            case 5:
                boardSix[x, y] = piece;
                PlaceEdgePiece(boardOne, boardThree, boardTwo, boardFour,
                    x,y, piece);
                OnPiecePlaced?.Invoke(boardSix, x, y, _playerTurn.ToString()); //Fire event for capture logic
                break;
        }
    }

    void PlaceEdgePiece(string[,] boardAbove, string[,] boardBelow, string[,] boardLeft, string[,] boardRight, int row, int col, string piece)
    {
        //Get edge pieces to update other 2D boards, as boards are connected 
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

    public void ChangeBoardNum(int num)
    {
        gameStateData.currentBoard = num;
    }

    #endregion
    
    #region  Board Setup

    void InitialiseBoards() //Setup boards
    {
        SetupBoard(boardOne);
        SetupBoard(boardTwo);
        SetupBoard(boardThree);
        SetupBoard(boardFour);
        SetupBoard(boardFive);
        SetupBoard(boardSix);
    }

    void SetupBoard(string[,] board) //Make the spots empty
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = "_";
                //if (i == 1 && j == 2) board[i, j] = "Red";
                //if (i == 2 && j == 1) board[i, j] = "Red";
                //if (i == 2 && j == 3) board[i, j] = "Blue";
                //if (i == 3 && j == 2) board[i, j] = "Blue";
            }
            
        }
    }

    #endregion
    
}
