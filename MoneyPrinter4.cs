using Microsoft.Xna.Framework;
using G = ShittyIdleGame.GlobalVariables;
using System;
namespace ShittyIdleGame
{

    public class MoneyPrinter4 : Building
    {
        public MoneyPrinter4(int _buildingNumber) : base("Good Money Printer", 1.2*System.Math.Pow(10, 4), 50, MakeRectangle(_buildingNumber)) {
            _upgrades.Add(new Upgrade("Good Money Printer","x2", unlock: 10, BaseCost*10, 2, this));//unlocks at 10, costs 200, multiplies by 2
            _upgrades.Add(new Upgrade("Good Money Printer","x2", unlock: 50,BaseCost*100,2, this));
            _upgrades.Add(new Upgrade("Good Money Printer","x2", unlock: 100, BaseCost*1000, 2, this));
            _upgrades.Add(new Upgrade("Good Money Printer","x2", unlock: 150, BaseCost*Math.Pow(10, 7), 2, this));
            foreach(Upgrade upgrade in _upgrades){
                _boughtUpgrades.Add(0);
            }
        }
    }
}