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
        private readonly Random _random;

        public bool GameOver = false;
        public Game()
        {
            _random = new Random();
        }
        
        public void Start()
        {
            Console.WriteLine("Please press Alt + Enter, or F11 to enter fullscreen mode.");
            while (Console.WindowHeight < Console.LargestWindowHeight && Console.WindowWidth <= Console.LargestWindowWidth)
            {
                continue;
            }
            Console.Clear();

            _map = new MapGenerator();
            _map.Render();

            Cell spawnLocation = _map.rooms[_random.Next(0, _map.rooms.Count)].RoomCenter;
            _character = new PlayerCharacter(spawnLocation, _map.mapGrid);
            _character.Render();

            GenerateEnemies(); // generate enemies within rooms

            do
            {
                GameTurn();
            }
            while (!GameOver);
        }
        private void Restart()
        {

        }
        private void NextStage()
        {

        }
        public void GameTurn()
        {
            ConsoleKey key = Console.ReadKey(intercept: true).Key;
            PlayerTurnKeyPress(key);
            Render();
            while (Console.KeyAvailable)
            {
                Console.ReadKey(intercept: true);
            }
        }
        public void PlayerTurnKeyPress(ConsoleKey key)
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
        public void GenerateEnemies()
        {

        }
        public void Render()
        {
            //_enemyCharacter.Render();
            _character.Render();
        }
    }
}
