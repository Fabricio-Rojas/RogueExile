using RogueExile.Enums;
using RogueExile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RogueExile.Classes
{
    internal class Game : IRenderable
    {
        private MapGenerator _map;
        private PlayerCharacter _character;
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
            WriteCentered("Please press Alt + Enter, or F11 to enter fullscreen mode.\n", (Console.WindowHeight / 2) - 1);
            WriteCentered("Also, make sure you're using Windows Console and not Windows Terminal.");

            while (Console.WindowHeight < MaxWindowHeight && Console.WindowWidth < MaxWindowWidth)
            {
                continue;
            }
            Console.Clear();

            string playerName = GameIntro();
            IntroAnimation();

            _map = new MapGenerator();
            _map.Render();

            Cell spawnLocation = _map.rooms[_random.Next(0, _map.rooms.Count)].RoomCenter;
            _character = new PlayerCharacter(playerName, spawnLocation, _map.mapGrid);
            _character.Render();

            GenerateEnemies(); // generate enemies within rooms

            do
            {
                GameTurn();
            }
            while (!GameOver);
        }
        private string GameIntro()
        {
            bool enterPressedPreviously = false;
            string playerName = "";

            while (true)
            {
                Console.Clear();
                if (enterPressedPreviously)
                {
                    WriteCentered("Please tell me your name, Exile: \n", (Console.WindowHeight / 2) - 2);
                }
                else
                {
                    WriteCentered("What is your name, Exile?: \n", (Console.WindowHeight / 2) - 2);
                }
                enterPressedPreviously = false;

                WriteCentered("\n");
                WriteCentered(playerName);
                WriteCentered("\n\n");
                WriteCentered("Press enter to continue.");

                int nameLengthPosition = (Console.WindowWidth / 2) + (playerName.Length / 2);
                nameLengthPosition += playerName.Length % 2 != 0 ? 1 : 0;
                Console.SetCursorPosition(nameLengthPosition, Console.WindowHeight / 2);

                ConsoleKeyInfo key = Console.ReadKey(false);

                if (char.IsLetter(key.KeyChar))
                {
                    playerName += key.KeyChar;
                }
                else if (key.Key == ConsoleKey.Backspace && playerName.Length > 0)
                {
                    playerName = playerName.Remove(playerName.Length - 1);
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    enterPressedPreviously = true;
                    if (playerName.Length > 0) break;
                }
            }
            
            Console.Clear();
            return playerName;
        }
        public void IntroAnimation()
        {
            Classes.IntroAnimation.Play();
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
        public void ShowMenu()
        {
            Console.SetCursorPosition(0, MapGenerator.mapH * 2);
            Console.SetCursorPosition(0, MapGenerator.mapH);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(intercept: true);
            Console.SetCursorPosition(0, 0);
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

                case ConsoleKey.Tab:
                    ShowMenu();
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
        public static void WriteCentered(string text, int? topPosition = null)
        {
            Console.SetCursorPosition((Console.WindowWidth / 2) - (text.Length / 2), (topPosition ?? Console.CursorTop));
            Console.Write(text);
        }
    }
}
