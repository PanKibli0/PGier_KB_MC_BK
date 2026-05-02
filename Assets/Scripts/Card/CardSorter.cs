using System.Collections.Generic;
using System.Linq;

public static class CardSorter
{
    public enum SortMode { Order, Type, Cost, Alphabetical }

    public static List<Card> sortCards(List<Card> cards, SortMode mode, bool ascending)
    {
        var sorted = new List<Card>(cards);

        switch (mode)
        {
            case SortMode.Order:
                if (ascending)
                    return sorted;
                else
                {
                    sorted.Reverse();
                    return sorted;
                }

            case SortMode.Type:
                if (ascending)
                    sorted = sorted.OrderBy(c => c.data.type).ToList();
                else
                    sorted = sorted.OrderByDescending(c => c.data.type).ToList();
                break;

            case SortMode.Cost:
                if (ascending)
                    sorted = sorted.OrderBy(c => c.currentCost).ToList();
                else
                    sorted = sorted.OrderByDescending(c => c.currentCost).ToList();
                break;

            case SortMode.Alphabetical:
                if (ascending)
                    sorted = sorted.OrderBy(c => c.data.cardName).ToList();
                else
                    sorted = sorted.OrderByDescending(c => c.data.cardName).ToList();
                break;
        }
        return sorted;
    }
}