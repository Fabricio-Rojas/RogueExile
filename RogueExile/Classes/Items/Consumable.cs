using RogueExile.Classes.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RogueExile.Classes.Items
{
    internal class Consumable
    {
        private Random random = new Random();
        private int _type;
        public string Name;
        public int Price;
        public bool IsBought;

        // should use switch case instead of ifs for more
        // add a throwable acid potion
        public Consumable(int type)
        {
            IsBought = false;
            _type = type;
            if (_type < 1)
            {
                Name = "Healing Potion";
            }
            else if (_type == 1)
            {
                Name = "Strength Potion";
            }
            else if (_type > 1)
            {
                Name = "Acid Flask";
            }
            Price = random.Next(25, 100);
        }
        public string? BeConsumed(PlayerCharacter hero)
        {
            if (hero.ConsumableList.Contains(this))
            {
                hero.ConsumableList.Remove(this);
            }
            switch (_type)
            {
                case 0:
                    // message: healed hero for 20% of Max health
                    int healthHealed = (int)(hero.MaxHealth * 0.2);
                    hero.CurrentHealth += healthHealed;
                    return $"( Healed {hero.Name} for 20% of total health ({healthHealed}) )";

                case 1:
                    // next attack should deal 1.5 damage
                    hero.HasStrengthBuff = true;
                    return $"( {hero.Name} has been strengthened, dealing 50% more damage on next attack )";

                case 2:
                    // next attack will reduce monsters defense by 20% percent
                    hero.HasAcidBuff = true;
                    return $"( The next attack will reduce the enemies defence by 20% )";

                // these these buff statuses should be resolved on attacking the enemy, on the attack method

                default:
                    return default;
            }
        }
    }
}
