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
        
        if (gameStateData.difficulty is Difficulty.Easy or Difficulty.Hard)
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
        
        List<string[,]> boards = new List<string[,]>
        {
            boardManager.BoardOne,
            boardManager.BoardTwo,
            boardManager.BoardThree,
            boardManager.BoardFour,
        };

        foreach (var board in boards)
        {
            foreach (var piece in board)
            {
                if (piece == "Red") redPieces++;
                if (piece == "Blue") bluePieces++;
            }
        }

        return (redPieces, bluePieces);
    }
    
}