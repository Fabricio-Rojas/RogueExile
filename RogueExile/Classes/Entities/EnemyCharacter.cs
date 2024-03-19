using RogueExile.Classes.Items;
using RogueExile.Classes.MapGen;
using RogueExile.Enums;
using RogueExile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;

namespace RogueExile.Classes.Entities
{
    internal class EnemyCharacter : IRenderable, IMovable
    {
        /* Stats */
        private int _currentHealth;

        public string Name;
        public int Level, Exp, Gold, BaseStrength, BaseDefense, MaxHealth;
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
        private List<char> enemyIcons = new List<char>()
        {
            '%',
            '&',
            'æ',
            'ö',
            '¤',
            '§',
            'Σ',
            'Φ',
            'Ω',
            'δ',
            'φ',
            'ø',
            'α',
            '☼'
        };
        private List<string> enemyNames = new List<string>()
        {
            "Enemy"
        };
        private Random Random;
        public EnemyCharacter(int playerLevel, Cell spawnLocation, Cell[,] mapGrid)
        {
            Random = new Random();

            // use enemy types from nethack
            Icon = enemyIcons[Random.Next(enemyIcons.Count)];
            Name = enemyNames[Random.Next(enemyNames.Count)];
            Level = playerLevel > 1 ? playerLevel + Random.Next(-1, 2) : 1;
            ConsoleColor[] consoleColors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));
            Color = consoleColors[Random.Next(1, consoleColors.Length)];
            Gold = 1 + (Random.Next(6) * Level);
            BaseStrength = (int)((13 * Math.Pow(1.19, Level)) * (1 + (double)(Random.Next(-5, 6) / 100))); // 15 at level 1 & 80 at level 10
            BaseDefense = (int)((7.5 * Math.Pow(1.13, Level)) * (1 + (double)(Random.Next(-5, 6) / 100))); // 8 at level 1 & 26 at level 10
            MaxHealth = (int)((100 * Math.Pow(1.2, Level - 1)) * (1 + (double)(Random.Next(-20, 21) / 100)));
            CurrentHealth = MaxHealth;
            Exp = (int)((15 * (1 + (0.4 * (Level - 1)))) * (1 + (double)(Random.Next(-10, 1) / 100)));

            WeaponList = new List<Weapon>();
            ArmourList = new List<Armour>();
            ConsumableList = new List<Consumable>();

            EquippedWeapon = new Weapon("Ragged Claws", 0, 0);
            EquippedArmour = new Armour("Callous Skin", 0, 0);

            MapGrid = mapGrid;
            ValUnder = spawnLocation.Val;
            spawnLocation = spawnLocation.SetVal(Icon);
            CurrentLocation = spawnLocation;
            MapGrid[spawnLocation.X, spawnLocation.Y] = spawnLocation;

            GenerateLoot();
            Render();
        }
        public void PerformTurn()
        {

        }
        public void Move(Direction direction)
        {
            // check -/+ 5 (or more cells) around the character location (x and y)
            // this should be done by drawing a straight line from the origin to the area border
            // if the line is obstructed by a wall cell then eliminate the cells after the wall from the visible range
            // as such the entities line of sight will be limited by terrain
            // move should be called every turn, but should usually only move if player is visible to the entity
            // 33% chance to move 1 cell in a random direction even if player is not currently visible
            // if moving into the player, then perform an attack
        }
        private void Attack()
        {

        }
        private void GenerateLoot()
        {
            // generate an amount and rarity of loot based on the monster level
            // later add method for dropping a chest containing the loop upon death

            int lootAmount = (int)((Random.Next(0, 2) * Level) + 1);

            for (int i = 0; i < lootAmount; i++)
            {
                // randomly determine what type of loot to generate
                // 10% consumables, 45% each armour/weapons

                int chance = Random.Next(0, 100);

                if (chance < 10)
                {
                    Consumable consumable = new Consumable(Random.Next(0, 3));
                    ConsumableList.Add(consumable);
                }
                else if (chance < 55)
                {
                    Weapon weapon = new Weapon(Level);
                    WeaponList.Add(weapon);
                }
                else if (chance < 100)
                {
                    Armour armour = new Armour(Level);
                    ArmourList.Add(armour);
                }
            }
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
