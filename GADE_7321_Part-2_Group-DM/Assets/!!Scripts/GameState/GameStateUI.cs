using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateUI : MonoBehaviour
{
    [Header("Game State")] public GameStateData gameStateData;
    [SerializeField] private PlayerInfo playerInfo;
    
    [Header("UI References: ")]
    public TMP_Text bluePiecesText;
    public TMP_Text redPiecesText;
    public TMP_Text piecesLeftText;
    public TMP_Text turnText;
    public Image colourIndicator;
    public GameObject endGameScreen;

    private string firstPlayerName = "";
    
    void Awake()
    {
        //InitializeTurnDisplay();
        
    }

    private void InitializeTurnDisplay()
    {
        if (playerInfo.player1.playerName != null)
        {
            // Blue always goes first, so check which player is Blue
            firstPlayerName = (playerInfo.player1.playerColour == gameStateData.playerTurn.ToString()) ? playerInfo.player1.playerName : playerInfo.player2.playerName;
            turnText.text = $"{firstPlayerName}";   
        }
        else
        {
            turnText.text = gameStateData.playerTurn.ToString();
        }

    }

    public void HandlePiecePlaced(int numPiecesLeft)
    {
        //Set UI for perfect information
        piecesLeftText.text = $"{numPiecesLeft.ToString()}";
        bluePiecesText.text = $"{gameStateData.numBluePieces}";
        redPiecesText.text = $"{gameStateData.numRedPieces}";
        turnText.text = turnText.text == playerInfo.player1.playerName
            ? playerInfo.player2.playerName
            : playerInfo.player1.playerName;
        colourIndicator.color = gameStateData.playerTurn != Player.Blue ?  Color.blue : Color.red;
    }

    public void UpdateUI()
    {
        InitializeTurnDisplay();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnHome()
    {
        SceneManager.LoadScene(0);
    }
    
}
