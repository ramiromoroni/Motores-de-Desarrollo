
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Animación del botón")]
    public float hoverScale = 1.05f;
    public float animationSpeed = 5f;

    [Header("Flecha")]
    public GameObject arrow;
    public float arrowOffset = 15f;
    public float arrowSpeed = 12f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    private RectTransform arrowRect;
    private Vector2 arrowOriginalPosition;
    private Vector2 arrowTargetPosition;

    private CanvasGroup arrowCanvas;

    void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;

        if (arrow != null)
        {
            arrowRect = arrow.GetComponent<RectTransform>();
            arrowCanvas = arrow.GetComponent<CanvasGroup>();

            if (arrowCanvas == null)
            {
                arrowCanvas = arrow.AddComponent<CanvasGroup>();
            }

            arrowOriginalPosition = arrowRect.anchoredPosition;
            arrowTargetPosition = arrowOriginalPosition;

            arrowCanvas.alpha = 0f;
            arrow.SetActive(false);
        }
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.unscaledDeltaTime * animationSpeed
        );

        if (arrow != null && arrow.activeSelf)
        {
            arrowRect.anchoredPosition = Vector2.Lerp(
                arrowRect.anchoredPosition,
                arrowTargetPosition,
                Time.unscaledDeltaTime * arrowSpeed
            );

            arrowCanvas.alpha = Mathf.Lerp(
                arrowCanvas.alpha,
                1f,
                Time.unscaledDeltaTime * arrowSpeed
            );
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;

        if (arrow != null)
        {
            arrow.SetActive(true);
            arrowRect.anchoredPosition = arrowOriginalPosition - new Vector2(arrowOffset, 0);
            arrowTargetPosition = arrowOriginalPosition;
            arrowCanvas.alpha = 0f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;

        if (arrow != null)
        {
            arrowTargetPosition = arrowOriginalPosition - new Vector2(arrowOffset, 0);
            arrowCanvas.alpha = 0f;
            arrow.SetActive(false);
        }
    }

    void OnDisable()
    {
        targetScale = originalScale;
        transform.localScale = originalScale;

        if (arrow != null)
        {
            arrow.SetActive(false);
        }
    }
}
