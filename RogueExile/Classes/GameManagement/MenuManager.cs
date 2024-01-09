using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueExile.Classes.GameManagement
{
    internal class MenuManager
    {
        public static void DisplayUI()
        {
            Game.WriteCentered("UI Placeholder", Console.WindowHeight - 1);
        }
    }
}
