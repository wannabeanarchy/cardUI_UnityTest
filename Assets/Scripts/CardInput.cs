using UnityEngine;
using UnityEngine.EventSystems;

public class CardInput : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,  IBeginDragHandler, IDragHandler, IEndDragHandler
{ 
    private Vector3 position;
    private float timeCount = 0.0f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<Card>().currentState = Card.StateCard.OnDrag;
        position = transform.position; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.dragging)
        { 
            timeCount += Time.deltaTime;
            if (timeCount > 0.25f)
            { 
                timeCount = 0.0f;
            }
        }

        eventData.pointerDrag.GetComponent<Card>().currentState = Card.StateCard.OnDrag;
        Vector3 pos = Camera.main.ScreenToWorldPoint( eventData.position );
        transform.position =  new Vector3(pos.x, pos.y, 30); 
        

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = position;

        eventData.pointerDrag.GetComponent<Card>().currentState = Card.StateCard.Simple;
        eventData.pointerDrag.GetComponent<Card>().OnHover(false); 
        CardsEvents.updateCardPosition?.Invoke(); 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging)
            return;

        if (eventData.pointerEnter && eventData.pointerEnter.GetComponent<Card>() != null)
        {
            eventData.pointerDrag.GetComponent<Card>().currentState = Card.StateCard.Selected;
            CardsEvents.updateCardPosition?.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging)
            return;

        if (eventData.pointerEnter.GetComponent<Card>() != null)
        {
            eventData.pointerEnter.GetComponent<Card>().OnHover(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging)
            return;

        if (eventData.pointerEnter.GetComponent<Card>() != null)
        {
            eventData.pointerEnter.GetComponent<Card>().OnHover(false);
            eventData.pointerEnter.GetComponent<Card>().currentState = Card.StateCard.Simple;
            CardsEvents.updateCardPosition?.Invoke();
        }
    }
}
