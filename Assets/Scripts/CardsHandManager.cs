using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

public class CardsHandManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private int countCardsMin;
    [SerializeField] private int countCardsMax;
    [Space(20)]
    [SerializeField] private float curve;
    [SerializeField] private float cardWidth;
    [SerializeField] private float cardHeight;
    [SerializeField] private float spacing;
    [SerializeField] private float offsetY; 
    [SerializeField] private float modificatorAngle = 0.1f;
    [Space(20)]
    [SerializeField] private float modificatorWidthSpace = 1.4f;
    [SerializeField] private float yPosSelected = -1.5f;
    [SerializeField] private float yPosModifying = 2.4f;
    [SerializeField] private Vector3 scaleSelected;
    [SerializeField] private Vector3 scaleModifying;

    private List<GameObject> cardsInHand;
    private int loadSpritesCount = 0;
    private int currentCardIndex;
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;

        CardDBManager.Init();
        CardsEvents.loadSpriteEnd += OnLoadSprite;
        CardsEvents.buttonClickMagic += UpdateCurrentCardValue;
        CardsEvents.updateCardPosition += UpdatePositionCardsInHand;
        CardsEvents.destroyCard += OnDestroyCard;
        CardsEvents.restartGame += OnRestart;
        CardsEvents.insertCardOnTable += InsertCardOnTable;
        GenerateCardsInHand(); 
    }

    private void OnDestroy()
    {
        CardsEvents.loadSpriteEnd -= OnLoadSprite;
        CardsEvents.buttonClickMagic -= UpdateCurrentCardValue;
        CardsEvents.updateCardPosition -= UpdatePositionCardsInHand;
        CardsEvents.destroyCard += OnDestroyCard;
        CardsEvents.restartGame += OnRestart;
        CardsEvents.insertCardOnTable += InsertCardOnTable;
    }


    private void OnRestart()
    {
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            Destroy(cardsInHand[i]);
        } 

        StopAllCoroutines();
        loadSpritesCount = 0; 
        GenerateCardsInHand();

    }

    private void OnLoadSprite()
    {
        loadSpritesCount++;
        if (loadSpritesCount == cardsInHand.Count)
        { 
             UpdatePositionCardsInHand();
        }
    }

    private void InsertCardOnTable(GameObject card)
    {
        cardsInHand.Remove(card);
        UpdatePositionCardsInHand();

        if (currentCardIndex >= cardsInHand.Count)
            currentCardIndex = (currentCardIndex >= cardsInHand.Count - 1) ? 0 : (currentCardIndex + 1);

        CardsEvents.generateCards?.Invoke(cardsInHand.Count);
    }

    void GenerateCardsInHand()
    {
        cardsInHand = new List<GameObject>();
        int countCards = UnityEngine.Random.Range(countCardsMin, countCardsMax);

        for (var i = 0; i < countCards; i++)
        {
            var card = Instantiate(cardPrefab, this.transform);
            cardsInHand.Add(card);
        }

        currentCardIndex = 0;
        CardsEvents.generateCards?.Invoke(cardsInHand.Count); 
        CardsEvents.lockedInput?.Invoke(false);
    }

    void UpdatePositionCardsInHand()
    { 
        float offsetX = (spacing * cardsInHand.Count / 2) - spacing / 2; 
        float angle = curve / cardsInHand.Count;
        float startAngle = -(curve / 2) + curve * modificatorAngle; 

        foreach (var card in cardsInHand)
        {
            float angleRotation = (startAngle + cardsInHand.IndexOf(card) * angle);
            float yDistance = Mathf.Abs(angleRotation) * cardHeight;
            float xPos = offsetX + cardWidth / 2;
            float yPos = offsetY - yDistance;
             
            Vector3 rotation = new Vector3(0, 0, -angleRotation);
            Vector3 scale = new Vector3(1, 1, 1);
            Vector3 position = new Vector3(xPos, yPos, -cardsInHand.IndexOf(card));

            switch (card.GetComponent<Card>().currentState)
            {
                case Card.StateCard.Simple:
                    card.GetComponent<CoroutineAnimations>().StopAllCoroutines();
                    card.GetComponent<CoroutineAnimations>().SetPositionCoroutine(position, EasingFunction.Ease.EaseInOutQuad, 0.5f);
                    card.GetComponent<CoroutineAnimations>().SetRotationCoroutine(rotation, EasingFunction.Ease.EaseInOutQuad, 0.5f, 0.3f);
                    card.GetComponent<CoroutineAnimations>().SetScaleCoroutine(scale, EasingFunction.Ease.EaseInOutQuad, 0.2f); 
                    offsetX += cardWidth + spacing;

                    break;

                case Card.StateCard.Selected:

                    rotation = Vector3.zero;
                    scale = scaleSelected;
                    position = new Vector3(xPos, yPosSelected, -cardsInHand.IndexOf(card));
                    card.GetComponent<CoroutineAnimations>().StopAllCoroutines();
                    card.GetComponent<CoroutineAnimations>().SetPositionCoroutine(position, EasingFunction.Ease.EaseInOutQuad, 0.5f);
                    card.GetComponent<CoroutineAnimations>().SetScaleCoroutine(scale, EasingFunction.Ease.EaseInOutBack, 0.5f);
                    card.GetComponent<CoroutineAnimations>().SetRotationCoroutine(rotation, EasingFunction.Ease.EaseInOutQuad, 0.2f); 
                    offsetX += cardWidth * modificatorWidthSpace + spacing;

                    break;

                case Card.StateCard.OnDrag: 
                    break;

                case Card.StateCard.Modifying:

                    rotation = Vector3.zero;
                    scale = scaleSelected;
                    position = new Vector3(0, yPosModifying, -cardsInHand.IndexOf(card));
                    card.GetComponent<CoroutineAnimations>().StopAllCoroutines();
                    card.GetComponent<CoroutineAnimations>().SetPositionCoroutine(position, EasingFunction.Ease.EaseInOutQuad, 0.5f);
                    card.GetComponent<CoroutineAnimations>().SetScaleCoroutine(scale, EasingFunction.Ease.EaseInOutBack, 0.5f);
                    card.GetComponent<CoroutineAnimations>().SetRotationCoroutine(rotation, EasingFunction.Ease.EaseInOutQuad, 0.2f);
 
                    offsetX += cardWidth * modificatorWidthSpace + spacing;

                    break;
            } 
        } 
    }

   
    private void UpdateCurrentCardValue()
    {
        cardsInHand[currentCardIndex].GetComponent<Card>().UpdateRandomValue();
        currentCardIndex = (currentCardIndex >= cardsInHand.Count - 1) ? 0 : (currentCardIndex + 1);
    }

    private void OnDestroyCard(GameObject card)
    {
        cardsInHand.Remove(card);
        StopAllCoroutines();
        Destroy(card);

        if (currentCardIndex >= cardsInHand.Count)
            currentCardIndex = (currentCardIndex >= cardsInHand.Count - 1) ? 0 : (currentCardIndex + 1);

        UpdatePositionCardsInHand();

        CardsEvents.generateCards?.Invoke(cardsInHand.Count);
    }
}
