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
- [DONE] add a UI menu that lets you see your inventory and change your equipment
- add handling of attacks by player character
- add enemies that try to attack you once they see you
- add an item system with drops and rarities
- add multiple character classes, with a level up system with perks and upgrades
- add fog of war that gets permanently cleared after it has been in a radius where the player explored
- add level completion after all enemies have been defeated, and generate a new level
- save character session progression in the form of json files that can be written and read
 */

Game game = new();
game.Start();

// add enemies, scale enemy stat amounts and room quantity by character level
// use scalings from text game
// add player property to keep track of enemies slain

// add aimable skills and items, like the acid potion in consumables, as well as adding the posibility of aiming even melee skills
// on using such an aimable action, draw a '■' on top of the player position and have it blink between the '■' and whatever cell was beneath it
// you should be able to move the icon with the regular directional keys, and you should press enter to confirm the selection
// some actions should be point and click, meaning they can only be used on valid targets like enemies, and not on the floor
// other actions such as a splash potion or spell should be able to be used on the floor
// each of these aimables should have their own specific ranges, like one square from the character and so on
// this just means that you wont be able to move the indicator outside of the range
// they should also not be able to be moved on to walls or empty cells, as well as around walls either
// to calculate this just use the previous pathfinder algorithm to draw a straight line from the player to the target cell
// if the path to that cell contains a wall or empty cell then exclude that cell from the range of the action

// add new menu selection logic, instead of pressing the according number for its respective option, we should instead have an '■' indicator
// this indicator should appear to the left of the hovered option, and should be able to be moved with the up and down inputs
// as such you should be able to confirm your selection with the enter key, but for some menus you should still be able to use the number keys
// this would remove the 9 slot limitation for some inventories and skill bars

// add monster types similar to that of nethack

/* https://theasciicode.com.ar/
╔══╩══╗
║     ║
╣     ╠
║     ║
╚══╦══╝
╬
░▒▓█·■@¤§πΣΦΩδφ∩Þøα▲▼◘◙☼♥♦öæ&%
*/