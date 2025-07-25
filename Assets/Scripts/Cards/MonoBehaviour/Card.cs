using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("组件")] 
    public SpriteRenderer cardSprite;
    public TextMeshPro costText, descriptionText, typeText;

    public CardDataSO cardData;

    private void Start()
    {
        Init(cardData);
    }

    public void Init(CardDataSO data)
    {
        cardData = data;
        cardSprite.sprite = data.cardImage;
        costText.text = data.cost.ToString();
        descriptionText.text = data.description;
        typeText.text = data.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "防御",
            CardType.Abilities => "技能",
            _ => throw new System.NotImplementedException("未知的卡牌类型")
        };

    }
}
