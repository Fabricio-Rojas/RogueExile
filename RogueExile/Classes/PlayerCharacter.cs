using RogueExile.Enums;
using RogueExile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueExile.Classes
{
    internal class PlayerCharacter : IRenderable, IMovable
    {
        private int _currentHealth, _exp, _lvlUpThreshold;

        public string Name;
        public int Level, Gold, BaseStrength, BaseDefense, MaxHealth;
        public char Icon;
        public ConsoleColor Color;
        public int Exp
        {
            get { return _exp; }
            set
            {
                if (value >= _lvlUpThreshold)
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
        public Weapon EquippedWeapon;
        public Armour EquippedArmour;
        public List<Weapon> WeaponList;
        public List<Armour> ArmourList;
        public List<Consumable> ConsumableList;

        private Cell CurrentLocation;
        private Cell NewLocation;
        private readonly Cell[,] MapGrid;
        private Cell NewCell;
        private Cell CellUnder;
        private Random Random;
        public PlayerCharacter(string name, Cell spawnLocation, Cell[,] mapGrid)
        {
            Random = new Random();

            Icon = '@';
            Name = name;
            Level = 1;
            Color = ConsoleColor.Cyan;
            Gold = Random.Next(10 + 1);
            BaseStrength = (int)Math.Round(20 * Random.NextDouble()) + 5;
            BaseDefense = (int)Math.Round(10 * Random.NextDouble()) + 3;
            MaxHealth = 100;
            CurrentHealth = MaxHealth;

            WeaponList = new List<Weapon>();
            ArmourList = new List<Armour>();
            ConsumableList = new List<Consumable>();

            EquippedWeapon = new Weapon("Bloodied Fist", 0);
            EquippedArmour = new Armour("Tattered Rags", 0);
            AddNewWeapon(EquippedWeapon);
            AddNewArmor(EquippedArmour);
            AddNewConsumable(new Consumable(0));

            CurrentLocation = spawnLocation.SetVal(Icon).SetColor(Color);
            NewLocation = mapGrid[CurrentLocation.X, CurrentLocation.Y];
            MapGrid = mapGrid;
        }
        public void Move(Direction direction)
        {
            Cell NextLocation = direction switch
            {
                Direction.Up => CurrentLocation.MoveYBy(-1),
                Direction.Down => CurrentLocation.MoveYBy(1),
                Direction.Left => CurrentLocation.MoveXBy(-1),
                Direction.Right => CurrentLocation.MoveXBy(1),
                _ => CurrentLocation,
            };
            NewLocation = MapGrid[NextLocation.X, NextLocation.Y];
        }
        public void LevelUp()
        {
            Level++;
            BaseStrength = (int)(2 + BaseStrength * 1.15);
            BaseDefense = (int)(1 + BaseDefense * 1.10);
            int OldHealth = MaxHealth;
            MaxHealth = (int)(MaxHealth * 1.2);
            CurrentHealth += MaxHealth - OldHealth;
            _lvlUpThreshold = (int)((1 - (1 / ((0.112 * Level) + 1))) * 1000);
        }
        public void AddNewWeapon(Weapon weapon)
        {
            if (WeaponList.Count + 1 > 9)
            {
                Console.WriteLine("You can only hold up to 9 weapons at once");
                Console.ReadKey(intercept: true);
                return;
            }
            WeaponList.Add(weapon);
        }
        public void AddNewArmor(Armour armour)
        {
            if (ArmourList.Count + 1 > 9)
            {
                Console.WriteLine("You can only hold up to 9 armours at once");
                Console.ReadKey(intercept: true);
                return;
            }
            ArmourList.Add(armour);
        }
        public void AddNewConsumable(Consumable consumable)
        {
            if (ConsumableList.Count + 1 > 9)
            {
                Console.WriteLine("You can only hold up to 9 consumables at once");
                Console.ReadKey(intercept: true);
                return;
            }
            ConsumableList.Add(consumable);
        }
        public void Render()
        {
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

                default:
                    CellUnder = NewCell;
                    NewCell = NewLocation;
                    break;
            }

            if (CellUnder != null)
            {
                Console.SetCursorPosition(CurrentLocation.X, CurrentLocation.Y);
                Console.ForegroundColor = CellUnder.Color;
                Console.Write(CellUnder.Val);
            }

            CurrentLocation = (NewLocation.X > 0 && NewLocation.Y > 0) ? NewLocation : CurrentLocation;

            Console.SetCursorPosition(CurrentLocation.X, CurrentLocation.Y);
            Console.ForegroundColor = Color;
            Console.Write(Icon);
            Console.ResetColor();
        }
    }
}
