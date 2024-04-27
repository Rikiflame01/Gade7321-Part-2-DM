using System;
using System.Collections;
using System.Collections.Generic;
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
    private Player _playerTurn;

    public UnityEvent<Vector3, Player> OnBoardPieceClicked;

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
        _playerTurn = gameStateData.playerTurn;
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

        Ray mousePos = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(mousePos, out hit, 100f, _boardPiece))
        {
            OnBoardPieceClicked?.Invoke(hit.transform.position, _playerTurn);
            
            if (_playerTurn == Player.Blue) gameStateData.numBluePieces++;
            else gameStateData.numRedPieces++;
            
            _playerTurn = _playerTurn == Player.Blue ? Player.Red : Player.Blue;
            Debug.Log($"Hit object {hit.transform.name} | transform: {hit.transform.position}");
        }

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
