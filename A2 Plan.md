# Assessment 2 Plan Document

## Table of Contents
- [Brief](#brief)
- [Objects](#objects)
    - [Enums](#enum)
    - [Interfaces](#interface)
    - [Creature](#creature)
    - [Item](#item)
    - [Inventory](#inventory)
    - [GameMap](#gamemap)
- [Testing](#testing)

## Brief <a id="brief"></a>

This is Assessment 2 and is an individual assignment.
After a successful first stage interview where you demonstrated use of Git, code review and basic object-oriented principles, you have been asked to demonstrate further object-oriented principles in a second interview:

### Coding Task Guidelines:
Expand the “Dungeon Explorer” with advanced OO principles and features:

#### Add new classes:
- Monster: Represents creatures in rooms.
- Item: Represents multiple types of items like weapons or
potions.
- Inventory: A collection to manage items.
- GameMap: Manages multiple interconnected rooms.
#### Encapsulation and Abstraction
- Create hierarchies for:
    - Creature (abstract class): Player and Monster inherit from this class.
    - Item: Subclasses such as Weapon and Potion.
#### Interfaces
- Implement interfaces like IDamageable (applied to both Player and Monster) and ICollectible (applied to items).

#### LINQs and Lambda Expressions:
- Use LINQs to filter inventory items (e.g., all weapons) or find the strongest monster in a room.
- Use lambda expressions for sorting or filtering.
  
#### Static and Dynamic Polymorphism:
- Implement polymorphic methods:
    - Different monsters (e.g., Goblin vs. Dragon) have different attack behaviors.
    - Items (e.g., Potion vs. Weapon) have unique effects when used.
      
#### Error Checking:
- Enhance error checking for invalid commands or interactions (e.g., trying to use a non-existent item).
  
#### Game Expansion:
- The player can now:
    - Navigate through multiple rooms.
    - Battle monsters with varying difficulty.
    - Manage an inventory with multiple items.
    - 
### Summary
Read the Coding Task Guidelines and perform the following:
- Develop a working solution which showcases your knowledge of the C# language, and object-oriented principles.
- Implement a testing strategy for the solution.
- Create a 5-minute video in which you demonstrate your solution and in particular, its object-oriented features. TA penalty will be applied if you exceed the suggested video duration.
- Fill in the self-reflective assessment of the task using the report template supplied.

## Objects <a id="objects"></a>

### Enums <a id="enum"></a>
#### BaseSatistic
- Contains all base stats
  
```csharp
public enum BaseStatistic
{
    Defense,
    AttackPower,
    MaxHealth
}
```

#### PotionEffect
- Contains all potion types.
  
```csharp
public enum PotionEffect
{
    Heal,
    Poison,
    StrengthBuff,
    DefenseBuff,
    Invisibility
}
```

#### Directon
- Contains all directions that rooms can be connected.
  
```csharp
public enum Direction
{
    North,
    East,
    South,
    West
}
```
### Interfaces <a id="interface"></a>
#### IDamagable
Applied to 'Player' and 'Monster'.
- TakeDamage(int amount)

```csharp
public interface IDamageable
{
    void TakeDamage(int amount)
    int Health { get; }
    int MaxHealth { get; }
}
```

#### ICollectable
Applied to 'Item'.
- Use(Creature target)

```csharp
public interface ICollectable
{
    void Use(Creature target)
    string Name { get; }
    string Description { get; }
```

### Creature <a id="creature"></a>
#### Attributes
- **Name** :: string: The creature's name.
- **Health** :: int: The creature's health.
- **MaxHealth** :: int: The creature's max health.
- **Inventory** :: Invetory: Instance of inventory for the creatures items.
- **BaseDefense** :: int: The defualt defense.
- **BaseAttackPower** :: int: The base attack power.
- **Defense** :: int: The defense statistic, modified by 'EquippedArmour'.
- **AttackPower** :: int: The attack statistic, modified by 'EquippedWeapon'.
- **EquippedWeapon** :: Weapon: Instance of weapon for the creature's current weapon, modifies attack power.
- **EquippedArmour** :: Armour: Instance of armour for the creature's current armour, modifies defence.

```csharp
public abstract class Creature
{
    public string Name {get; private set;}
    public int Health {get; private set;}
    public int MaxHealth {get; private set;}
    public Inventory Inventory {get; private set;}
    public int BaseDefense {get; private set;}
    public int BaseAttackPower {get; private set;}
    public int Defense {get; private set;}
    public int AttackPower {get; private set;}
    public Weapon EquippedWeapon {get; private set;}
    public Armour EquippedArmour {get; private set;}

    protected Creature (string name, int max_health, int defense, int attackPower)
    {
        this.Name = name;
        this.Health = maxHealth;
        this.MaxHealth = maxHealth;
        this.Inventory = new Inventory();
        this.BaseDefense = defense;
        this.BaseAttackPower = attackPower;
        this.Defense = defense;
        this.BaseAttackPower = attackPower;
    }
```

#### Methods
- **Heal**(int amount): Heals the creature a set amount; cannot go above MaxHealth.
- **TakeDamage**(int amount): Damages the creature a set amount reduced by defense, which cannot go below 0.
- **Attack**(IDamagable target): Abstract, deals damage based on attack power.
- **EquipWeapon**(Weapon weapon): Equips a specified weapon.
- **EquipArmour**(Armour armour): Equips a specified armour.
- **ModifyBaseStat(Enum stat): Modifies a specified base stat.
- **RecalculateStats**(): Recalculate variable stats.

```csharp
    public abstract void Attack(IDamagable target);

    public void TakeDamage(int amount)
    {
        int actualDamage = Math.Max(0, amount - Defense)
        Health = Math.Max(0, Health - actualDamage)
        // Display Damage
    }

    public void Heal(int amount)
    {
        Health = Math.Min(MaxHealth, Health + amount);
        // Display Heal
    }

    public void EquipArmour(Armour armour)
    {
        EquippedArmour = armour;
        RecalculateStats() 
    }

    public void EquipWeapon(Weapon weapon)
    {
        EquippedWeapon = weapon;
        RecalculateStats() 
    }

    public void ModifyBaseStat(BaseStatistic stat, int newValue)
    {
        switch (stat)
        {
            case BaseStatistics.Defense:
                BaseDefense = newValue;
                break;
            case BaseStatistics.AttackPower:
                BaseAttackPower = newValue;
                break;
            case BaseStatistics.MaxHealth:
                MaxHealth = newValue;
                // If the max is reduced, ensure that health does not exceed it.
                Health = Math.Min(Health, MaxHealth) 
                break;
        }
        RecalculateStats();       
    }

    public void RecalculateStats()
    {
        AttackPower = BaseAttackPower + EquippedWeapon?.DamageModifier;
        Defense = BaseDefense + EquippedArmour?.DefenseModifier;
    }
```

#### Overrides
**ToString***(): Displays the basic information of the creature

```csharp
    public override ToString()
    {
        // Display Creature
    }
}
```


### Player :: Creature
#### Additional Attributes
- **Experience** :: int: The player's current experience, received from beating monsters
- **Level** :: int: The player's current level

```csharp
public class Player : Creature, IDamageable
{
    public int Experience {get; private set;}
    public int Level {get; private set;} = 1;

    public Player(string name) : base(name, maxHealth: 100, defense: 10, attack_power: 15)
    {
        Inventory = new Inventory();
    }

```

#### Additional Methods
- **GainExperience**(int amount): Add a specified amount to the player's experience.
- **LevelUp**(): Increases player's stats when experience goal is met.
- **GetRequiredXP**(): Returns the required experience for the level up.

```csharp
    public GainExperience(int amount)
    {
        Experience += amount;
        //Display increase

        while (Experience >= GetRequiredXP())
        {
            LevelUp();
        }
    }

    private LevelUp()
    {
        // Increase Stats
        Level++;
        ModfiyBaseStat(BaseStatistics.MaxHealth, MaxHealth + 10);
        ModfiyBaseStat(BaseStatistics.AttackPower, BaseAttackPower + 2);
        ModfiyBaseStat(BaseStatistics.Defense, BaseDefense + 1);
        Heal(MaxHealth);

        //Display level up
    }

    private GetRequiredXP() => Level * 100;
```

#### Overrides 
- **Attack**(IDamagable target): Override of attack method.
- **ToString**(): Override of ToString to display the player's stats.

```csharp
    public override ToString()
    {
        //Display Player Stats
    }

    public override Attack(IDamagable target)
    {
        int damage = AttackPower;
        target.TakeDamage(damage);
        //Display attack;
    }
```

### Monster :: Creature
#### Additional Attributes
- **RewardXP** :: int: The amount of experience granted on the monster's defeat.

```csharp
public class Monster : Creature, IDamageable
{
    public int RewardXP {get; private set;}

    protected Monster(string name, int maxHealth, int defense, int attackPower, int rewardXP)
        : base(name, maxHealth, defense, attackPower)
    {
        RewardXP = rewardXP;
    }
```

#### Additional Methods
- **GenerateDrops** :: int: The 'Items' granted on the monster's defeat.
- Plan to maybe add JSON with loot tables.
  
```csharp
    public virtual List<Item> GenerateDrops()
    {
        var drops = new List<Item>();

        if (Random.Next(0, 2) == 0)
        {
            // Add item here (Health potion)
        }
        return drops;
    }
```

#### Subclasses
- **Clockwork Mage**: Weak, Fast attacks.
    - Overrides Attack().

```csharp
public class ClockworkMage : Monster
{
    public ClockworkMage()
         : base("Clockwork Mage", maxHealth:~, defense:~, attackPower:~, rewardXP:~)
    {}

    public override void Attack(IDamageable target)
    {
        for (int i = 0; i < 2; i++)
        {
            int damage = AttackPower / 2;
            target.TakeDamage(damage);
            //Display attack
        }
    }
}
```

- **Rusting Construct**: High Health, Heavy attacks.
    - Overrides Attack() and GenerateDrops().

```csharp
public class RustingConstruct : Monster
{
    public RustingConstruct()
         : base("Rusting Construct", maxHealth:~, defense:~, attackPower:~, rewardXP:~)
    {}

    public override void Attack(IDamageable target)
    {
        int damage = AttackPower;
        target.TakeDamage(damage);
        //Display attack        
    }

    public override List<Item> GenereateDrops()
    {
        var drops = new List<Item>()

        if (Random.Next(0, 3) == 0)
        {
            //Add armour to the drops
        }

        return drops;
    }
}
```
  
- **Repair Unit**: Moderate Stats, Self-healing abilities.
    - Overrides Attack().
  
```csharp
public class RepairUnit : Monster
{
    public RepairUnit()
         : base("Clockwork Mage", maxHealth:~, defense:~, attackPower:~, rewardXP:~)
    {}

    public override void Attack(IDamageable target)
    {
        int damage = AttackPower;
        target.TakeDamage(damage);
        //Display attack   

        if (Random.Next(0, 4) == 0)
        {
            Heal(~);
            // Display self-heal
        }
    }
}
```


### Item <a id="item"></a>
#### Attributes
- **Name** :: string: The item's name.
- **Description** :: string: The item's description.

```csharp
public class Item: ICollectable
{
    public string Name {get; private set;}
    public string Description {get; private set;}

    public Item(string name, string description)
    {
        Name = name;
        Description = description;
    }
```

#### Methods
- **Use**(Creature target): Virtual, depends on the class.

```csharp
    public virtual void Use()
    {
        // Display description as if player was looking stating it has no use.
    }
```

### Weapon :: Item
#### Additional Attributes
- **DamageModifier** :: int: This will alter the user's damage.

```csharp
public class Weapon : Item
{
    public int DamageModifier {get; private set;}

    public Weapon (string name, string description, int damageModifier)
         : base (name, description)
    {
        DamageModifier = damageModifier;
    }
```

#### Overrides
- **Use**(): Equips the weapon.
- **ToString**(): Displays the weapon name and description.

```csharp
    public override ToString()
    {
        // Display the weapon name, description and stats.
    }

    public override Use(Creature target)
    {
        target.EquipWeapon(this)
        \\Display equipped weapon
    }
```

### Armour :: Item
#### Additional Attributes
- **DefenseModifier** :: int: This will alter the user's defense.

```csharp
public class Armour : Item
{
    public int DefenseModifier {get; private set;}

    public Weapon (string name, string description, int defenseModifier)
         : base (name, description)
    {
        DefenseModifier = defenseModifier;
    }
```
  
#### Overrides
- **Use**(): Equips the armour.
- **ToString**(): Returns the armour name and description.

```csharp
    public override ToString()
    {
        // Display the armour name, description and stats.
    }

    public override Use(Creature target)
    {
        target.EquipArmour(this)
        \\Display equipped weapon
    }
```

### Potion :: Item
#### Additional Attributes
- **EffectType** :: Enum: The effect of the potion.
- **EffectDuration** :: int: The time the effect lasts; -1 implies an instant effect.
- **EffectPower** :: int: The strength of the effect.

```csharp
public class Potion : Item
{
    public PotionEffect EffectType {get; private set}
    public int EffectDuration {get; private set}
    public int EffectPower {get: private set}

    public Weapon (string name, string description, Enum type, int duration, int power)
         : base (name, description)
    {
        EffectType = type;
        EffectDuration = duration;
        EffectPower = power; // -1 Refers to instant effects, e.g. Heal.
    }
```

#### Additional Methods
- **ApplyPoison**(Creature target): Apply posion to a creature.
- **ApplyBuff**(Creature target, BaseStatistic stat): Apply a buff to a specific creature's stats.

```csharp
    public void ApplyPoison(Creature target)
    {
        if (EffectDuration > 0)
        {
            \\Poison for multiple turns
            \\Used if status effects are implemented
        }
        else
        {
            target.TakeDamage(EffectPower)
            \\Display poison info
        }
    }

    public void ApplyBuff(Creature target, BaseStatistics stat)
    {
        if (EffectDuration > 0)
        {
            \\Temprory stat increase
        }
        else \\Permanent buff
        {
            int amount = EffectPower
            switch(stat)
                case BaseStatistics.AttackPower:
                    amount += target.BaseAttackPower
                    break;

                case BaseStatistics.Defense:
                    amount += target.BaseDefense
                    break;

                case BaseStatistics.MaxHealth:
                    amount += target.MaxHealth
                    break;

            ModifyBaseStat(stat, amount) 
        }
    }
```

#### Overrides
- **Use**(): Applies the effect of the potion.

```csharp
    public override Use(Creature target)
    {
        switch (EffectType)
        {
            case PotionEffect.Heal:
                target.Heal(EffectPower)
                \\Display Heal
                break;

            case PotionEffect.Posion:
                ApplyPosion(target)
                break;

            case PotionEffect.StrengthBuff:
                ApplyBuff(target, BaseStatistic.AttackPower)
                break;

            case PotionEffect.DefenseBuff:
                ApplyBuff(target, BaseStatistic.Defense)
                break;

            case PotionEffect.Invisibility:
                \\ Possible future implementation stopping being attacked
                break;
        }
    }
```

### Inventory <a id="inventory"></a>
#### Attributes
- **_items** :: List<Item>: Private variable to store all the items in the inventory.
- **Items** :: IReadOnlyList<Item>: Public readonly list of _items.
- **Count** :: The amount of items in inventory.
- **BestWeapon** :: Weapon: The best weapon in the inventory.
- **BestArmour** :: Armour: The best armour in the inventory.

```csharp
public class Inventory
{
    private List<ICollectable> _items = new List<ICollectable>();

    public IReadOnlyList<ICollectable> Items => _items.AsReadOnly();

    public int Count => _items.Count;

    public Weapon BestWeapon => FindBestWeapon();

    public Armour BestArmour => FindBestArmour();
```

#### Methods
- **HasItem**(Item item): Checks if the inventory has the item.
- **AddItem**(Item item): Add a new item to the inventory.
- **RemoveItem**(Item item): Remove an item from the inventory.
- **Clear**(): Clears the inventory.
- **ListWeapons**(): Returns a list of all 'Weapons' in the inventory.
- **FindBestWeapon**(): Returns the best 'Weapon'.
- **ListArmour**(): Returns a list of all 'Armour' in the inventory.
- **FindBestArmour**(): Returns the best 'Armour'.

```csharp
    public bool HasItem(ICollectable item) => Contents.Contains(item);

    public void AddItem(ICollectable item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        _items.Add(item);
    }

    public void RemoveItem(ICollectable item) => _items.Remove(item);

    public void Clear() => _items.Clear();

    public List<Weapon> ListWeapons() => _items.OfType<Weapon>().ToList();
    
    private Weapon FindBestWeapon() =>
        ListWeapon()
            .OrderByDescending(w => w.DamageModifier)
            .FirstOrDefault(); 
    

    public List<Armour> ListArmour() => _items.OfType<Armour>().ToList();
    
    private Armour FindBestArmour() =>
        ListWeapon()
            .OrderByDescending(a => a.DefenseModifier)
            .FirstOrDefault(); 
```

#### Overrides
- **ToString**(): Displays the contents of the inventory.

```csharp
    public override string ToString()
    {
        // Display Inventory
    }
}
```

### GameMap <a id="gamemap"></a>
#### Attributes
- **_rooms** :: List<Room>: Private list of all rooms in the game.
- **Rooms** :: IReadOnlyList<Room>: A readlonly list of all rooms in the game.
- **CurrentRoom** :: Room: The current room in focus.

```csharp
public class GameMap
{
    private List<Room> _rooms = new List<Room>();
    public IReadOnlyList<Room> Rooms => _rooms.AsReadOnly();
    public Room CurrentRoom {get; private set;}

    public GameMap(Room startingRoom)
    {
        CurrentRoom = startingRoom
        AddRoom(startingRoom);
    }

```

#### Methods
- **AddRoom**(Room room): Adds a room to the private _rooms;
- **MovePlayer**(Direction direction): Moves the player a specified direction through the map.

```
    public void AddRoom(Room room)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));
        _rooms.Add(room)
    }

    public bool MovePlayer(Direction direction)
    {
        var neighbour = CurrentRoom.GetNeighbour(direction);
        if (neighbour != null)
        {
            CurrentRoom = neighbour;
            if (!_rooms.Contains(CurrentRoom))
            {
                _rooms.Add(CurrentRoom);
            }
            return true;
        }
        return false;
    }

```

## Testing 

### Unit Tests
#### Creature
- Test TakeDamage() with various defense values
- Test Heal() doesn't exceed MaxHealth
- Test stat recalculation when equipping items

#### Player
- Test experience gain and level up
- Verify stat increases on level up

#### Monster
- Test attack behviours for each monister type
- Test drop generation probabilities

#### Item
- Test weapon damamge modification
- Test armour defense modification
- Test all potion effects

#### Inventory
- Test item add/removal
- Test best weapon/armour selection
- Test LINQ queries

#### GameMap 
- Test room connections
- Test movement between rooms
- Test room content management

### Intergration Tests
1. Test full combat sequence
2. Test inventory management during gameplay
3. Test map navigation and discovery

## Video

*Jake Morgan* *29160569*
