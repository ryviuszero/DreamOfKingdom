using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;

    private List<CardDataSO> drawDeck = new(); //抽牌堆
    private List<CardDataSO> discardPile = new(); //弃牌堆

    private List<Card> handCardObjectList = new(); //当前手牌

    //测试
    private void Start()
    {
        InitializeDeck();
    }

    public void InitializeDeck()
    {
        drawDeck.Clear();
        foreach (var entry in cardManager.currentCardLibrary.cardLibraryList)
        {
            for (int j = 0; j < entry.amount; j++)
            {
                drawDeck.Add(entry.cardData);
            }
        }

        // TODO: 洗牌/更新抽牌堆 or 弃牌堆显示的数字
    }

    [ContextMenu("测试抽牌")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }

    private void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawDeck.Count == 0)
            {
                // TODO: 洗牌/更新抽牌堆 or 弃牌堆显示的数字
            }
            CardDataSO currentCardData = drawDeck[0];
            drawDeck.RemoveAt(0);

            var card = cardManager.GetCardObject().GetComponent<Card>();
            // 初始化卡牌对象
            card.Init(currentCardData);
            handCardObjectList.Add(card);
        }
    }





}
