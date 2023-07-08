using RogueExile.Classes;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

/*
- add escape key close program [Done]
- add randomly generated room layouts with paths
- use different characters or ASCII for graphics
- add enemies that try to attack you once they see you
- add an item system with drops and rarities
- add multiple character classes, with a level up system with perks and upgrades
- add a UI menu that lets you see your inventory and change your equipment
- add fog of war that gets permanently cleared after it has been in a radius where the player explored
- add level completion after all enemies have been defeated, and generate a new level
- add an intro animation and possibly some background music or intro music
 */

Game game = new();
game.Start();

// add colour to room walls, corridors, player character