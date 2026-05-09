using System.Collections.Generic;

public class MoveState
{
    public UnitMove lastUsedMove;
    public UnitMove forcedNextMove;
    public int currentTurn;
    private Dictionary<UnitMove, int> useCounts = new Dictionary<UnitMove, int>();
    private Dictionary<MoveCondition, int> cooldowns = new Dictionary<MoveCondition, int>();

    public bool canUse(UnitMove move)
    {
        if (forcedNextMove != null && move != forcedNextMove)
            return false;

        if (move.condition != null && !move.condition.canUse(this, move))
            return false;

        return true;
    }

    public void recordUse(UnitMove move)
    {
        lastUsedMove = move;

        if (!useCounts.ContainsKey(move))
            useCounts[move] = 0;
        useCounts[move]++;

        if (move.condition != null)
            move.condition.onUse(this, move);

        if (move.condition is ForceNextMoveCondition force)
            forcedNextMove = force.nextMove;
        else
            forcedNextMove = null;
    }

    public void onTurnEnd(List<UnitMove> allMoves)
    {
        foreach (UnitMove move in allMoves)
        {
            if (move.condition != null)
                move.condition.onTurnEnd(this);
        }
    }

    public int getUseCount(UnitMove move)
    {
        if (useCounts.ContainsKey(move))
            return useCounts[move];
        return 0;
    }

    public int getCooldown(MoveCondition condition)
    {
        if (cooldowns.ContainsKey(condition))
            return cooldowns[condition];
        return 0;
    }

    public void setCooldown(MoveCondition condition, int value)
    {
        cooldowns[condition] = value;
    }

    public void decrementCooldown(MoveCondition condition)
    {
        if (cooldowns.ContainsKey(condition) && cooldowns[condition] > 0)
            cooldowns[condition]--;
    }
}