using UnityEngine;
using TMPro;

public class BoardTracker : MonoBehaviour
{
    public GameObject[] empties;
    public GameStateData gameStateData;
    public TextMeshProUGUI boardText;

    private GameObject currentClosestEmpty;
    private int currentBoardIndex = -1;

    void Update()
    {
        FindClosestEmpty();
    }

    void FindClosestEmpty()
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestEmpty = null;
        int closestIndex = -1;

        for (int i = 0; i < empties.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, empties[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEmpty = empties[i];
                closestIndex = i;
            }
        }

        if (closestEmpty != currentClosestEmpty && closestIndex != -1)
        {
            currentClosestEmpty = closestEmpty;
            currentBoardIndex = closestIndex;
            UpdateGameStateData(currentBoardIndex);
        }
    }

    void UpdateGameStateData(int boardIndex)
    {
        if (gameStateData != null)
        {
            gameStateData.currentBoard = boardIndex;
            UpdateBoardText(boardIndex);
            Debug.Log($"Current board updated to: {gameStateData.currentBoard}");
        }
        else
        {
            Debug.LogError("GameStateData reference not set in the BoardTracker script.");
        }
    }

    void UpdateBoardText(int boardIndex)
    {
        if (boardText != null)
        {
            boardText.text = $"{boardIndex + 1}";

        }
        else
        {
            Debug.LogError("TextMeshProUGUI reference not set in the BoardTracker script.");
        }
    }
}
