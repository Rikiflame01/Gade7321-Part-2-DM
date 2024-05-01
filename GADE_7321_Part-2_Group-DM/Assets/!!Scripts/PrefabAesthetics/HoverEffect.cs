using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    public GameStateData gameStateData; 
    private Renderer renderer;
    private Color originalColor;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color; 
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
        {
            if (gameObject.layer == LayerMask.NameToLayer("BoardPiece"))
            {
                SetHoverColor();
            }
        }
        else
        {
            renderer.material.color = originalColor; 
        }
    }

    void SetHoverColor()
    {
        Color hoverColor;
        if (gameStateData.playerTurn == Player.Blue)
        {
            ColorUtility.TryParseHtmlString("#0000FF", out hoverColor); // Blue
        }
        else
        {
            ColorUtility.TryParseHtmlString("#FF0000", out hoverColor); // Red
        }

        renderer.material.color = hoverColor;
    }
}
