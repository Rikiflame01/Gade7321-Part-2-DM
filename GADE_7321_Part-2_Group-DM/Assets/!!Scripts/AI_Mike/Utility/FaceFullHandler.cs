using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class FaceFullHandler : MonoBehaviour
{
    [SerializeField] private GameStateData gameStateData;
    [SerializeField] private BoardManager boardManager;
    //Handle Face Full Logic
    
    public UnityEvent<int> onAIMove;

    //If we change face play AI move on different board face
    public void HandlePlayerChangeFace()
    {
        if(!gameStateData.aiPlaying) return;
        
        if(CheckIfBoardFull()) return;
        
        if (gameStateData.difficulty is Difficulty.Easy or Difficulty.Medium or Difficulty.Hard)
        {
            StartCoroutine(PlayAIMove());
        }
    }

    private bool CheckIfBoardFull()
    {
        List<string[,]> boards = new List<string[,]>
        {
            boardManager.BoardOne,
            boardManager.BoardTwo,
            boardManager.BoardThree,
            boardManager.BoardFour,
            boardManager.BoardFive,
            boardManager.BoardSix
        };

        int boardIndex = gameStateData.currentBoard;

        foreach (var square in boards[boardIndex])
        {
            if (square == "_")
            {
                return false;
            }
        }
        
        return false;
    }

    //Play the move from AI after changing face
    IEnumerator PlayAIMove()
    {
        yield return new WaitForSeconds(1f);
        if(!gameStateData.aiPlaying) yield return null;
        onAIMove?.Invoke(gameStateData.currentBoard);
    }

    public (int, int) GetFullAmountOfPieces()
    {
        int redPieces = 0;
        int bluePieces = 0;

        // Initialize boards
        List<string[,]> boards = new List<string[,]>
    {
        boardManager.BoardOne,
        boardManager.BoardTwo,
        boardManager.BoardThree,
        boardManager.BoardFour,
        boardManager.BoardFive,
        boardManager.BoardSix
    };

        // Define the size of the boards
        int boardSize = boards[0].GetLength(0) - 1;

        // List to store already counted coordinates
        HashSet<(int, int, int)> countedCoordinates = new HashSet<(int, int, int)>();

        // Iterate over each board
        for (int b = 0; b < boards.Count; b++)
        {
            for (int x = 0; x <= boardSize; x++)
            {
                for (int y = 0; y <= boardSize; y++)
                {
                    string piece = boards[b][x, y];
                    if (piece == "Red" || piece == "Blue")
                    {
                        var currentCoordinate = Get3DCoordinate(b, x, y, boardSize);

                        // Check if this coordinate has already been counted
                        if (!countedCoordinates.Contains(currentCoordinate))
                        {
                            if (piece == "Red") redPieces++;
                            if (piece == "Blue") bluePieces++;

                            // Add this coordinate to the set of counted coordinates
                            countedCoordinates.Add(currentCoordinate);
                        }
                    }
                }
            }
        }

        return (redPieces, bluePieces);
    }

    // Method to map 2D board coordinates to unique 3D coordinates
    private (int, int, int) Get3DCoordinate(int boardIndex, int x, int y, int boardSize)
    {
        switch (boardIndex)
        {
            case 0: // Front face
                return (x, y, 0);
            case 1: // Back face
                return (x, y, boardSize);
            case 2: // Left face
                return (0, x, y);
            case 3: // Right face
                return (boardSize, x, y);
            case 4: // Top face
                return (x, 0, y);
            case 5: // Bottom face
                return (x, boardSize, y);
            default:
                return (x, y, boardIndex); // Fallback, shouldn't happen
        }
    }



}