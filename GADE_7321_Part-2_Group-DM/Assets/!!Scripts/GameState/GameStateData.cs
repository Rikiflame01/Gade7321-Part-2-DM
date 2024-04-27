using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game State Data", menuName = "Game Data/Game State Data")]
public class GameStateData : ScriptableObject
{
    public Player playerTurn;
    public int numRedPieces;
    public int numBluePieces;
    public int currentBoard;
}

public enum Player
{
    Blue,
    Red
}
