using RogueExile.Classes.Items;
using RogueExile.Classes.MapGen;
using RogueExile.Enums;
using RogueExile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueExile.Classes.Entities
{
    internal class PlayerCharacter : IRenderable, IMovable
    {
        /* Stats */
        private int _currentHealth, _exp;

        public string Name;
        public int Level, Gold, BaseStrength, BaseDefense, MaxHealth, LvlUpThreshold;
        public int Exp
        {
            get { return _exp; }
            set
            {
                if (value >= LvlUpThreshold)
                {
                    LevelUp();
                    _exp = 0;
                }
                _exp = value;
            }
        }
        public int CurrentHealth
        {
            get { return _currentHealth; }
            set
            {
                if (value > MaxHealth)
                {
                    _currentHealth = MaxHealth;
                }
                if (value < 0)
                {
                    _currentHealth = 0;
                }
                _currentHealth = value;
            }
        }

        /* Status Effects */
        public bool HasStrengthBuff;
        public bool HasAcidBuff;

        /* Equipped Items */
        public Weapon EquippedWeapon;
        public Armour EquippedArmour;

        /* Inventories */
        public List<Weapon> WeaponList;
        public List<Armour> ArmourList;
        public List<Consumable> ConsumableList;
        
        /* Map, Position & Movement */
        public char Icon;
        public ConsoleColor Color;
        private Cell CurrentLocation;
        private char ValUnder;
        private readonly Cell[,] MapGrid;

        /* Misc */
        private Random Random;
        public PlayerCharacter(string name, Cell spawnLocation, Cell[,] mapGrid)
        {
            Random = new Random();

            Icon = '@';
            Name = name;
            Level = 1;
            Color = ConsoleColor.Cyan;
            Gold = Random.Next(10 + 1);
            BaseStrength = (int)Math.Round(21 * Random.NextDouble()) + 5;
            BaseDefense = (int)Math.Round(11 * Random.NextDouble()) + 3;
            MaxHealth = 100;
            CurrentHealth = MaxHealth;
            LvlUpThreshold = 100;
            Exp = 0;

            HasStrengthBuff = false;
            HasAcidBuff = false;

            WeaponList = new List<Weapon>();
            ArmourList = new List<Armour>();
            ConsumableList = new List<Consumable>();

            EquippedWeapon = new Weapon("Bloodied Fist", 0, 0);
            EquippedArmour = new Armour("Tattered Rags", 0, 0);
            AddNewWeapon(EquippedWeapon);
            AddNewArmor(EquippedArmour);
            AddNewConsumable(new Consumable(0));

            MapGrid = mapGrid;
            ValUnder = spawnLocation.Val;
            spawnLocation = spawnLocation.SetVal(Icon);
            CurrentLocation = spawnLocation;
            MapGrid[spawnLocation.X, spawnLocation.Y] = spawnLocation;

            Render();
        }
        public void Move(Direction direction)
        {
            Cell NewLocation = direction switch
            {
                Direction.Up => CurrentLocation.MoveYBy(-1),
                Direction.Down => CurrentLocation.MoveYBy(1),
                Direction.Left => CurrentLocation.MoveXBy(-1),
                Direction.Right => CurrentLocation.MoveXBy(1),
                _ => CurrentLocation,
            };
            NewLocation = MapGrid[NewLocation.X, NewLocation.Y];

            switch (NewLocation.Val)
            {
                case '█':
                case '║':
                case '═':
                case ' ':
                case '╔':
                case '╗':
                case '╚':
                case '╝':
                    return;

                case '%':
                case '&':
                case 'æ':
                case 'ö':
                case '¤':
                case '§':
                case 'Σ':
                case 'Φ':
                case 'Ω':
                case 'δ':
                case 'φ':
                case 'ø':
                case 'α':
                case '☼':
                    Attack();
                    return;

                default:
                    break;
            }

            CurrentLocation = CurrentLocation.SetVal(ValUnder);
            MapGrid[CurrentLocation.X, CurrentLocation.Y] = CurrentLocation;
            Console.SetCursorPosition(CurrentLocation.X, CurrentLocation.Y);
            Console.ForegroundColor = CurrentLocation.Color;
            Console.Write(CurrentLocation.Val);
            Console.ResetColor();
            
            ValUnder = NewLocation.Val;
            CurrentLocation = NewLocation.SetVal(Icon);
            MapGrid[CurrentLocation.X, CurrentLocation.Y] = CurrentLocation;

            Render();
        }
        public void Attack()
        {

        }
        public void LevelUp()
        {
            Level++;
            BaseStrength = (int)(2 + BaseStrength * 1.15);
            BaseDefense = (int)(1 + BaseDefense * 1.10);
            int OldHealth = MaxHealth;
            MaxHealth = (int)(MaxHealth * 1.2);
            CurrentHealth += MaxHealth - OldHealth;
            LvlUpThreshold = Level < 15 ? (int)((3.5714 * Math.Pow(Level, 2)) + 96.4286) : (int)(42.7 + (1 - (1 / (((0.4 * (Level + 0.012)) + 1))) * 1000));
        }
        public bool AddNewWeapon(Weapon weapon)
        {
            if (WeaponList.Count + 1 > 9)
            {
                return false;
            }
            WeaponList.Add(weapon);
            return true;
        }
        public bool AddNewArmor(Armour armour)
        {
            if (ArmourList.Count + 1 > 9)
            {
                return false;
            }
            ArmourList.Add(armour);
            return true;
        }
        public bool AddNewConsumable(Consumable consumable)
        {
            if (ConsumableList.Count + 1 > 9)
            {
                return false;
            }
            ConsumableList.Add(consumable);
            return true;
        }
        public void EquipWeapon(Weapon weapon)
        {
            EquippedWeapon.IsEquipped = false;
            weapon.IsEquipped = true;
            EquippedWeapon = weapon;
        }
        public void EquipArmour(Armour armour)
        {
            EquippedArmour.IsEquipped = false;
            armour.IsEquipped = true;
            EquippedArmour = armour;
        }
        public void Render()
        {
            // should only be called on spawn and update

            Console.SetCursorPosition(CurrentLocation.X, CurrentLocation.Y);
            Console.ForegroundColor = Color;
            Console.Write(Icon);
            Console.ResetColor();
        }
    }
}
