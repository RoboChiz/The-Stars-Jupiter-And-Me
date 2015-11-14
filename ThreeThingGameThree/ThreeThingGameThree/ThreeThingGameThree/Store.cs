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

        public void Activate(Moon moon, Double money, float healthBonus)
        {
            Store.BuyItem(this, money);

            // Make sure that the moon health is less than the max before adding it.
            if ((moon.Health < moon.MaxHealth))
            {
                if (moon.Health + healthBonus > moon.MaxHealth) {
                    moon.Health = moon.MaxHealth;
                }
                else
                {
                    moon.Health += healthBonus;
                }
            }
        }
    }

    class MoonHealthUpgradePowerUp : StoreItem
    {
        int upgradeLevel = 0;
        double[] upgradePrices = new double[] { 10.0, 25.0, 55.0, 75.0, 100.0 };

        public MoonHealthUpgradePowerUp(Moon moon, Double money) { }

        public void Activate(Moon moon, Double money)
        {
            this.Price = upgradePrices[upgradeLevel];
            Store.BuyItem(this, money);

            if (upgradeLevel <= upgradePrices.Length)
            {
                moon.MaxHealth += 25;
                moon.Health = moon.MaxHealth;
            }
        }
    }

}
