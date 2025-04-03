# Assessment 2 Plan Document

## Objects
### Creature
#### Attributes
- Name :: string: The creatures name.
- Health :: int: The creatures health.
- MaxHealth :: int: The creatures max health.
- Inventory :: Invetory: Instance of inventory for the creatures items.
- Defense :: int: The defense statistic, modified by 'EquippedArmour'.
- AttackPower :: int: The attack statistic, modified by 'EquippedWeapon'.
- EquippedWeapon :: Weapon: Instance of weapon for the creatures current weapon, modifies attack power.
- EquippedArmour :: Armour: Instance of armour for the creatures current armour, modifies defence.

#### Methods
- Heal(int amount) : Heals the creature a set amount, cannot go above MaxHealth.
- TakeDamage(int amount) : Damages the creature a set amount reduced by defense, cannot go below 0.
- Attack(IDamagable target) : Deals damaged based on attack power.
- EquipWeapon(Weapon weapon) : Equips a specified weapon.
- EquipArmour(Armour armour) : Equips a specified armour.

### Player :: Creature
#### Additional Attributes
- Experience :: int: The players current experience, recieved from beating monsters
- Level :: int: The players current level

### Monster :: Creature
#### Subclasses
- Clockwork Mage: Weak, Fast attacks.
- Rusting Construct: High Health, Slow attacks.
- Repair Unit: Moderate Stats, Resistant to some attacks.

### Item
#### Attributes
- Name :: string: The items name.
- Description :: string: The items description.

#### Methods
- Use(Creature target) : Abstract, depends on the class.

### Weapon :: Item
#### Additional Attributes
- DamageModifier :: int: This will alter the user's damage.

#### Overrides
- Use() : Equips the weapon.

### Armour :: Item
#### Additional Attributes
- DefenseModifier :: int: This will alter the user's defense.
  
#### Overrides
- Use() : Equips the armour.

### Potion :: Item
#### Additional Attributes
- EffectType :: Enum: The effect of the potion.
- EffectDuration :: int: The time the effect lasts for.

#### Overrides
- Use() : Applies the effect of the potion.

### Invetory
#### Methods
- AddItem(Item item) : Add a new item to the inventory.
- RemoveItem(Item item) : Remove an item from the inventory.
- ListWeapons() : Returns a list of all 'Weapons' in the inventory.
- ListArmour() : Returns a list of all 'Armour' in the inventory.
- FindBestWeapon() : Returns the strongest 'Weapon' in the inventory.
- FindBestArmour() : Returns the strongest 'Armour' in the inventory.

### GameMap
#### Attributes
- Rooms :: List<Room>: A list of all rooms in the game.
- CurrentRoom :: Room: The current room in focus.

#### Methods
- MovePlayer(string direction) : Moves the player a specifed direction through the map.

#### Overrides
- ToString() : Returns the room name, description and contents.

## Interfaces
### IDamagable
Applied to 'Player' and 'Monster'.
- TakeDamage(int amount)

### ICollectible
Applied to 'Item'.
- Use(Creature target)

