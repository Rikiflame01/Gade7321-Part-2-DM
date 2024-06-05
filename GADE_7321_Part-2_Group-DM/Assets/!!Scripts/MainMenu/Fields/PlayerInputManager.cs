using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/*
 * 
The PlayerInputManager script in Unity captures player names through input fields, 
assigns colors randomly to each player, and changes scenes upon successful submission. 
It handles button click events to submit names and color assignments. If a player name 
is missing, it logs a reminder; otherwise, it loads the specified scene. 
The script also manages event listeners for the submit button.
 */

public class PlayerInputManager : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public GameStateData gameData;

    public TMP_InputField player1InputField;
    public TMP_InputField player2InputField;

    public Button submitButton;

    void Start()
    {
        submitButton.onClick.AddListener(SubmitNamesAndRandomizeColors);
    }

    private void SubmitNamesAndRandomizeColors()
    {
        playerInfo.player1.playerName = player1InputField.text;
        playerInfo.player2.playerName = player2InputField.text;

        if (Random.value > 0.5f)
        {
            playerInfo.player1.playerColour = "Red";
            playerInfo.player2.playerColour = "Blue";
        }
        else
        {
            playerInfo.player1.playerColour = "Blue";
            playerInfo.player2.playerColour = "Red";
        }

        Debug.Log($"{playerInfo.player1.playerName} is {playerInfo.player1.playerColour}");
        Debug.Log($"{playerInfo.player2.playerName} is {playerInfo.player2.playerColour}");

        if (playerInfo.player1.playerName == "" || playerInfo.player2.playerName == "")
        {
            //Maybe load a fading warning panel here for the polished version.
            Debug.Log("Please enter a name for both players.");
            return;
        }
        else
        {
            SceneManager.LoadScene("MultiplayerScene");
        }
    }

    private void EnterPlayerInfoAI(string difficulty)
    {
        playerInfo.player1.playerName = "Player";
        playerInfo.player2.playerName = difficulty + "AI";
            
        if (Random.value > 0.5f)
        {
            playerInfo.player1.playerColour = "Red";
            playerInfo.player2.playerColour = "Blue";
        }
        else
        {
            playerInfo.player1.playerColour = "Blue";
            playerInfo.player2.playerColour = "Red";
        }
    }

    public void EnterEasy()
    {
        gameData.difficulty = Difficulty.Easy;
        EnterPlayerInfoAI("Easy");
        SceneManager.LoadScene("EasyScene");
    }

    public void EnterMedium()
    {
        gameData.difficulty = Difficulty.Medium;
        EnterPlayerInfoAI("Medium");
        SceneManager.LoadScene("MinimaxScene");
    }

    public void EnterDifficultScne()
    {
        gameData.difficulty = Difficulty.Hard;
        EnterPlayerInfoAI("Hard");
        SceneManager.LoadScene("MinimaxScene");
    }

    void OnDestroy()
    {
        submitButton.onClick.RemoveListener(SubmitNamesAndRandomizeColors);
    }
}
