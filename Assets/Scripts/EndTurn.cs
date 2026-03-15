using UnityEngine;

public class EndTurn : MonoBehaviour
{
    public HandSystem handSystem;
    public CardPileSystem cardPileSystem;

    public void endTurn()
    {
        if (handSystem) handSystem.discardAllCards();

        int drawCount = Random.Range(3, 6);
		//drawCount = 1;
        for (int i = 0; i < drawCount; i++)
            if (cardPileSystem) cardPileSystem.drawCard();
    }
}
