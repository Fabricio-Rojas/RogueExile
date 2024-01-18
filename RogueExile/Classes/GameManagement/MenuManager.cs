using RogueExile.Classes.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueExile.Classes.GameManagement
{
    internal class MenuManager
    {
        private PlayerCharacter _playerCharacter;
        private int _menuMiddleScreenHeight = (int)(Console.WindowHeight * 1.5);

        public MenuManager(PlayerCharacter player)
        {
            _playerCharacter = player;
        }
        public void DisplayUI()
        {
            Game.WriteCentered("Press ESC for Menu and Actions", Console.WindowHeight - 2);
            Game.WriteCentered
                ($"Name: {_playerCharacter.Name}, " +
                $"Level: {_playerCharacter.Level}, " +
                $"Health: {_playerCharacter.CurrentHealth}/{_playerCharacter.MaxHealth}, " +
                $"EXP: {_playerCharacter.Exp}/{_playerCharacter.LvlUpThreshold}, " +
                $"Gold: {_playerCharacter.Gold}", Console.WindowHeight - 1);
        }
        public void ShowMenu()
        {
            bool menuActive = true;

            while (menuActive)
            {
                ResetToMenuArea();
                Game.WriteCentered("1. Display Equipment", _menuMiddleScreenHeight - 1);
                Game.WriteCentered("2. Display Statistics", _menuMiddleScreenHeight);
                Game.WriteCentered("3. Exit Game", _menuMiddleScreenHeight + 1);
                Game.WriteCentered("(Press ESC to return)", _menuMiddleScreenHeight + 3);
                ConsoleKey key = Console.ReadKey(intercept: true).Key;
                menuActive = HandleMenuKeyPress(key);
            }
            // add a loop to await menu inputs, avoid recurrency
        }
        private bool HandleMenuKeyPress(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D1:
                    DisplayEquipment();
                    break;

                case ConsoleKey.D2:
                    DisplayStatistics();
                    break;

                case ConsoleKey.D3:
                    ConfirmExitRequest();
                    break;

                case ConsoleKey.Escape:
                    Console.SetCursorPosition(0, 0);
                    return false;

                default:
                    return true;
            }
            // same as Game.GameTurn() will only ever break out of the menu loop on the correct exit key, otherwise will just await
            // next keypress
            return true;
        }
        private void DisplayEquipment()
        {
            ResetToMenuArea();
        }
        private void DisplayStatistics()
        {
            ResetToMenuArea();
        }
        private void ConfirmExitRequest()
        {
            bool optionSelected = false;

            while (!optionSelected)
            {
                ResetToMenuArea();
                Game.WriteCentered("Are you sure you want to exit?", _menuMiddleScreenHeight - 1);
                Game.WriteCentered("Y   N", _menuMiddleScreenHeight);
                ConsoleKey key = Console.ReadKey(intercept: true).Key;

                switch (key)
                {
                    case ConsoleKey.Y:
                        // should place code that saves gamestate here (i.e. SaveManager.SaveGame() )
                        Environment.Exit(0);
                        break;

                    case ConsoleKey.N:
                        optionSelected = true;
                        break;

                    default:
                        break;
                }
            }

        }
        private void ResetToMenuArea()
        {
            // should move screen to menu area and clear screen
            Console.SetCursorPosition(0, Console.WindowHeight * 2);
            for (int i = Console.WindowHeight; i < Console.WindowHeight * 2; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine(new string(' ', Console.WindowWidth - 1));
            }
            Console.SetCursorPosition(0, Console.WindowHeight);
        }
    }
}
