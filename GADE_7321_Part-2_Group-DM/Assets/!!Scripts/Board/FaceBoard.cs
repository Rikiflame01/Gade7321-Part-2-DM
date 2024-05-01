using System;
using System.Collections;
using System.Collections.Generic;
using __Scripts.Board;
using UnityEngine;

public class FaceBoard : MonoBehaviour
{
    [field: SerializeField] public Vector3 Coordinates;
    
    public Transform SpawnPosition { get; private set; }
    public BoardPiece BoardPiece { get; private set; }
    
    [SerializeField] private LayerMask layerMask;

    public void PopulateData()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.right, out hit, 2, layerMask))
        {
            Debug.DrawRay(transform.position, transform.right * 2f, Color.red);
            if (hit.transform.TryGetComponent<BoardPiece>(out BoardPiece piece))
            {
                SpawnPosition = hit.transform;
                BoardPiece = piece;
                Debug.Log($"Combined board piece: {piece.Coordinates}");
            }
        }
        
    }

    private void OnDrawGizmos()
    {
       Gizmos.color = Color.red;
       Gizmos.DrawRay(transform.position, transform.right * 2f);
    }
}
