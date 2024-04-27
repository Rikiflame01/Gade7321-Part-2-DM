using UnityEngine;

namespace __Scripts.Board
{
    public class BoardPiece : MonoBehaviour
    {
        [SerializeField] private bool isOccupied;
        [field: SerializeField] public Vector3 Coordinates { get; set; }

        public void PlacePiece()
        {
            isOccupied = true;
        }

        public bool IsPieceOccupied()
        {
            return isOccupied;
        }
    }
}