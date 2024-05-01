using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace __Scripts.Board
{
    public class BoardPiece : MonoBehaviour
    {
        public int face = 10;
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
            _meshRenderer = piece.GetComponent<MeshRenderer>();
        }

        public bool IsPieceOccupied()
        {
            return isOccupied;
        }

        public void ChangePieceColour(bool blue)
        {
            _material = blue ? blueMat : redMat;
            if (spherePiece == null) return;
            StartCoroutine(ChangeColour(_meshRenderer.material.color, _material.color));
            
        }

        IEnumerator ChangeColour(Color from, Color to)
        {
            float duration = 1f;
            float elapsedTime = 0f;
            
            while (elapsedTime <= duration)
            {
                elapsedTime+= Time.deltaTime;
                float progress = elapsedTime / duration;
                _meshRenderer.material.color = Color.Lerp(from, to, progress);
                yield return null;
            }

            _meshRenderer.material.color = to;
        }
    }
}