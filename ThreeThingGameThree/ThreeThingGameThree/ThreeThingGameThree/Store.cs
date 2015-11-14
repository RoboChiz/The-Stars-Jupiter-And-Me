using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeThingGameThree
{
    static class Store
    {
        static public void BuyItem(StoreItem item, Double money)
        {
            if (money >= item.Price) {
                money -= item.Price;
            }
        }
    }

    class MoonHealthPowerUp : StoreItem
    {
        public MoonHealthPowerUp (Moon moon, Double money) {}

        public void Activate(Moon moon, Double money, float healthBonus, float maxHealth)
        {
            Store.BuyItem(this, money);    

            // Make sure that the moon health is less than the max before adding it.
            if ((moon.Health < maxHealth))
            {
                if (moon.Health + healthBonus > maxHealth) {
                    moon.Health = maxHealth;
                }
                else
                {
                    moon.Health += healthBonus;
                }
            }
        }
    }
}
