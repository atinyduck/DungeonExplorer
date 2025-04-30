using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer;
public static class ItemFactory
{
    public static Item CreateFromSaveData(dynamic data)
    {
        try
        {
            string type = data.Type;
            return type switch
            {
                "Weapon" => new Weapon(
                    (string)data.Name,
                    (string)data.Description,
                    (int)data.DamageModifier),

                "Armour" => new Armour(
                    (string)data.Name,
                    (string)data.Description,
                    (int)data.DefenceModifier),

                "Potion" => new Potion(
                    (string)data.Name,
                    (string)data.Description,
                    Enum.Parse<PotionEffect>((string)data.EffectType),
                    (int)data.EffectDuration,
                    (int)data.EffectPower),

                "Item" => new Item(
                    (string)data.Name,
                    (string)data.Description),

                _ => throw new InvalidDataException($"Unknown item type: {type}")
            };
        }
        catch (Exception ex)
        {
            UI.Message($"Failed to create item: {ex.Message}");
            return null;
        }
    }
}