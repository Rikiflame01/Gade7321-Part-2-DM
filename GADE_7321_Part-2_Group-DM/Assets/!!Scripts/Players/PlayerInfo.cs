using UnityEngine;

[CreateAssetMenu(fileName = "New Player Info", menuName = "Game Data/Player Info")]
public class PlayerInfo : ScriptableObject //Player info container
{
    public PlayerData player1;
    public PlayerData player2;
}

