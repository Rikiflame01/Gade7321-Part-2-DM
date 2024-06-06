using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PieceCaptureHandler : MonoBehaviour
{
    [SerializeField] private List<FaceBoard> pieces;
    [SerializeField] private GameStateData data;

    // Directions vectors for diagonals: northeast, northwest, southeast, southwest
    public int[,] Directions = new int[,]
    {
        { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }
    };

    public UnityEvent<string> onPieceTaken;

    private void Start()
    {
        FaceBoard.OnPieceSpawn += PopulatePieces;

        List<Vector2> edgePieces = new List<Vector2>();
        for (int i = 0; i < 5; i++) // Set edge pieces
        {
            edgePieces.Add(new Vector2(0, i));
            edgePieces.Add(new Vector2(i, 0));
            edgePieces.Add(new Vector2(4, i));
            edgePieces.Add(new Vector2(i, 4));
        }
    }

    private void OnDestroy()
    {
        FaceBoard.OnPieceSpawn -= PopulatePieces;
    }

    public void CheckDiagonalTake(BoardMove boardMove)
    {
        string[,] board = boardMove.board;
        int x = boardMove.x;
        int y = boardMove.y;

        Debug.Log($"CheckDiagonalTake called at ({x}, {y})");
        GetDiagonals(board, x, y);
    }

    public void GetDiagonals(string[,] board, int x, int y)
    {
        int size = board.GetLength(0);

        for (int i = 0; i < 4; i++)
        {
            int dx = Directions[i, 0];
            int dy = Directions[i, 1];

            List<Vector2> diagonalPositions = new List<Vector2>();
            // Check in both positive and negative directions
            for (int direction = -1; direction <= 1; direction += 2)
            {
                for (int j = 0; j < size; j++)
                {
                    int newX = x + direction * dx * j;
                    int newY = y + direction * dy * j;

                    if (!IsInBounds(newX, newY, size))
                        break;

                    diagonalPositions.Add(new Vector2(newX, newY));
                }
            }

            diagonalPositions.Sort((a, b) =>
            {
                if (a.x == b.x)
                    return a.y.CompareTo(b.y);
                return a.x.CompareTo(b.x);
            });

            CheckDiagonals(diagonalPositions, board);
        }
        
    }

    public bool CheckDiagonals(List<Vector2> diagonalPositions, string[,] board)
    {
        bool captured = CapturePieces(diagonalPositions, board);
        bool trapped = TrappedPiece(diagonalPositions, board);
        return captured || trapped;
    }

    public bool CheckCapturesOnly(List<Vector2> diagonalPositions, string[,] board)
    {
        bool captured = CapturePieces(diagonalPositions, board);

        return captured;
    }

    private bool CapturePieces(List<Vector2> diagonalPositions, string[,] board)
    {
        int size = diagonalPositions.Count;
        bool captured = false;

        for (int i = 0; i < size - 2; i++)
        {
            string firstPieceColour = board[(int)diagonalPositions[i].x, (int)diagonalPositions[i].y];
            if (firstPieceColour == "_") continue;

            for (int j = i + 2; j < size; j++)
            {
                string lastPieceColour = board[(int)diagonalPositions[j].x, (int)diagonalPositions[j].y];
                if (lastPieceColour != firstPieceColour) continue;

                bool validCapture = true;
                List<Vector2> capturePositions = new List<Vector2>();

                for (int k = i + 1; k < j; k++)
                {
                    string middlePieceColor = board[(int)diagonalPositions[k].x, (int)diagonalPositions[k].y];
                    if (middlePieceColor == "_" || middlePieceColor == firstPieceColour)
                    {
                        validCapture = false;
                        break;
                    }
                    capturePositions.Add(diagonalPositions[k]);
                }

                if (validCapture)
                {
                    foreach (var pos in capturePositions)
                    {
                        board[(int)pos.x, (int)pos.y] = firstPieceColour;
                        ChangePieceVisual((int)pos.x, (int)pos.y, firstPieceColour == "Blue");
                        onPieceTaken?.Invoke($"{GetOppositeColour(firstPieceColour)} was captured by {firstPieceColour}");
                    }
                    captured = true;
                }
            }
        }

        return captured;
    }

    private bool TrappedPiece(List<Vector2> diagonalPositions, string[,] board)
    {
        int size = diagonalPositions.Count;
        string firstColour = "_";
        int firstIndex = 0;
        int lastIndex = 0;
        string lastColour = "_";
        List<string> middleColours = new List<string>();

        for (int i = 0; i < size; i++)
        {
            string test = board[(int)diagonalPositions[i].x, (int)diagonalPositions[i].y];

            if (test != "_" && firstColour == "_")
            {
                firstColour = test;
                firstIndex = i;
            }

            if (i > 1 && test != "_" && test == firstColour)
            {
                lastColour = test;
                lastIndex = i;
            }
            Debug.Log($"Checking Diagonal at ( {(int)diagonalPositions[i].x}, {(int)diagonalPositions[i].y}");
        }

        if (lastIndex - firstIndex < 2) return false;

        for (int i = firstIndex + 1; i < lastIndex; i++)
        {
            string test = board[(int)diagonalPositions[i].x, (int)diagonalPositions[i].y];
            string middlePieceColour = board[(int)diagonalPositions[i].x, (int)diagonalPositions[i].y];
            middleColours.Add(middlePieceColour);
        }

        if (lastColour != "_" && firstColour != "_")
        {
            if (middleColours.Count > 0 && middleColours.TrueForAll(colour => colour != firstColour && colour != "_"))
            {
                Debug.Log($"Trapping pieces between ({diagonalPositions[firstIndex].x}, {diagonalPositions[firstIndex].y}) and ({diagonalPositions[lastIndex].x}, {diagonalPositions[lastIndex].y})");

                for (int k = firstIndex + 1; k < lastIndex; k++)
                {
                    Vector2 pos = diagonalPositions[k];
                    board[(int)pos.x, (int)pos.y] = firstColour;
                    ChangePieceVisual((int)pos.x, (int)pos.y, firstColour == "Blue");
                    onPieceTaken?.Invoke($"{GetOppositeColour(firstColour)} entered a trap");
                }
                return true;
            }
        }

        return false;
    }

    string GetOppositeColour(string colour) // Utility method
    {
        return colour == "Blue" ? "Red" : "Blue";
    }

    public bool IsInBounds(int x, int y, int size) // Check for in bounds
    {
        return x >= 0 && y >= 0 && x < size && y < size;
    }

    public void PopulatePieces(FaceBoard piece) // Get all FaceBoards
    {
        pieces.Add(piece);
    }

    private void ChangePieceVisual(int x, int y, bool isBlue)
    {
        FaceBoard piece = GetPiece(x, y);
        if (piece != null)
        {
            piece.ChangePieceColour(isBlue);
        }
        else
        {
            Debug.LogWarning($"No piece found at ({x}, {y}) to change visual.");
        }
    }

    public FaceBoard GetPiece(int x, int y)
    {
        foreach (var piece in pieces)
        {
            if (piece.Coordinates.x == x && piece.Coordinates.y == y && piece.isActiveAndEnabled)
            {
                return piece;
            }
        }
        return null;
    }

    private void ShowBoard(string[,] board)
    {
        string debugBoard = "";
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                debugBoard += board[i, j] + " ";
            }
            debugBoard += "\n";
        }

        Debug.Log("\n" + debugBoard);
    }
}
