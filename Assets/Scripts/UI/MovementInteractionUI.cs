using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 offset;

    [SerializeField] private bool restrictToCanvasBounds = true;
    [SerializeField] private Vector2 dragLimitMin = new Vector2(-100, -100);
    [SerializeField] private Vector2 dragLimitMax = new Vector2(100, 100);

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        
        if (rectTransform == null)
            Debug.LogError("DragUI requires RectTransform component");
        if (canvas == null)
            Debug.LogError("DragUI requires Canvas parent");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Calcular offset entre la posición del puntero y el centro del objeto
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPointerPosition
        );
        offset = (Vector2)rectTransform.localPosition - localPointerPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null || canvas == null) return;

        // Convertir posición screen → posición local canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPointerPosition
        );

        Vector2 newPosition = localPointerPosition + offset;

        // Aplicar restricción de límites si está habilitada
        if (restrictToCanvasBounds)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, dragLimitMin.x, dragLimitMax.x);
            newPosition.y = Mathf.Clamp(newPosition.y, dragLimitMin.y, dragLimitMax.y);
        }

        rectTransform.localPosition = new Vector3(newPosition.x, newPosition.y, rectTransform.localPosition.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Aquí puedes agregar lógica al soltar (ej: detectar zona, reproducir sonido, etc)
        Debug.Log("Drag finalizado en posición: " + rectTransform.localPosition);
    }
}
