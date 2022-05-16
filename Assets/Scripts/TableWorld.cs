using UnityEngine;

public class TableWorld : MonoBehaviour
{
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private float xOffset;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private Vector3 scale;

    private void Start()
    {
        CardsEvents.insertCardOnTable += InsertCardOnTable;
        CardsEvents.restartGame += OnRestart;
    }

    private void OnDestroy()
    { 
        CardsEvents.insertCardOnTable -= InsertCardOnTable;
        CardsEvents.restartGame += OnRestart;
    }

    private void OnRestart()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        } 

    }

    public void InsertCardOnTable(GameObject card)
    {
        card.transform.parent = this.transform;
        int childCount = this.transform.childCount;
        Vector3 targetPosition = new Vector3(positionOffset.x + xOffset * childCount, positionOffset.y, positionOffset.z);
        card.GetComponent<CoroutineAnimations>().StopAllCoroutines();
        card.GetComponent<CoroutineAnimations>().SetPositionCoroutine(targetPosition, EasingFunction.Ease.EaseInOutQuad, 0.5f, 0);
        card.GetComponent<CoroutineAnimations>().SetRotationCoroutine(rotation, EasingFunction.Ease.EaseInOutQuad, 0.5f, 0);
        card.GetComponent<CoroutineAnimations>().SetScaleCoroutine(scale, EasingFunction.Ease.EaseInOutQuad, 0.5f, 0);

        card.GetComponent<CardInput>().enabled = false; 
    }
     
}
