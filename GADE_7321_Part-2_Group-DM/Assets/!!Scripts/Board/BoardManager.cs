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
    [SerializeField] private PieceSpawner _pieceSpawner;
    
    [Header("Settings: ")]
    [SerializeField] private int gameEndAmount = 120;

    [SerializeField] private float aiMoveWaitTime = 2f;
    
    [Space]
    [Header("AI Gameplay")]
    [SerializeField] private bool AIGameplay;

    private int _numPiecesOnBoard;
    private bool isGameEnd;
    private Player _playerTurn;
    private BoardMove currentBoardMove;

    private bool _isAITurn;
    
    //All boards separated for minimax, minimxa will be calculated on board being played
    private string[,] boardOne = new string[5, 5]; //Front Face on start
    private string[,] boardTwo = new string[5, 5]; //Too the right of one
    private string[,] boardThree = new string[5, 5]; //Opposite of one
    private string[,] boardFour = new string[5, 5]; //Too left of one 
    private string[,] boardFive = new string[5, 5]; // Top of one
    private string[,] boardSix = new string[5, 5]; //Bottom of one

    public string[,] BoardOne => boardOne;
    public string[,] BoardTwo => boardTwo;
    public string[,] BoardThree => boardThree;
    public string[,] BoardFour => boardFour;
    public string[,] BoardFive => boardFive;
    public string[,] BoardSix => boardSix;

    public UnityEvent<BoardMove> OnPiecePlaced;
    public UnityEvent<int> PiecePlaced;
    public UnityEvent OnEndGame;
    public UnityEvent<BoardMove> onAIMove;
    public UnityEvent<int, int> onCheckNumPieces;

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
        if (AIGameplay) //Setup board Move for AI
        {
            currentBoardMove = new BoardMove
            {
                board = boardOne,
                playerTurn = "Blue"
            };
        }
        
        gameStateData.aiPlaying = false;
        _playerTurn = gameStateData.playerTurn;
    }

    public void TryPlacePiece(Vector3 position, BoardPiece boardPiece, Vector3 coordinates) //Subscription to unity event 
    {
        if (boardPiece.IsPieceOccupied() || isGameEnd)
        {
            Debug.LogWarning("Piece Occupied");
            if(gameStateData.aiPlaying) StartCoroutine(AIPlacePieceDelayed(currentBoardMove));
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
            isGameEnd = true;
            OnEndGame?.Invoke();
        }
    }
    
    public void TryPlacePieceAI(MoveData moveData) //Subscription to unity event for AI
    {
        if(!AIGameplay) return;

        if (moveData.AITurn)
        {
            currentBoardMove.playerTurn = _playerTurn.ToString();
            StartCoroutine(AIPlacePieceDelayed(currentBoardMove));
        }
        else
        {
            TryPlacePiece(moveData.Position, moveData.Piece, moveData.Coordinate);
        }
    }

    public void HandleBoardFull(int board) //Perform AI move on new board if previous board was full
    {
        BoardMove newBoardMove = new();
        switch (board)
        {
            case 0: newBoardMove.board = boardOne;
                break;
            case 1: newBoardMove.board = boardTwo;
                break;
            case 2: newBoardMove.board = boardThree;
                break;
            case 3: newBoardMove.board = boardFour;
                break;
            case 4: newBoardMove.board = boardFive;
                break;
            case 5: newBoardMove.board = boardSix;
                break;
        }

        StartCoroutine(AIPlacePieceDelayed(newBoardMove));
    }
    
    private IEnumerator AIPlacePieceDelayed(BoardMove boardData)
    {
        yield return new WaitForSeconds(aiMoveWaitTime); //Wait so player can see easy move

        onAIMove?.Invoke(boardData); //Perform AI move
        
        if (AIGameplay) _isAITurn = false;
    }

    void ChangePlayerTurn() //Change player turn
    {
        _playerTurn = _playerTurn == Player.Blue ? Player.Red : Player.Blue;
        gameStateData.playerTurn = _playerTurn;
    }

    void PlacePieceOnBoard(int x, int y, string piece)
    {
        BoardMove move = new BoardMove
        {
            x = x,
            y = y,
            playerTurn = _playerTurn.ToString()
        };

        switch (gameStateData.currentBoard) //Get board to see which board array to update
        {
            case 0:
                ShowBoard(boardOne);
                boardOne[x, y] = piece;
                PlaceEdgePiece(boardFive, boardSix, boardFour, boardTwo,
                    x,y, piece);
                move.board = boardOne;
                OnPiecePlaced?.Invoke(move); //Fire event for capture logic
                ShowBoard(boardFive);
                break;
            case 1:
                boardTwo[x, y] = piece;
                PlaceEdgePiece(boardFive, boardSix, boardOne, boardThree,
                    x,y, piece);
                move.board = boardTwo;
                OnPiecePlaced?.Invoke(move); //Fire event for capture logic
                break;
            case 2:
                boardThree[x, y] = piece;
                PlaceEdgePiece(boardFive, boardSix, boardTwo, boardFour,
                    x,y, piece);
                move.board = boardThree;
                OnPiecePlaced?.Invoke(move); //Fire event for capture logic
                break;
            case 3:
                boardFour[x, y] = piece;
                PlaceEdgePiece(boardFive, boardSix, boardThree, boardOne,
                    x,y, piece);
                move.board = boardFour;
                OnPiecePlaced?.Invoke(move);
                //OnPiecePlaced?.Invoke(boardFour, x, y, _playerTurn.ToString()); //Fire event for capture logic
                break;
            case 4:
                boardFive[x, y] = piece;
                PlaceEdgePiece(boardOne, boardThree, boardTwo, boardFour,
                    x,y, piece);
                move.board = boardFive;
                OnPiecePlaced?.Invoke(move);//Fire event for capture logic
                ShowBoard(boardFive);
                break;
            case 5:
                boardSix[x, y] = piece;
                PlaceEdgePiece(boardOne, boardThree, boardTwo, boardFour,
                    x,y, piece);
                move.board = boardSix;
                OnPiecePlaced?.Invoke(move);
                //OnPiecePlaced?.Invoke(boardSix, x, y, _playerTurn.ToString()); //Fire event for capture logic
                break;
        }

        currentBoardMove = move;
        //Update game UI
        onCheckNumPieces?.Invoke(NumPieces(move.board, "Red"), NumPieces(move.board, "Blue"));
        
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

    void ShowBoard(string[,] board) //For debugging 2D boards
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
            }
            
        }
    }

    #endregion


    private int NumPieces(string[,] board, string colour) //Getting number of each colour on the current board
    {
        int value = 0;

        foreach (var piece in board)
        {
            if (piece == colour)
            {
                value++;
            }
        }
        
        return value;
    }
}

public struct BoardMove
{
    public string[,] board;
    public int x;
    public int y;
    public string playerTurn;
}

public struct MoveData
{
    public Vector3 Position;
    public BoardPiece Piece;
    public Vector3 Coordinate;
    public bool AITurn;
}