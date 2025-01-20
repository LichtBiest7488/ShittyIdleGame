using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using G = ShittyIdleGame.GlobalVariables;
using System;
namespace ShittyIdleGame
{
    public class MoneyPrinter1 : Building
    {
        public MoneyPrinter1(int _buildingNumber) : base("Bad Money Printer", 10, 0.1, MakeRectangle(_buildingNumber)) {
            _upgrades.Add(new Upgrade("Bad Money Printer 1","multiplies cps x2", unlock: 10, BaseCost*10, 2, this));//unlocks at 10, costs 200, multiplies by 2
            _upgrades.Add(new Upgrade("Bad Money Printer 2","x2", unlock: 50, BaseCost*100,2, this));
            _upgrades.Add(new Upgrade("Bad Money Printer 3","x2", unlock: 100, BaseCost*1000,2, this));
            _upgrades.Add(new Upgrade("Bad Money Printer 4","x2", unlock: 150, BaseCost*Math.Pow(10, 8),2, this));
            foreach(Upgrade upgrade in _upgrades){
                _boughtUpgrades.Add(0);
            }
        }
    }
}
