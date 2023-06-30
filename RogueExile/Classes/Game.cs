using RogueExile.Enums;
using RogueExile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueExile.Classes
{
    internal class Game : IRenderable
    {
        private MapGenerator _map;
        private PlayerCharacter _character;
        private EnemyCharacter _enemyCharacter;

        public bool GameOver = false;
        public Game()
        {
            _map = new MapGenerator();
            _map.Render();
            _character = new PlayerCharacter(new Cell(MapGenerator.mapW / 2, MapGenerator.mapH / 2, '@'), _map.mapGrid);
            _enemyCharacter = new EnemyCharacter();
        }
        public void Start()
        {
            Console.WriteLine(Console.LargestWindowWidth);
            Console.WriteLine("Please press Alt + Enter, or F11 to enter fullscreen mode.");
            while (Console.WindowHeight < Console.LargestWindowHeight && Console.WindowWidth <= Console.LargestWindowWidth)
            {
                continue;
            }
            Console.Clear();
            Render();

            do
            {
                ConsoleKey key = Console.ReadKey(intercept: true).Key;
                OnKeyPress(key);
                Render();
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(intercept: true);
                }
            }
            while (!GameOver);
        }
        public void OnKeyPress(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.NumPad8:
                case ConsoleKey.UpArrow:
                    _character.Move(Direction.Up);
                    break;

                case ConsoleKey.A:
                case ConsoleKey.NumPad4:
                case ConsoleKey.LeftArrow:
                    _character.Move(Direction.Left);
                    break;

                case ConsoleKey.S:
                case ConsoleKey.NumPad2:
                case ConsoleKey.DownArrow:
                    _character.Move(Direction.Down);
                    break;

                case ConsoleKey.D:
                case ConsoleKey.NumPad6:
                case ConsoleKey.RightArrow:
                    _character.Move(Direction.Right);
                    break;

                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;

                default:
                    break;
            }
        }
        public void Render()
        {
            _character.Render();
            _enemyCharacter.Render();
        }
    }
}
