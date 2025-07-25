using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    public bool isHorizontal;
    public float maxWidth = 7f;
    public float cardSpacing = 2f;
    public Vector3 centerPoint;

    private List<Vector3> cardPositions = new();
    private List<Quaternion> cardRotations = new();

    private void CalculatePosition(int numberOfCards, bool isHorizontal)
    {
        cardPositions.Clear();
        cardRotations.Clear();
        if (isHorizontal)
        {
            float currentWidth = cardSpacing * (numberOfCards - 1);
            float totalWidth = Mathf.Min(currentWidth, maxWidth);

            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;

            for (int i = 0; i < numberOfCards; i++)
            {
                float xPosition = -totalWidth / 2 + i * currentSpacing;
                cardPositions.Add(new Vector3(xPosition, centerPoint.y, centerPoint.z));
                cardRotations.Add(Quaternion.identity);
            }
        }
    }

    public CardTransform GetCardTransforms(int index, int totalCards)
    {
       CalculatePosition(totalCards, isHorizontal);
       
       return new CardTransform(cardPositions[index], cardRotations[index]);
    }

}
