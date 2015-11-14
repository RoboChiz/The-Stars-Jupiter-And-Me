using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeThingGameThree
{
    class StoreItem
    {
        string itemName;
        double itemPrice;

        public string Name
        {
            get { return itemName; }
            set { itemName = value; }
        }

        public double Price
        {
            get { return itemPrice; }
            set { itemPrice = value; }
        }
    }
}
