using System;
using System.Collections;
using System.Collections.Generic;
using __Scripts.Board;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMouseController : MonoBehaviour
{
    [SerializeField] private LayerMask _boardPiece;
    [SerializeField] private GameStateData gameStateData;
    
    private DefaultInputActions _playerInput;
    private Camera _mainCam;

    public UnityEvent<Vector3, BoardPiece, Vector3> OnBoardPieceClicked;

    private void Awake()
    {
        if (_playerInput == null)
        {
            _playerInput = new DefaultInputActions();
        }
    }

    private void Start()
    {
        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        _playerInput.Player.Enable();
        
        _playerInput.Player.Fire.performed += OnMouseClick;
    }

    private void OnDisable()
    {
        _playerInput.Player.Disable();
    }
    
    void OnMouseClick(InputAction.CallbackContext obj)
    {
        Debug.Log("Mouse Click");
        RaycastHit hit;

        Ray mousePos = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue()); //Get mouse position in world when player clicks

        if (Physics.Raycast(mousePos, out hit, 100f, _boardPiece)) //Fire a raycast to get the correct Face Board
        {
            if (hit.transform.TryGetComponent<FaceBoard>(out FaceBoard piece))
            {
                //Set up FaceBoard and boardPiece
                piece.PopulateData(gameStateData); 
                piece.BoardPiece.Coordinates = piece.Coordinates;
                OnBoardPieceClicked?.Invoke(piece.SpawnPosition.position, piece.BoardPiece, piece.Coordinates);
            }
        }
    }
    
}
