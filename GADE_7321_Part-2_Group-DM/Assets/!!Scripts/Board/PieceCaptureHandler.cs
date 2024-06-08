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
        GetDiagonals(board, x, y, boardMove.playerTurn);
    }

    public void GetDiagonals(string[,] board, int x, int y, string colour)
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

            CheckDiagonals(diagonalPositions, board, colour);
        }
    }

    private bool CheckDiagonals(List<Vector2> diagonalPositions, string[,] board, string colour)
    {
        bool captured = CheckCapturesOnly(diagonalPositions, board, colour);
        return captured;
    }

    public bool CheckCapturesOnly(List<Vector2> diagonalPositions, string[,] board, string colour)
    {
        CaptureData data = CapturePieces(diagonalPositions, board, colour);
        if (data.CapturePositions == null) return false;

        if (data.CapturePositions.Count > 0)
        {
            foreach (var pos in data.CapturePositions)
            {
                board[(int)pos.x, (int)pos.y] = data.FirstColour;
                ChangePieceVisual((int)pos.x, (int)pos.y, data.FirstColour == "Blue");
                if (data.FirstColour != colour)
                {
                    onPieceTaken?.Invoke($"{GetOppositeColour(data.FirstColour)} was trapped by {data.FirstColour}");
                }
                else
                {
                    onPieceTaken?.Invoke($"{GetOppositeColour(data.FirstColour)} was captured by {data.FirstColour}");
                }
                
            }
            return true;
        }

        return false;
    }

    public bool CheckTrapsOnly(List<Vector2> diagonalPositions, string[,] board, string colour)
    {
        CaptureData data = CapturePieces(diagonalPositions, board, colour);
        if (data.CapturePositions == null) return false;

        if (data.CapturePositions.Count > 0)
        {
            bool trap = false;
            foreach (var pos in data.CapturePositions)
            {
                board[(int)pos.x, (int)pos.y] = data.FirstColour;
                ChangePieceVisual((int)pos.x, (int)pos.y, data.FirstColour == "Blue");
                //onPieceTaken?.Invoke($"{GetOppositeColour(data.FirstColour)} was captured by {data.FirstColour}");
            }

            if (trap)
            {
                onPieceTaken?.Invoke($"{colour} was trapped by {data.FirstColour}");
                return true;
            }
        }

        return false;
    }

    private CaptureData CapturePieces(List<Vector2> diagonalPositions, string[,] board, string colour)
    {
        int size = diagonalPositions.Count;
        CaptureData answer = new CaptureData();

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
                    string middlePieceColour = board[(int)diagonalPositions[k].x, (int)diagonalPositions[k].y];
                    if (middlePieceColour == "_" || middlePieceColour == firstPieceColour)
                    {
                        validCapture = false;
                        break;
                    }
                    capturePositions.Add(diagonalPositions[k]);
                }

                if (validCapture)
                {
                    CaptureData captureData = new CaptureData()
                    {
                        CapturePositions = capturePositions,
                        FirstColour = firstPieceColour,
                        LastColour = lastPieceColour
                    };
                    return captureData;
                }
            }
        }

        return answer;
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

public struct CaptureData
{
    public string FirstColour;
    public string LastColour;
    public List<Vector2> CapturePositions;
}
