using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueExile.Classes.Items
{
    abstract class Equipment
    {
        protected Random random = new Random();
        public string Name;
        public int Power, Price;
        public bool IsBought, IsEquipped;

        public Equipment() { }
        public Equipment(string name, int power, int price)
        {
            Name = name;
            Power = power;
            Price = price;
            IsBought = true;
            IsEquipped = true;
        }
    }
    internal class Weapon : Equipment
    {
        private List<string> _weakWeaponNames = new List<string>
        {
            "Blunt Stick",
            "Rusty Dagger",
            "Wooden Mallet",
            "Bent Sword",
            "Fractured Axe",
            "Chipped Blade",
            "Dull Shiv",
            "Cracked Club",
            "Worn-out Cleaver",
            "Flimsy Lance"
        };
        private List<string> _averageWeaponNames = new List<string>
        {
            "Iron Broadsword",
            "Steel Warhammer",
            "Balanced Katana",
            "Polished Battleaxe",
            "Reinforced Maul",
            "Tempered Scimitar",
            "Reliable Halberd",
            "Sturdy Cutlass",
            "Weighted Flail",
            "Serrated Dagger"
        };
        private List<string> _strongWeaponNames = new List<string>
        {
            "Excalibur, God's Blade",
            "Stormbreaker the Blade",
            "Reapers Harvester",
            "Oblivion's Voidblade",
            "Seraph's Angelic Saber",
            "Bloodmoon, the Cleaver",
            "Dragonfang's Firebrand",
            "Leviathan of Neptune",
            "Celestial Star Spear",
            "Mjolnir, The Hammer"
        };
        public Weapon(int level)
        {
            IsBought = false;
            IsEquipped = false;
            if (level <= 5)
            {
                Name = _weakWeaponNames[random.Next(_weakWeaponNames.Count)];
                Power = random.Next(3, 7) * level;
                Price = random.Next(1, 20);
            }
            else if (level <= 10)
            {
                Name = _averageWeaponNames[random.Next(_averageWeaponNames.Count)];
                Power = random.Next(7, 15) * level;
                Price = random.Next(20, 50);
            }
            else if (level > 10)
            {
                Name = _strongWeaponNames[random.Next(_strongWeaponNames.Count)];
                Power = random.Next(15, 20) * level;
                Price = random.Next(50, 100);
            }
        }
        public Weapon(string name, int power, int price) : base(name, power, price) { }
    }
    internal class Armour : Equipment
    {
        private List<string> _weakArmourNames = new List<string>
        {
            "Tattered Rags",
            "Worn Leather Vest",
            "Frayed Cloth Robe",
            "Threadbare Tunic",
            "Patched Chainmail",
            "Rusty Iron Bracers",
            "Faded Linen Cowl",
            "Thin Padded Pants",
            "Cracked Leather Boots",
            "Dented Iron Helmet"
        };
        private List<string> _averageArmourNames = new List<string>
        {
            "Sturdy Plate Gauntlets",
            "Reliable Chain Coif",
            "Polished Steel Breastplate",
            "Leather Greaves",
            "Tempered Iron Pauldrons",
            "Balanced Hardened Helm",
            "Durable Mail Leggings",
            "Fortified Kite Shield",
            "Silver Vambraces"
        };
        private List<string> _strongArmourNames = new List<string>
        {
            "Titansteel Warplate",
            "Shadowweave Shroud",
            "Dragonbone Chestplate",
            "Seraphic Wingspan",
            "Infernal Demonplate",
            "Bloodforged Harness",
            "Celestial Embrace",
            "Stormguard Defender",
            "Radiant Soulmail",
            "Eternal Vanguard's Plate"
        };

        public Armour(int level)
        {
            IsBought = false;
            IsEquipped = false;
            if (level <= 5)
            {
                Name = _weakArmourNames[random.Next(_weakArmourNames.Count)];
                Power = random.Next(1, 3) * level;
                Price = random.Next(1, 10);
            }
            else if (level <= 10)
            {
                Name = _averageArmourNames[random.Next(_averageArmourNames.Count)];
                Power = random.Next(3, 7) * level;
                Price = random.Next(10, 25);
            }
            else if (level > 10)
            {
                Name = _strongArmourNames[random.Next(_strongArmourNames.Count)];
                Power = random.Next(7, 15) * level;
                Price = random.Next(25, 75);
            }
        }
        public Armour(string name, int power, int price) : base(name, power, price) { }
    }
}
