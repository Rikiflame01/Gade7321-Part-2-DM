using UnityEngine;
using TMPro;

public class TurnDisplayManager : MonoBehaviour
{
    public GameStateData data;
    public PlayerInfo playerInfo;
    public TextMeshProUGUI turnTextDisplay; 

    void Awake()
    {
        InitializeTurnDisplay();
    }

    private void InitializeTurnDisplay() //Testing to see player turn on start
    {
        if (playerInfo.player1.playerName != null)
        {
            // Blue always goes first, so check which player is Blue
            string firstPlayerName = (playerInfo.player1.playerColour == "Blue") ? playerInfo.player1.playerName : playerInfo.player2.playerName;
            turnTextDisplay.text = $"{firstPlayerName}";   
        }
        else
        {
            turnTextDisplay.text = data.playerTurn.ToString();
        }

    }
}
