using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void OnHandle(Vector2 position);


    public event OnHandle onBeginDrag;
    public event OnHandle onDrag;
    public event OnHandle onEndDrag;



    public void OnBeginDrag(PointerEventData eventData)
    {
        onBeginDrag?.Invoke(eventData.position);
    }


    public void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke(eventData.position);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        onEndDrag?.Invoke(eventData.position);
    }


}
