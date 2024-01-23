using RogueExile.Classes.Entities;
using RogueExile.Classes.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
                    DisplayEquipmentMenu();
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
        private void DisplayEquipmentMenu()
        {
            bool correctKeyPressed = false;

            while (!correctKeyPressed)
            {
                ResetToMenuArea();

                List<string> lines = new List<string>()
                {
                    "Showing Equipment",
                    "",
                    $"Equipped Weapon: {_playerCharacter.EquippedWeapon.Name}, {_playerCharacter.EquippedWeapon.Power} Power, {_playerCharacter.EquippedWeapon.Price} Gold",
                    $"Equipped Armour: {_playerCharacter.EquippedArmour.Name}, {_playerCharacter.EquippedArmour.Power} Power, {_playerCharacter.EquippedArmour.Price} Gold",
                    $"",
                    $"1. Change Equipped Weapon",
                    $"2. Change Equipped Armour",
                    $"3. Use Consumable",
                    $"4. Show Full Inventory",
                    "",
                    "(Press ESC to return)"
                };

                Game.WriteColumnCentered(lines, _menuMiddleScreenHeight);

                ConsoleKey key = Console.ReadKey(intercept: true).Key;
                correctKeyPressed = EquipmentMenuKeyPress(key);
            }
        }
        private bool EquipmentMenuKeyPress(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D1:
                    EquipWeaponMenu();
                    break;

                case ConsoleKey.D2:
                    EquipArmourMenu();
                    break;

                case ConsoleKey.D3:
                    UseConsumableMenu();
                    break;

                case ConsoleKey.D4:
                    DisplayInventory();
                    break;

                case ConsoleKey.Escape:
                    return true;

                default:
                    return false;
            }
            return false;
        }
        private void EquipWeaponMenu()
        {
            List<Weapon> weaponList = _playerCharacter.WeaponList;
            bool recentlyEquipped = false;
            string weaponName = "";

            while (true)
            {
                ResetToMenuArea();

                List<string> lines = new List<string>()
                {
                    "Choose Weapon to equip",
                    ""
                };
                for (int i = 0; i < weaponList.Count; i++)
                {
                    lines.Add($"{i + 1}. {weaponList[i].Name}, {weaponList[i].Power} power, {weaponList[i].Price} Gold  {(weaponList[i].IsEquipped ? "(Equipped)" : "")}");
                }
                lines.Add("");
                lines.Add("(Press esc to return)");

                if (recentlyEquipped)
                {
                    lines.Add("");
                    lines.Add($"(Equipped {weaponName})");
                }

                Game.WriteColumnCentered(lines, _menuMiddleScreenHeight);

                recentlyEquipped = false;

                ConsoleKeyInfo key = Console.ReadKey(intercept: true);
                if (int.TryParse(key.KeyChar.ToString(), out int result) && result > 0 && result <= weaponList.Count)
                {
                    _playerCharacter.EquipWeapon(weaponList[result - 1]);
                    weaponName = weaponList[result - 1].Name;
                    recentlyEquipped = true;
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
        private void EquipArmourMenu()
        {
            List<Armour> armourList = _playerCharacter.ArmourList;
            bool recentlyEquipped = false;
            string armourName = "";

            while (true)
            {
                ResetToMenuArea();

                List<string> lines = new List<string>()
                {
                    "Choose Armour to equip",
                    ""
                };
                for (int i = 0; i < armourList.Count; i++)
                {
                    lines.Add($"{i + 1}. {armourList[i].Name}, {armourList[i].Power} power, {armourList[i].Price} Gold  {(armourList[i].IsEquipped ? "(Equipped)" : "")}");
                }
                lines.Add("");
                lines.Add("(Press esc to return)");

                if (recentlyEquipped)
                {
                    lines.Add("");
                    lines.Add($"(Equipped {armourName})");
                }

                Game.WriteColumnCentered(lines, _menuMiddleScreenHeight);

                recentlyEquipped = false;

                ConsoleKeyInfo key = Console.ReadKey(intercept: true);
                if (int.TryParse(key.KeyChar.ToString(), out int result) && result > 0 && result <= armourList.Count)
                {
                    _playerCharacter.EquipArmour(armourList[result - 1]);
                    armourName = armourList[result - 1].Name;
                    recentlyEquipped = true;
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
        private void UseConsumableMenu()
        {
            List<Consumable> consumableList = _playerCharacter.ConsumableList;
            bool recentlyConsumed = false;
            string? consumedMessage = "";

            while (true)
            {
                ResetToMenuArea();

                List<string> lines = new List<string>()
                {
                    "Choose consumable to use",
                    ""
                };
                for (int i = 0; i < consumableList.Count; i++)
                {
                    lines.Add($"{i + 1}. {consumableList[i].Name}, {consumableList[i].Price} Gold");
                }
                lines.Add("");
                lines.Add("(Press esc to return)");

                if (recentlyConsumed && !string.IsNullOrEmpty(consumedMessage))
                {
                    lines.Add("");
                    lines.Add($"{consumedMessage}");
                }

                Game.WriteColumnCentered(lines, _menuMiddleScreenHeight);

                recentlyConsumed = false;

                ConsoleKeyInfo key = Console.ReadKey(intercept: true);
                if (int.TryParse(key.KeyChar.ToString(), out int result) && result > 0 && result <= consumableList.Count)
                {
                    Game.WriteCentered("");
                    Game.WriteCentered("Are you sure you want to use this consumable?");
                    Game.WriteCentered("Y   N");

                    while (true)
                    {
                        ConsoleKey answer = Console.ReadKey(intercept: true).Key;
                        switch (answer)
                        {
                            case ConsoleKey.Y:
                            case ConsoleKey.Enter:
                                consumedMessage = consumableList[result - 1].BeConsumed(_playerCharacter);
                                recentlyConsumed = true;
                                break;

                            case ConsoleKey.N:
                            case ConsoleKey.Escape:
                                break;

                            default:
                                continue;
                        }
                    }
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
        private void DisplayInventory()
        {
            List<Weapon> weaponList = _playerCharacter.WeaponList;
            List<Armour> armourList = _playerCharacter.ArmourList;
            List<Consumable> consumableList = _playerCharacter.ConsumableList;

            int lineLength = 150;
            int secondColumnPosition = 57;
            int thirdColumnPosition = 115;

            while (true)
            {
                ResetToMenuArea();
                List<string> lines = new List<string>()
                {
                    "Showing full inventory",
                    ""
                };

                int biggestLength = Math.Max(Math.Max(weaponList.Count, armourList.Count), consumableList.Count);
                if (biggestLength <= 0)
                {
                    lines.Add("(No items to show)");
                    ConsoleKey press = Console.ReadKey(intercept: true).Key;
                    if (press == ConsoleKey.Escape)
                    {
                        break;
                    }
                }

                string header = new string(' ', lineLength);
                header = ReplaceSubstring(header, "WEAPONS", 15);
                header = ReplaceSubstring(header, "ARMOUR", secondColumnPosition + 15);
                header = ReplaceSubstring(header, "CONSUMABLES", thirdColumnPosition + 8);
                lines.Add(header);
                lines.Add("");

                for (int i = 0; i < biggestLength; i++)
                {
                    string line = new string(' ', lineLength);

                    if (i < weaponList.Count)
                    {
                        line = ReplaceSubstring(line, $"{i + 1}. {weaponList[i].Name}, {weaponList[i].Power} power, {weaponList[i].Price} Gold  {(weaponList[i].IsEquipped ? "(Equipped)" : "")}", 0);
                    }
                    if (i < armourList.Count)
                    {
                        line = ReplaceSubstring(line, $"{i + 1}. {armourList[i].Name}, {armourList[i].Power} power, {armourList[i].Price} Gold  {(armourList[i].IsEquipped ? "(Equipped)" : "")}", secondColumnPosition);
                    }
                    if (i < consumableList.Count)
                    {
                        line = ReplaceSubstring(line, $"{i + 1}. {consumableList[i].Name}, {consumableList[i].Price} Gold", thirdColumnPosition);
                    }
                    lines.Add(line);
                }
                lines.Add("");
                lines.Add("(Press ESC to return)");

                Game.WriteColumnCentered(lines, _menuMiddleScreenHeight);

                ConsoleKey key = Console.ReadKey(intercept: true).Key;
                if (key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
        private string ReplaceSubstring(string original, string replacement, int startIndex)
        {
            char[] chars = original.ToCharArray();
            for (int i = 0; i < replacement.Length && (startIndex + i) < chars.Length; i++)
            {
                chars[startIndex + i] = replacement[i];
            }
            return new string(chars);
        }
        private void DisplayStatistics()
        {
            bool correctKeyPressed = false;

            while (!correctKeyPressed)
            {
                ResetToMenuArea();

                List<string> lines = new List<string>()
                {
                    "Showing Statistics",
                    "",
                    $"Name: {_playerCharacter.Name}",
                    $"Level: {_playerCharacter.Level}",
                    $"Health: {_playerCharacter.CurrentHealth} / {_playerCharacter.MaxHealth}",
                    $"Exp: {_playerCharacter.Exp} / {_playerCharacter.LvlUpThreshold}",
                    $"Gold: {_playerCharacter.Gold}",
                    $"Base Strength: {_playerCharacter.BaseStrength}",
                    $"Base Defense: {_playerCharacter.BaseDefense}",
                    "Monsters Slain: {add monsters slain prop to player}",
                    "",
                    "(Press ESC to return)"
                };

                for (int i = 0; i < lines.Count; i++)
                {
                    Game.WriteCentered(lines[i], _menuMiddleScreenHeight - (lines.Count / 2) + i);
                }

                ConsoleKey key = Console.ReadKey(intercept: true).Key;
                switch (key)
                {
                    case ConsoleKey.Escape:
                        correctKeyPressed = true;
                        break;

                    default:
                        break;
                }
            }
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
