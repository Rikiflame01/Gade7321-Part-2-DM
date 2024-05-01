using System;
using System.Collections;
using UnityEngine;

namespace __Scripts.Board
{
    public class BoardPiece : MonoBehaviour
    {
        [SerializeField] private bool isOccupied;
        [field: SerializeField] public Vector3 Coordinates { get; set; }
        [field: SerializeField] public GameObject spherePiece;
        [SerializeField] private Material redMat;
        [SerializeField] private Material blueMat;

        private MeshRenderer _meshRenderer;
        private Material _material;

        public static event Action<BoardPiece> OnPieceSpawn;

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _material = blueMat;
        }

        private void OnEnable()
        {
            OnPieceSpawn?.Invoke(this);
        }

        public void PlacePiece(GameObject piece)
        {
            isOccupied = true;
            spherePiece = piece;
        }

        public bool IsPieceOccupied()
        {
            return isOccupied;
        }

        public void ChangePieceColour(bool blue)
        {
            _material = blue ? blueMat : redMat;
            if (spherePiece == null) return;
            StartCoroutine(ChangeColour(spherePiece.GetComponent<MeshRenderer>().material.color, _material.color));
            
        }

        IEnumerator ChangeColour(Color from, Color to)
        {
            float duration = 1f;
            float elapsedTime = 0f;
            
            while (elapsedTime <= duration)
            {
                elapsedTime+= Time.deltaTime;
                float progress = elapsedTime / duration;
                spherePiece.GetComponent<MeshRenderer>().material.color = Color.Lerp(from, to, progress);
                yield return null;
            }

            spherePiece.GetComponent<MeshRenderer>().material.color = to;
        }
    }
}