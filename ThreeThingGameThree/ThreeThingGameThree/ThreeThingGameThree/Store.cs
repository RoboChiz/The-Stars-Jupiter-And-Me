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

    /// <summary>
    /// Increases player health by one
    /// </summary>
    class PlayerHealthReplenish : StoreItem
    {
        public PlayerHealthReplenish(NewPlayer player, Double money) 
        {
            this.Price = 20.0;
        }

        public void Activate(NewPlayer player, Double money)
        {
            int healthBonus = 1;
            Store.BuyItem(this, money);
            if ((player.Health < player.MaxHealth))
            {
                if (player.Health + healthBonus > player.MaxHealth)
                {
                    player.Health = player.MaxHealth;
                }
                else
                {
                    player.Health += healthBonus;
                }
            }
        }
    }

    /// <summary>
    /// Weapon Damage Increase Power up
    /// </summary>
    class PlayerWeaponDamageIncrease : StoreItem
    {
        int upgradeLevel = 0;
        double[] upgradePrices = new double[] { 10.0, 25.0, 55.0, 75.0, 100.0 };

        public PlayerWeaponDamageIncrease(NewPlayer player, Double money)
        {
            this.Price = upgradePrices[0];
        }

        public void Activate(NewPlayer player, Double money)
        {
            this.Price = upgradePrices[upgradeLevel];
            Store.BuyItem(this, money);

            if (upgradeLevel <= upgradePrices.Length)
            {
                player.GunDamage += 1f;
                upgradeLevel++;
            }
        }
    }

    /// <summary>
    /// Upgrade rate of fire.
    /// </summary>
    class PlayerWeaponROFIncrease : StoreItem
    {
        int upgradeLevel = 0;
        double[] upgradePrices = new double[] { 10.0, 25.0, 55.0, 75.0, 100.0 };

        public PlayerWeaponROFIncrease(NewPlayer player, Double money)
        {
            this.Price = upgradePrices[0];
        }

        public void Activate(NewPlayer player, Double money)
        {
            this.Price = upgradePrices[upgradeLevel];
            Store.BuyItem(this, money);

            if (upgradeLevel <= upgradePrices.Length)
            {
                player.GunROF += 1f;
                upgradeLevel++;
            }
        }
    }



    /// <summary>
    /// Increases the moons health by 25
    /// </summary>
    class MoonHealthReplenish : StoreItem
    {
        public MoonHealthReplenish (Moon moon, Double money) 
        {
            this.Price = 20.0;
        }

        public void Activate(Moon moon, Double money)
        {
            float healthBonus = 25;
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

    /// <summary>
    /// Upgrade the moons health capacity
    /// </summary>
    class MoonHealthUpgradePowerUp : StoreItem
    {
        int upgradeLevel = 0;
        double[] upgradePrices = new double[] { 10.0, 25.0, 55.0, 75.0, 100.0 };

        public MoonHealthUpgradePowerUp(Moon moon, Double money) 
        {
            this.Price = upgradePrices[0];
        }

        public void Activate(Moon moon, Double money)
        {
            this.Price = upgradePrices[upgradeLevel];
            Store.BuyItem(this, money);

            if (upgradeLevel <= upgradePrices.Length)
            {
                moon.MaxHealth += 25;
                moon.Health = moon.MaxHealth;
                upgradeLevel++;
            }
        }
    }



}
