using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class BoardTracker : MonoBehaviour
{
    public GameObject[] empties;
    public GameObject[] boardFaces;
    public GameStateData gameStateData;
    public TextMeshProUGUI boardText;

    private GameObject currentClosestEmpty;
    private int currentBoardIndex = -1;

    private GameObject previousBoardFace;

    private void Start()
    {
        previousBoardFace = boardFaces[0];

        for (int i = 0; i < boardFaces.Length; i++) //Show faces
        {
            boardFaces[i].SetActive(true);
        }

        StartCoroutine(ShowFrontBoardOnly());
    }

    IEnumerator ShowFrontBoardOnly()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < boardFaces.Length; i++)
        {
            if(i == 0) continue;
            boardFaces[i].SetActive(false);
        }
    }

    void Update()
    {
        FindClosestEmpty();
    }

    void FindClosestEmpty()
    {
        //Set camera positions to closest
        float closestDistance = Mathf.Infinity;
        GameObject closestEmpty = null;
        int closestIndex = -1;

        for (int i = 0; i < empties.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, empties[i].transform.position); //Get distance from different points
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
            previousBoardFace.SetActive(false);
            previousBoardFace = boardFaces[closestIndex];
            previousBoardFace.SetActive(true);
        }
    }

    void UpdateGameStateData(int boardIndex)
    {
        if (gameStateData != null)
        {
            gameStateData.currentBoard = boardIndex;
            UpdateBoardText(boardIndex); //Update game data container
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
