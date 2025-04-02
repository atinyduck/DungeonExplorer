# Assessment 2 Plan Document

## Implement New Objects
- Creature :: Parent: Abstract class which will be the base of all entities.
  - Player :: Child: What the user will control.
  - Monster :: Chilkd: Enemies in the game, fought in combat.
- Item :: Parent: Abstract class which will be the base of all items in the game.
  - Weapon :: Child: The weapons, modifies attack statistics.
  - Armour :: Child: The armours, modifies defence and health statistics.
  - Potion :: Child: The potions, alters statistics temporarily.
- Inventory: Collection of 'Items'
- GameMap :: Collection of 'Rooms'

### Creature
#### Attributes
- Name :: string: The creatures name.
- Health :: int: The creatures health.
- MaxHealth :: int: The creatures max health.
- Inventory :: Invetory: Instance of inventory for the creatures items.
- Weapon :: Weapon: Instance of weapon for the creatures current weapon, modifies attack power.
- Armour :: Armour: Instance of armour for the creatures current armour, modifies defence.

#### Procedures
- Heal (int amount) : Heals the creature a set amount, cannot go above MaxHealth.
- TakeDamage (int amount) : Damages the creature a set amount, cannot go below 0.
- 
