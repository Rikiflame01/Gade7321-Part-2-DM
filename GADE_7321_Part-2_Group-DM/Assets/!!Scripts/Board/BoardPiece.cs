using UnityEngine;

namespace __Scripts.Board
{
    public class BoardPiece : MonoBehaviour
    {
        [SerializeField] private bool isOccupied;
        [field: SerializeField] public Vector2 Coordinates { get; private set; }

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