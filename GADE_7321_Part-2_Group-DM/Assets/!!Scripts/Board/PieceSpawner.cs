using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [Header("Piece Prefab")] 
    [SerializeField] private GameObject blueSpherePrefab;
    [SerializeField] private GameObject redSpherePrefab;

    public GameObject SpawnSphere(Vector3 position, Player playerTurn) //Spawning different colour spheres based on player turn
    {
        GameObject obj = null;
        //Debug.Log($"Spawning piece at: {position}");
        
        if(playerTurn == Player.Blue)  obj =  Instantiate(blueSpherePrefab, position, Quaternion.identity);  
        if(playerTurn == Player.Red)  obj =  Instantiate(redSpherePrefab, position, Quaternion.identity);
        return obj;

    }
}
