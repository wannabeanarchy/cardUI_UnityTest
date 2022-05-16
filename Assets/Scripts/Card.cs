using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Card : MonoBehaviour
{
    public enum TypeData
    {
        Attack,
        Health,
        Mana
    }
    public enum StateCard
    {
        Simple,
        Modifying,
        Selected,
        OnDrag,
        OnTable
    }


    [SerializeField] private SpriteRenderer spriteImage;
    [SerializeField] private GameObject selectedMark;
    [SerializeField] private TextMeshPro valueAttack;
    [SerializeField] private TextMeshPro valueHealth;
    [SerializeField] private TextMeshPro valueMana;
    [SerializeField] private TextMeshPro textHeader;
    [SerializeField] private TextMeshPro textDescription;
    [SerializeField] private Vector3 scaleVectorUpdate;

    [SerializeField] string imageUrl = "https://picsum.photos/200/300";

    private Dictionary<TypeData, int> cardStats;
 
    public StateCard currentState = StateCard.Simple;

    void Start()
    {
        StartCoroutine(GetTexture());
        InitCardData();
    }

    private void InitCardData()
    { 
        CardDB cardDb = CardDBManager.GetRandomCardData();
        valueAttack.text = cardDb.attack.ToString();
        valueHealth.text = cardDb.health.ToString();
        valueMana.text = cardDb.mana.ToString();
        textHeader.text = cardDb.name.ToString();
        textDescription.text = cardDb.descriptions.ToString();

        cardStats = new Dictionary<TypeData, int>();
        cardStats.Add(TypeData.Attack, cardDb.attack);
        cardStats.Add(TypeData.Health, cardDb.health);
        cardStats.Add(TypeData.Mana, cardDb.mana);

    }

    public IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            CardsEvents.loadSpriteEnd?.Invoke();
        }
        else
        {
            Texture2D loadTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite = Sprite.Create(loadTexture,  new Rect(0, 0, loadTexture.width, loadTexture.height), Vector2.one / 2);
            spriteImage.sprite = sprite; 
            CardsEvents.loadSpriteEnd?.Invoke();
        }

        www.Dispose(); 
        System.GC.Collect();
    }

    public void UpdateRandomValue()
    {
        int indexValue = Random.Range(0, cardStats.Count);
        int newRandomValue = Random.Range(-2, 9);
        TypeData typeToChange = cardStats.ElementAt(indexValue).Key;

        currentState = StateCard.Modifying;
        CardsEvents.updateCardPosition?.Invoke(); 

        StartCoroutine(UpdateValue(typeToChange, newRandomValue));
    }

    public IEnumerator UpdateValue(TypeData typeToChange, int newValue)
    {
        yield return new WaitForSeconds(0.5f); 
        int currentValue = cardStats[typeToChange];

        switch (typeToChange)
        {
            case TypeData.Attack:
                valueAttack.transform.parent.gameObject.GetComponent<CoroutineAnimations>().SetScaleCoroutine(scaleVectorUpdate, EasingFunction.Ease.EaseInElastic, 0.3f, 0);
                yield return new WaitForSeconds(0.1f);
                valueAttack.transform.parent.gameObject.GetComponent<CoroutineAnimations>().SetCountText(valueAttack, newValue, currentValue, 20, 0.3f);
                cardStats[typeToChange] = newValue;
                yield return new WaitForSeconds(0.3f);
                valueAttack.transform.parent.gameObject.GetComponent<CoroutineAnimations>().SetScaleCoroutine( new Vector3(1.0f, 1.0f, 1.0f), EasingFunction.Ease.EaseInElastic, 0.3f, 0);

                break;
            case TypeData.Health:
                valueHealth.transform.parent.gameObject.GetComponent<CoroutineAnimations>().SetScaleCoroutine(scaleVectorUpdate, EasingFunction.Ease.EaseInElastic, 0.3f, 0);
                yield return new WaitForSeconds(0.1f);
                valueHealth.transform.parent.gameObject.GetComponent<CoroutineAnimations>().SetCountText(valueHealth, newValue, currentValue, 20, 0.3f);
                yield return new WaitForSeconds(0.3f);
                valueHealth.transform.parent.gameObject.GetComponent<CoroutineAnimations>().SetScaleCoroutine(new Vector3(1.0f, 1.0f, 1.0f), EasingFunction.Ease.EaseInElastic, 0.3f, 0);

                break;
            case TypeData.Mana:
                valueMana.transform.parent.gameObject.GetComponent<CoroutineAnimations>().SetScaleCoroutine(scaleVectorUpdate, EasingFunction.Ease.EaseInElastic, 0.3f, 0);
                yield return new WaitForSeconds(0.1f);
                valueMana.transform.parent.gameObject.GetComponent<CoroutineAnimations>().SetCountText(valueMana, newValue, currentValue, 20, 0.3f);
                yield return new WaitForSeconds(0.3f);
                valueMana.transform.parent.gameObject.GetComponent<CoroutineAnimations>().SetScaleCoroutine(new Vector3(1.0f, 1.0f, 1.0f), EasingFunction.Ease.EaseInElastic, 0.3f, 0);

                break;
        }
        cardStats[typeToChange] = newValue;

        yield return new WaitForSeconds(0.5f);
 
        if (newValue < 0)
        {
            GetComponent<CoroutineAnimations>().StopAllCoroutines();
            GetComponent<CoroutineAnimations>().SetRotationCoroutine(new Vector3(0, 180, 0), EasingFunction.Ease.EaseInOutCirc, 0.3f, 0);
            GetComponent<CoroutineAnimations>().SetPositionCoroutine(new Vector3(this.transform.position.x, this.transform.position.y - 10, this.transform.position.z), EasingFunction.Ease.EaseInOutCirc, 0.3f, 0);

            yield return new WaitForSeconds(0.4f);
            CardsEvents.destroyCard?.Invoke(this.gameObject); 
        }
        else
        {
            yield return new WaitForSeconds(0.3f);

            currentState = StateCard.Simple;
            CardsEvents.updateCardPosition?.Invoke();
        }


        CardsEvents.lockedInput?.Invoke(false);
        yield return null;
    }
     
    public void OnHover(bool isHover)
    { 
        selectedMark.SetActive(isHover);
    }
}
