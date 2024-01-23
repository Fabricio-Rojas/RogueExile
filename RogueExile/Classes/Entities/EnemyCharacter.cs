using RogueExile.Enums;
using RogueExile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueExile.Classes.Entities
{
    internal class EnemyCharacter : IRenderable, IMovable
    {
        public string Name { get; set; }
        public EnemyCharacter()
        {

        }
        public void Move(Direction direction)
        {

        }
        public void Render()
        {

        }
    }
}
