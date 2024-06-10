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
    [SerializeField] private FaceFullHandler faceFullHandler;
    
    [Header("UI References: ")]
    public TMP_Text bluePiecesText;
    public TMP_Text redPiecesText;
    public TMP_Text piecesLeftText;
    public TMP_Text turnText;
    public Image colourIndicator;
    public GameObject endGameScreen;
    public TMP_Text endRedPiecesText;
    public TMP_Text endBluePiecesText;
    public TMP_Text endWinnerText;

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
        turnText.text = turnText.text == playerInfo.player1.playerName
            ? playerInfo.player2.playerName
            : playerInfo.player1.playerName;
        colourIndicator.color = gameStateData.playerTurn != Player.Blue ?  Color.blue : Color.red;
    }

    public void ShowNumberOfPiecesPerColour(int red, int blue)
    {
        bluePiecesText.text = $"{blue}";
        redPiecesText.text = $"{red}";
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

    public void ShowEndGameScreen() //Show the game end facts and winner
    {
         int redPieces = faceFullHandler.GetFullAmountOfPieces().Item1;
         int bluePieces = faceFullHandler.GetFullAmountOfPieces().Item2;

         endRedPiecesText.text = $"Red Pieces: {redPieces}";
         endBluePiecesText.text = $"Blue Pieces: {bluePieces}";

         if (redPieces > bluePieces)
         {
             endWinnerText.text = "Winner Red";
         }else if (redPieces == bluePieces)
         {
             endWinnerText.text = "Draw";
         }
         else
         {
             endWinnerText.text = "Winner Blue";
         }
         
         endGameScreen.SetActive(true);
    }
    
}
