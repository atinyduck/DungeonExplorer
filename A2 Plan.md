# Assessment 2 Plan Document

## Table of Contents
- [Objects](#objects)
  - [Creature](#creature)
  - [Item](#item)
  - [Inventory](#inventory)
  - [GameMap](#gamemap)
- [Interfaces](#interfaces)

## Objects <a id="objects"></a>

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
        Defense = BaseDefense + armour.DefenseModifier;
    }

    public void EquipWeapon(Weapon weapon)
    {
        EquippedWeapon = weapon;
        AttackPower = BaseAttackPower + weapon.DamageModifier 
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
public class Player : Creature
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
        MaxHealth += 10;
        Health = MaxHealth;
        BaseAttackPower += 2;
        BaseDefense += 1;
        // Reapply bonuses
        EquipWeapon(EquippedWeapon);
        EquipArmour(EquippedArmour);
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
public class Monster : Creature
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
    - Overrides Attack() and GenerateDrops().

```csharp
public class ClockworkMage: Monster
{
    public ClockworkMage()
         : base("Clockwork Mage", maxHealth:~, defense:~, attackPower:~, rewardXP:~)
    {}

    public override void Attack(IDamageable target)
    {
        for (int i = 0; i < 2; i++)
        {
            target.TakeDamage(AttackPower / 2);
            //Display attack
        }
    }
}
```

- **Rusting Construct**: High Health, Heavy attacks.

```csharp
public class RustingConstruct: Monster
{
    public RustingConstruct()
         : base("Rusting Construct", maxHealth:~, defense:~, attackPower:~, rewardXP:~)
    {}

    public override void Attack(IDamageable target)
    {
        target.TakeDamage(AttackPower);
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

```csharp
public class ClockworkMage: Monster
{
    public ClockworkMage()
         : base("Clockwork Mage", maxHealth:~, defense:~, attackPower:~, rewardXP:~)
    {}

    public override void Attack(IDamageable target)
    {
        target.TakeDamage(AttackPower);
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
public class Item
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
        target.EquipWeapon(...)
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
        target.EquipArmour(...)
    }
```

### Potion :: Item
#### Additional Attributes
- **EffectType** :: Enum: The effect of the potion.
- **EffectDuration** :: int: The time the effect lasts; -1 implies an instant effect.

```csharp
public class Potion : Item
{
    public Enum EffectType {get; private set}
    public int EffectDuration {get; private set}

    public Weapon (string name, string description, Enum EffectType, int effectDuration)
         : base (name, description)
    {
        EffectType = effectType;
        EffectDuration = effectDuration
    }
```

#### Overrides
- **Use**(): Applies the effect of the potion.

```csharp
    public override Use(Creature target)
    {
        target.Heal(...) \\ Heal 
    }
```

### Inventory <a id="inventory"></a>
#### Methods
- **AddItem**(Item item): Add a new item to the inventory.
- **RemoveItem**(Item item): Remove an item from the inventory.
- **ListWeapons**(): Returns a list of all 'Weapons' in the inventory.
- **ListArmour**(): Returns a list of all 'Armour' in the inventory.
- **FindBestWeapon**(): Returns the strongest 'Weapon' in the inventory.
- **FindBestArmour**(): Returns the strongest 'Armour' in the inventory.

### GameMap <a id="gamemap"></a>
#### Attributes
- **Rooms** :: List<Room>: A list of all rooms in the game.
- **CurrentRoom** :: Room: The current room in focus.

#### Methods
- **MovePlayer**(Room neighbour): Moves the player a specified direction through the map.
- **GetNeigbours**(): Returns the neighbours to the current room, used to move between.

## Interfaces <a id="interfaces"></a>
### IDamagable
Applied to 'Player' and 'Monster'.
- TakeDamage(int amount)

### ICollectible
Applied to 'Item'.
- Use(Creature target)


## Testing 

*Jake Morgan* *29160569*
