using Microsoft.Xna.Framework;
using G = ShittyIdleGame.GlobalVariables;
using System;
namespace ShittyIdleGame
{

    public class MoneyPrinter3 : Building
    {
        public MoneyPrinter3(int _buildingNumber) : base("Money Printer", 1.1*System.Math.Pow(10, 3), 10, MakeRectangle(_buildingNumber)) {
            _upgrades.Add(new Upgrade("Money Printer","multiplies cps x2", unlock: 10, BaseCost*10, 2, this));//unlocks at 10, costs 200, multiplies by 2
            _upgrades.Add(new Upgrade("Money Printer","x2", unlock: 50, BaseCost*1000, 2, this));
            _upgrades.Add(new Upgrade("Money Printer","x2",unlock: 100, BaseCost*10000, 2, this));
            _upgrades.Add(new Upgrade("Money Printer","x2",unlock: 150, BaseCost*Math.Pow(10, 7), 2, this));
            foreach(Upgrade upgrade in _upgrades){
                _boughtUpgrades.Add(0);
            }
        }
    }
}
