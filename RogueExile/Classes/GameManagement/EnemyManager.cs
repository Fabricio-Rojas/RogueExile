using RogueExile.Classes.Entities;
using RogueExile.Classes.MapGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueExile.Classes.GameManagement
{
    internal class EnemyManager
    {
        private MapGenerator _map;
        private Random _random;

        public List<EnemyCharacter> Enemies;

        public EnemyManager(MapGenerator mapGenerator)
        {
            _map = mapGenerator;
            _random = new Random();
            Enemies = new List<EnemyCharacter>();
        }

        public void GenerateEnemies(int level)
        {
            List<Room> rooms = _map.rooms;
            rooms.Remove(_map.SpawnRoom);

            int enemyAmount = ((level - 1) * 3) + 5;

            for (int i = 0; i < enemyAmount; i++)
            {
                bool spaceFound = false;
                Cell spawnCell;

                do
                {
                    // get a random room, and then get a random cell within that room
                    Room selectedRoom = rooms[_random.Next(0, rooms.Count)];
                    spawnCell = selectedRoom.RoomCenter;

                    int positionXModifier = _random.Next(((selectedRoom.Width / 2) - 2) * -1, ((selectedRoom.Width / 2) - 1));
                    int positionYModifier = _random.Next(((selectedRoom.Height / 2) - 2) * -1, ((selectedRoom.Height / 2) - 1));

                    spawnCell.MoveXBy(positionXModifier);
                    spawnCell.MoveYBy(positionYModifier);

                    spawnCell = _map.mapGrid[spawnCell.X, spawnCell.Y]; 

                    if (spawnCell.Val != '·')
                    {
                        spaceFound = false;
                    }
                    else
                    {
                        spaceFound = true;
                    }
                    // if cell has an enemy, retry with a while loop
                    // after a certain amount of tries, continue the for loop
                }
                while (!spaceFound);

                EnemyCharacter enemy = new EnemyCharacter(level, spawnCell, _map.mapGrid);
                Enemies.Add(enemy);
            }
        }
        public void PerformTurns()
        {
            int enemyCount = Enemies.Count;

            for (int i = 0; i < enemyCount; i++)
            {
                Enemies[i].PerformTurn();
            }
        }
        public void RenderEnemies()
        {
            int enemyCount = Enemies.Count;

            for (int i = 0; i < enemyCount; i++)
            {
                Enemies[i].Render();
            }
        }
    }
}
