using System;
using System.Collections.Generic;

public class RelicManager
{
    private List<RelicData> relics;

    public event Action onRelicsChanged;

    public RelicManager(List<RelicData> initialRelics)
    {
        relics = new List<RelicData>(initialRelics);
        onRelicsChanged?.Invoke();
    }

    public void addRelic(RelicData relic)
    {
        relics.Add(relic);
        onRelicsChanged?.Invoke();
    }

    public List<RelicData> getRelics()
    {
        return relics;
    }

    public void onBattleStart(Unit source)
    {
        for (int i = 0; i < relics.Count; i++)
        {
            RelicData relic = relics[i];
            if (relic.trigger != RelicTrigger.OnBattleStart)
                continue;

            executeActions(relic.actions, source);
        }
    }

    public void onBattleEnd(Unit source)
    {
        for (int i = 0; i < relics.Count; i++)
        {
            RelicData relic = relics[i];
            if (relic.trigger != RelicTrigger.OnBattleEnd)
                continue;

            executeActions(relic.actions, source);
        }
    }

    public void onTurnStart(Unit source, int turnNumber)
    {
        for (int i = 0; i < relics.Count; i++)
        {
            RelicData relic = relics[i];
            if (relic.trigger != RelicTrigger.OnTurnStart)
                continue;

            if (relic.turnsBetweenTriggers > 0 && turnNumber % relic.turnsBetweenTriggers != 0)
                continue;

            executeActions(relic.actions, source);
        }
    }

    public void onTurnEnd(Unit source, int turnNumber)
    {
        for (int i = 0; i < relics.Count; i++)
        {
            RelicData relic = relics[i];
            if (relic.trigger != RelicTrigger.OnTurnEnd)
                continue;

            if (relic.turnsBetweenTriggers > 0 && turnNumber % relic.turnsBetweenTriggers != 0)
                continue;

            executeActions(relic.actions, source);
        }
    }

    public void onCardPlayed(Unit source, Card card)
    {
        for (int i = 0; i < relics.Count; i++)
        {
            RelicData relic = relics[i];
            if (relic.trigger != RelicTrigger.OnCardPlayed)
                continue;

            if (!relic.anyCardType && card.data.type != relic.requiredCardType)
                continue;


            executeActions(relic.actions, source);
        }
    }

    public void onDamageDealt(Unit source, Unit target)
    {
        for (int i = 0; i < relics.Count; i++)
        {
            RelicData relic = relics[i];
            if (relic.trigger != RelicTrigger.OnDamageDealt)
                continue;

            executeActions(relic.actions, source, target);
        }
    }

    public void onDamageTaken(Unit target, Unit source)
    {
        for (int i = 0; i < relics.Count; i++)
        {
            RelicData relic = relics[i];
            if (relic.trigger != RelicTrigger.OnDamageTaken)
                continue;

            executeActions(relic.actions, target, source);
        }
    }

    private void executeActions(List<BaseAction> actions, Unit source, Unit selectedTarget = null)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            BaseAction action = actions[i];
            List<Unit> targets = TargetingSystem.getTargets(source, action.targetType, selectedTarget);
            for (int j = 0; j < targets.Count; j++)
            {
                Unit t = targets[j];
                if (t != null)
                    action.execute(t, source);
            }
        }
    }
}