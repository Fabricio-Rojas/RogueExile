using RogueExile.Classes.Entities;
using RogueExile.Classes.MapGen;
using RogueExile.Enums;
using RogueExile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RogueExile.Classes.GameManagement
{
    internal class Game
    {
        private MapGenerator _map;
        private PlayerCharacter _player;
        private MenuManager _menuManager;
        private readonly Random _random;

        public bool GameOver = false;

        public static int MaxWindowHeight = 46;
        public static int MaxWindowWidth = 170;
        public Game()
        {
            _random = new Random();
        }

        public void Start()
        {
            WriteCentered("Please press Alt + Enter, or F11 to enter fullscreen mode.\n", Console.WindowHeight / 2 - 1);
            WriteCentered("Also, make sure you're using Windows Console and not Windows Terminal.");

            while (Console.WindowHeight < MaxWindowHeight && Console.WindowWidth < MaxWindowWidth)
            {
                continue;
            }
            Console.Clear();

            string playerName = IntroSequence.Play();

            _map = new MapGenerator();
            _map.Generate(1);

            Cell spawnLocation = _map.rooms[_random.Next(0, _map.rooms.Count)].RoomCenter;
            _player = new PlayerCharacter(playerName, spawnLocation, _map.mapGrid);
            _player.Spawn();

            _menuManager = new MenuManager(_player);
            _menuManager.DisplayUI();

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
            bool turnCompleted = false;
            while (!turnCompleted)
            {
                ConsoleKey key = Console.ReadKey(intercept: true).Key;
                turnCompleted = PlayerTurnKeyPress(key);
            }
            Update();
            while (Console.KeyAvailable)
            {
                Console.ReadKey(intercept: true);
            }
        }
        public bool PlayerTurnKeyPress(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.NumPad8:
                case ConsoleKey.UpArrow:
                    _player.Move(Direction.Up);
                    break;

                case ConsoleKey.A:
                case ConsoleKey.NumPad4:
                case ConsoleKey.LeftArrow:
                    _player.Move(Direction.Left);
                    break;

                case ConsoleKey.S:
                case ConsoleKey.NumPad2:
                case ConsoleKey.DownArrow:
                    _player.Move(Direction.Down);
                    break;

                case ConsoleKey.D:
                case ConsoleKey.NumPad6:
                case ConsoleKey.RightArrow:
                    _player.Move(Direction.Right);
                    break;

                case ConsoleKey.Escape:
                    _menuManager.ShowMenu();
                    return false;

                default:
                    return false;
            }

            // opening the menu or pressing a wrong key will not skip player's turn, only movements will
            return true;
        }
        public void GenerateEnemies()
        {

        }
        public void Update()
        {
            //_enemyCharacter.Render();
            _player.Render();
            _menuManager.DisplayUI();
        }
        public static void WriteCentered(string text, int? topPosition = null)
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - text.Length / 2, topPosition ?? Console.CursorTop);
            Console.Write(text);
        }
        public static void WriteColumnCentered(List<string> lines, int topPosition)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                WriteCentered(lines[i], topPosition - (lines.Count / 2) + i);
            }
        }
    }
}
