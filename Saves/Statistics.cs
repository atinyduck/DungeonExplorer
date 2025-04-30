namespace DungeonExplorer;
public class Statistics
{
    public int RoomsVisited { get; private set; }
    public int MonstersKilled { get; private set; }
    public int ItemsCollected { get; private set; }
    public int TotalDamageDealt { get; private set; }
    public int TotalDamageTaken { get; private set; }

    public DateTime StartTime { get; private set; } = DateTime.Now;

    public TimeSpan Duration => DateTime.Now - StartTime;

    #region Save Implementation

    public string GetSaveIdentifier = "player_stats";

    public static string GetSavePath(string saveFolder) =>
        Path.Combine(saveFolder, "statistics.json");

    public object ToSaveData() => new
    {
        RoomsVisited,
        MonstersKilled,
        ItemsCollected,
        TotalDamageDealt,
        TotalDamageTaken,
        StartTime = StartTime.ToString("o") // ISO 8601 format
    };

    public void FromSaveData(object data)
    {
        dynamic json = data;
        RoomsVisited = json.RoomsVisited;
        MonstersKilled = json.MonstersKilled;
        ItemsCollected = json.ItemsCollected;
        TotalDamageDealt = json.TotalDamageDealt;
        TotalDamageTaken = json.TotalDamageTaken;
        StartTime = DateTime.Parse(json.StartTime);
    }

    #endregion

    public void UpdateRoomsVisited(int amount = 1, bool set = false)
    {
        if (set) RoomsVisited = amount;
        else RoomsVisited += amount;
    }
    public void UpdateMonstersKilled(int amount = 1, bool set = false)
    {
        if (set) MonstersKilled = amount;
        else MonstersKilled += amount;
    }
    public void UpdateItemsCollected(int amount = 1, bool set = false)
    {
        if (set) ItemsCollected = amount;
        else ItemsCollected += amount;
    }
    public void UpdateTotalDamageDealt(int amount, bool set = false)
    {
        if (set) TotalDamageDealt = amount;
        else TotalDamageDealt += amount;
    }
    public void UpdateTotalDamageTaken(int amount, bool set = false)
    {
        if (set) TotalDamageTaken = amount;
        else TotalDamageTaken += amount;
    }

    public void UpdateStartTime(DateTime time) => StartTime = time;
    
    public void Reset()
    {
        StartTime = DateTime.Now;
        RoomsVisited = 0;
        MonstersKilled = 0;
        ItemsCollected = 0;
        TotalDamageTaken = 0;
        TotalDamageDealt = 0;
    }

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
