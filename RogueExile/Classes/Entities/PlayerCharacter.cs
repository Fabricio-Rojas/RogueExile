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
        private int _currentHealth, _exp;

        public string Name;
        public int Level, Gold, BaseStrength, BaseDefense, MaxHealth, LvlUpThreshold;
        public char Icon;
        public ConsoleColor Color;
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
        public Weapon EquippedWeapon;
        public Armour EquippedArmour;
        public List<Weapon> WeaponList;
        public List<Armour> ArmourList;
        public List<Consumable> ConsumableList;

        private Cell CurrentLocation;
        private readonly Cell[,] MapGrid;
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
            LvlUpThreshold = 100;
            Exp = 0;

            WeaponList = new List<Weapon>();
            ArmourList = new List<Armour>();
            ConsumableList = new List<Consumable>();

            EquippedWeapon = new Weapon("Bloodied Fist", 0, 0);
            EquippedArmour = new Armour("Tattered Rags", 0, 0);
            AddNewWeapon(EquippedWeapon);
            AddNewArmor(EquippedArmour);
            AddNewConsumable(new Consumable(0));

            CurrentLocation = spawnLocation;
            MapGrid = mapGrid;
        }
        public void Spawn()
        {
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

            Cell CellUnder;

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
                    CellUnder = CurrentLocation;
                    CurrentLocation = NewLocation;
                    break;
            }

            if (CellUnder != null)
            {
                Console.SetCursorPosition(CellUnder.X, CellUnder.Y);
                Console.ForegroundColor = CellUnder.Color;
                Console.Write(CellUnder.Val);
                Console.ResetColor();
            }

            Render();
        }
        public void LevelUp()
        {
            Level++;
            BaseStrength = (int)(2 + BaseStrength * 1.15);
            BaseDefense = (int)(1 + BaseDefense * 1.10);
            int OldHealth = MaxHealth;
            MaxHealth = (int)(MaxHealth * 1.2);
            CurrentHealth += MaxHealth - OldHealth;
            LvlUpThreshold = (int)((1 - 1 / (0.112 * Level + 1)) * 1000);
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
            Console.SetCursorPosition(CurrentLocation.X, CurrentLocation.Y);
            Console.ForegroundColor = Color;
            Console.Write(Icon);
            Console.ResetColor();
        }
    }
}
