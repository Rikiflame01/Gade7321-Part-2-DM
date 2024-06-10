using System.Collections;
using UnityEngine;

/// <summary>
/// Changes the color of a piece when the mouse hovers over it, with a smooth transition.
/// </summary>
public class HoverEffect : MonoBehaviour
{
    [Header("Game State Data")]
    [Tooltip("Reference to the GameStateData scriptable object.")]
    public GameStateData gameStateData;

    private Renderer renderer;
    private Color originalColor;
    private Coroutine colorCoroutine;
    private static HoverEffect currentHoveredEffect = null;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
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
                        currentHoveredEffect.ResetColor();
                    }

                    currentHoveredEffect = hitEffect;
                    currentHoveredEffect.SetHoverColor();
                }
                break;
            }
        }

        if (!hitGridPiece && currentHoveredEffect != null)
        {
            currentHoveredEffect.ResetColor();
            currentHoveredEffect = null;
        }
    }

    private void SetHoverColor()
    {
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
        }
        colorCoroutine = StartCoroutine(LerpColor(GetHoverColor(), 1f));
    }

    public void ResetColor()
    {
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
        }
        colorCoroutine = StartCoroutine(LerpColor(originalColor, 1f));
    }

    private Color GetHoverColor()
    {
        Color hoverColor = originalColor;
        if (gameStateData != null)
        {
            if (gameStateData.playerTurn == Player.Blue)
            {
                ColorUtility.TryParseHtmlString("#0000FF", out hoverColor); // Blue
            }
            else if (gameStateData.playerTurn == Player.Red)
            {
                ColorUtility.TryParseHtmlString("#FF0000", out hoverColor); // Red
            }

            hoverColor.a = 0.5f; // Set alpha to 50%
        }
        return hoverColor;
    }

    private IEnumerator LerpColor(Color targetColor, float duration)
    {
        Color startColor = renderer.material.color;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            renderer.material.color = Color.Lerp(startColor, targetColor, time / duration);
            yield return null;
        }

        renderer.material.color = targetColor;
        colorCoroutine = null;
    }
}
