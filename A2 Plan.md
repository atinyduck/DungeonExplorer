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

#### Methods
- **Heal**(int amount): Heals the creature a set amount; cannot go above MaxHealth.
- **TakeDamage**(int amount): Damages the creature a set amount reduced by defense, which cannot go below 0.
- **Attack**(IDamagable target): Abstract, deals damage based on attack power.
- **EquipWeapon**(Weapon weapon): Equips a specified weapon.
- **EquipArmour**(Armour armour): Equips a specified armour.

```csharp
public abstract class Creature
{
  string Name {get; private set;}
  int Health {get; private set;}
  int MaxHealth {get; private set;}
  Inventory Inventory {get; private set;}
  int BaseDefense {get; private set;}
  int BaseAttackPower {get; private set;}
  int Defense {get; private set;}
  int AttackPower {get; private set;}
  Weapon EquippedWeapon {get; private set;}
  Armour EquippedArmour {get; private set;}

  public Creature (string name, int max_health, int defense, int attackPower)
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

  public abstract void Attack(IDamagable target);

  public void TakeDamage(int amount)
  {
    int actualDamage = Math.Max(0, amount - this.Defense)
    this.Health = Math.Max(0, this.Health - actualDamage)
    // Display Damage
  }

  public void Heal(int amount)
  {
    this.Health = Math.Min(this.MaxHealth, this.Health + amount);
    // Display Heal
  }

  public void EquipArmour(Armour armour)
  {
    this.EquippedArmour = armour;
    this.Defense = this.BaseDefense + armour.DefenseModifier;
  }
  
  public void EquipWeapon(Weapon weapon)
  {
    this.EquippedWeapon = weapon;
    this.AttackPower = this.BaseAttackPower + weapon.DamageModifier 
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
}
```

### Monster :: Creature
#### Subclasses
- **Clockwork Mage**: Weak, Fast attacks.
- **Rusting Construct**: High Health, Slow attacks.
- **Repair Unit**: Moderate Stats, Self-healing abilities.

```csharp
public class Monster : Creature
{
}
```

### Item <a id="item"></a>
#### Attributes
- **Name** :: string: The item's name.
- **Description** :: string: The item's description.

#### Methods
- **Use**(Creature target): Abstract, depends on the class.

```csharp
public class Item
{
}
```

### Weapon :: Item
#### Additional Attributes
- **DamageModifier** :: int: This will alter the user's damage.

#### Overrides
- **Use**(): Equips the weapon.
- **ToString**(): Returns the weapon name and description.

### Armour :: Item
#### Additional Attributes
- **DefenseModifier** :: int: This will alter the user's defense.
  
#### Overrides
- **Use**(): Equips the armour.
- **ToString**(): Returns the armour name and description.

### Potion :: Item
#### Additional Attributes
- **EffectType** :: Enum: The effect of the potion.
- **EffectDuration** :: int: The time the effect lasts.

#### Overrides
- **Use**(): Applies the effect of the potion.

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
