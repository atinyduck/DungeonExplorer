namespace DungeonExplorer;
/// <summary>
/// Class to hold player statistics.
/// </summary>
public class Statistics : ISaveable
{
    public int RoomsVisited { get; private set; }
    public int MonstersKilled { get; private set; }
    public int ItemsCollected { get; private set; }
    public int TotalDamageDealt { get; private set; }
    public int TotalDamageTaken { get; private set; }

    public DateTime StartTime { get; private set; } = DateTime.Now;

    public TimeSpan Duration => DateTime.Now - StartTime;

    /// <summary>
    /// Gets the save identifier.
    /// </summary>
    /// <returns>The identifier</returns>
    public string GetSaveIdentifier() => "player_stats";

    /// <summary>
    /// Updates the rooms visited.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="set">if set to <c>true</c> [set].</param>
    public void UpdateRoomsVisited(int amount = 1, bool set = false)
    {
        if (set) RoomsVisited = amount;
        else RoomsVisited += amount;
    }

    /// <summary>
    /// Updates the monsters killed.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="set">if set to <c>true</c> [set].</param>
    public void UpdateMonstersKilled(int amount = 1, bool set = false)
    {
        if (set) MonstersKilled = amount;
        else MonstersKilled += amount;
    }

    /// <summary>
    /// Updates the items collected.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="set">if set to <c>true</c> [set].</param>
    public void UpdateItemsCollected(int amount = 1, bool set = false)
    {
        if (set) ItemsCollected = amount;
        else ItemsCollected += amount;
    }

    /// <summary>
    /// Updates the total damage dealt.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="set">if set to <c>true</c> [set].</param>
    public void UpdateTotalDamageDealt(int amount, bool set = false)
    {
        if (set) TotalDamageDealt = amount;
        else TotalDamageDealt += amount;
    }

    /// <summary>
    /// Updates the total damage taken.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="set">if set to <c>true</c> [set].</param>
    public void UpdateTotalDamageTaken(int amount, bool set = false)
    {
        if (set) TotalDamageTaken = amount;
        else TotalDamageTaken += amount;
    }

    /// <summary>
    /// Updates the start time.
    /// </summary>
    /// <param name="time">The time.</param>
    public void UpdateStartTime(DateTime time) => StartTime = time;

    /// <summary>
    /// Resets this instance.
    /// </summary>
    public void Reset()
    {
        StartTime = DateTime.Now;
        RoomsVisited = 0;
        MonstersKilled = 0;
        ItemsCollected = 0;
        TotalDamageTaken = 0;
        TotalDamageDealt = 0;
    }

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"""
        === Statistics ===

        Time Played: {Duration}

        Rooms Visited: {RoomsVisited}
        Monsters Killed: {MonstersKilled}
        Items Collected: {ItemsCollected}
        Total Damage Dealt: {TotalDamageDealt}
        Total Damage Taken: {TotalDamageTaken}
        """;

}
