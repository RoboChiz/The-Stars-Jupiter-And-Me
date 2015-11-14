using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeThingGameThree
{
    class Store
    {
        public void BuyItem(StoreItem item, Double money)
        {
            money -= item.Price;
        }
    }

    class MoonHealthPowerUp : StoreItem
    {
        public MoonHealthPowerUp (Moon moon) {}

        public void Activate(Moon moon, float healthBonus)
        {
            moon.Health += healthBonus;
        }
    }
}
