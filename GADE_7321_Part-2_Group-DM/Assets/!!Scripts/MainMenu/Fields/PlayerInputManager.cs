using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public GameObject gameCanvas;
    public GameObject Input;
    public GameStateUI ui;

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
            ui.UpdateUI();
            Input.SetActive(true);
            gameCanvas.SetActive(false);
            
        }
    }

    void OnDestroy()
    {
        submitButton.onClick.RemoveListener(SubmitNamesAndRandomizeColors);
    }
}
