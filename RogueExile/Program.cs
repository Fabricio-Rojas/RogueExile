using RogueExile.Classes.GameManagement;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

/*
- [DONE] add escape key close program
- [DONE] add randomly generated room layouts with paths
- [DONE] use different characters or ASCII for graphics
- [DONE] make player spawn in a random room
- [DONE] add colour to room walls, corridors, player character
- [DONE] print menu below game map, move screen with setCursorPosition
- [DONE] add an intro animation and possibly some background music or intro music
- add enemies that try to attack you once they see you
- add an item system with drops and rarities
- add multiple character classes, with a level up system with perks and upgrades
- add a UI menu that lets you see your inventory and change your equipment
- add fog of war that gets permanently cleared after it has been in a radius where the player explored
- add level completion after all enemies have been defeated, and generate a new level
- save character session progression in the form of json files that can be written and read
 */

Game game = new();
game.Start();

// add enemies, scale enemy stat amounts and room quantity by character level
// use scalings from text game

/* https://theasciicode.com.ar/
╔══╩══╗
║     ║
╣     ╠
║     ║
╚══╦══╝
╬
░▒▓·■@
*/