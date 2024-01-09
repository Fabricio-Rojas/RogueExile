using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueExile.Interfaces
{
    internal interface IRenderable
    {
        // should only ever manage the displaying and printing of things on the console, not their operations or calculations
        void Render();
    }
}
