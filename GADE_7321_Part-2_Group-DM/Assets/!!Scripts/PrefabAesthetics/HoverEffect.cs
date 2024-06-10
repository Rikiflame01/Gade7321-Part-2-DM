using System.Collections;
using UnityEngine;

/// <summary>
/// Changes the colour of a piece when the mouse hovers over it, with a smooth transition.
/// </summary>
public class HoverEffect : MonoBehaviour
{
    [Header("Game State Data")]
    [Tooltip("Reference to the GameStateData scriptable object.")]
    public GameStateData gameStateData;

    private Renderer renderer;
    private Color originalColour;
    private Coroutine colourCoroutine;
    private static HoverEffect currentHoveredEffect = null;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColour = renderer.material.color;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        bool hitGridPiece = false;

        foreach (RaycastHit hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("GridPiece"))
            {
                hitGridPiece = true;

                HoverEffect hitEffect = hitObject.GetComponent<HoverEffect>();

                if (hitEffect != null && currentHoveredEffect != hitEffect)
                {
                    if (currentHoveredEffect != null)
                    {
                        currentHoveredEffect.ResetColour();
                    }

                    currentHoveredEffect = hitEffect;
                    currentHoveredEffect.SetHoverColour();
                }
                break;
            }
        }

        if (!hitGridPiece && currentHoveredEffect != null)
        {
            currentHoveredEffect.ResetColour();
            currentHoveredEffect = null;
        }
    }

    private void SetHoverColour()
    {
        if (colourCoroutine != null)
        {
            StopCoroutine(colourCoroutine);
        }
        colourCoroutine = StartCoroutine(LerpColour(GetHoverColour(), 1f));
    }

    public void ResetColour()
    {
        if (colourCoroutine != null)
        {
            StopCoroutine(colourCoroutine);
        }
        colourCoroutine = StartCoroutine(LerpColour(originalColour, 1f));
    }

    private Color GetHoverColour()
    {
        Color hoverColour = originalColour;
        if (gameStateData != null)
        {
            if (gameStateData.playerTurn == Player.Blue)
            {
                ColorUtility.TryParseHtmlString("#0000FF", out hoverColour); // Blue
            }
            else if (gameStateData.playerTurn == Player.Red)
            {
                ColorUtility.TryParseHtmlString("#FF0000", out hoverColour); // Red
            }

            hoverColour.a = 0.5f; // Set alpha to 50%
        }
        return hoverColour;
    }

    private IEnumerator LerpColour(Color targetColour, float duration)
    {
        Color startColour = renderer.material.color;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            renderer.material.color = Color.Lerp(startColour, targetColour, time / duration);
            yield return null;
        }

        renderer.material.color = targetColour;
        colourCoroutine = null;
    }
}
