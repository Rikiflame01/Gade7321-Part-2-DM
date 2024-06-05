using System;
using System.Collections;
using System.Collections.Generic;
using __Scripts.Board;
using UnityEngine;

public class FaceBoard : MonoBehaviour
{
    [field: SerializeField] public Vector3 Coordinates;
    [SerializeField] private bool changeStartPosition;
    //[SerializeField] private BoardData boardData;
    
    public Transform SpawnPosition { get; private set; }
    public BoardPiece BoardPiece { get; private set; }
    
    [SerializeField] private LayerMask layerMask;

    public List<Vector2> startPositions = new List<Vector2>();
    public static event Action<FaceBoard> OnPieceSpawn;

    private PieceSpawner _pieceSpawner;

    private void Awake()
    {
        _pieceSpawner = FindObjectOfType<PieceSpawner>();
        
        //boardData.allFaceBoards.Add(this);
    }

     private void OnEnable()
     {
         //StartCoroutine(ShowPieces());
         OnPieceSpawn?.Invoke(this);
     }

     IEnumerator ShowPieces() //For showing red and blue spheres in cube on start but is obsolete
     {
         Time.timeScale = 1;
        yield return new WaitForSeconds(0.25f);
        for (int i = 0; i < startPositions.Count; i++)
        {
            if (Coordinates.x == startPositions[i].x && Coordinates.y == startPositions[i].y)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.right, out hit, 2, layerMask))
                {
                    Debug.DrawRay(transform.position, transform.right * 2f, Color.blue);
                    if (hit.transform.TryGetComponent<BoardPiece>(out BoardPiece piece))
                    {
                        if (i > 1)
                        {
                            //_pieceSpawner.SpawnSphere(piece.transform.position, Player.Blue);
                            piece.PlacePiece(_pieceSpawner.SpawnSphere(piece.transform.position, Player.Blue));
                        }
                        else
                        {
                            piece.PlacePiece(_pieceSpawner.SpawnSphere(piece.transform.position, Player.Red));
                            //_pieceSpawner.SpawnSphere(piece.transform.position, Player.Red);
                        }
                    }
                }
                
            }
            
        }
    }
    

    public void PopulateData(GameStateData gameStateData) //Get Data from cube using the correct coordinates, Face board has the correct coordinates
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.right, out hit, 2, layerMask)) // use a raycast to get BoardPiece
        {
            Debug.DrawRay(transform.position, transform.right * 2f, Color.red);
            if (hit.transform.TryGetComponent<BoardPiece>(out BoardPiece piece))
            {
                piece.face = gameStateData.currentBoard;
                SpawnPosition = hit.transform;
                BoardPiece = piece;
            }
        }
        
    }

    public BoardPiece GetBoardPiece()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.right, out hit, 2, layerMask)) //
        {
            Debug.DrawRay(transform.position, transform.right * 2f, Color.red);
            if (hit.transform.TryGetComponent<BoardPiece>(out BoardPiece piece))
            {
                return piece;
            }
        }

        return null;
    }

    public void ChangePieceColour(bool blue) //Using a raycast again to get the board piece
    {
        GetBoardPiece().ChangePieceColour(blue);
        
        // RaycastHit hit;
        //
        // if (Physics.Raycast(transform.position, transform.right, out hit, 2, layerMask)) //
        // {
        //     Debug.DrawRay(transform.position, transform.right * 2f, Color.red);
        //     if (hit.transform.TryGetComponent<BoardPiece>(out BoardPiece piece))
        //     {
        //         piece.ChangePieceColour(blue);
        //     }
        // }
    }
    
    
}

[CreateAssetMenu(menuName = "BoardData")]
public class BoardData : ScriptableObject
{
    public List<FaceBoard> allFaceBoards;
    public List<FaceBoard> activeFaceBoards;
}
