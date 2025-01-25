using Microsoft.Xna.Framework;
using G = ShittyIdleGame.GlobalVariables;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using System;
using System.Globalization;



namespace ShittyIdleGame
{

    public class Sacrifice : Building
    {   
        public override double Cost => (float)(BaseCost * System.Math.Pow(5.0, PurchasedCount)); // Scale cost by x5 per purchase
        public Sacrifice(int _buildingNumber) : base("Human Sacrifice",Basecost: 6.66*System.Math.Pow(10, 9), 1, MakeMagicRectangle(_buildingNumber)) {
            _upgrades.Add(new Upgrade("1. Sacrifice upgrade","multiplies cps x2", 10, BaseCost*10, 2, this));//unlocks at 10, costs 200, multiplies by 2
            _upgrades.Add(new Upgrade("2. Sacrifice upgrade","x2",50,BaseCost*100,2, this));
            foreach(Upgrade upgrade in _upgrades){
                _boughtUpgrades.Add(0);
            }
        }
        public override void BuyUpgrade(Upgrade upgrade){
            if(_upgrades.Contains(upgrade)){
                this.UpgradeMultiplier*=upgrade.CpsMultiplier;
                G._boughtUpgrades.Add(upgrade);
                for(int i=0; i<_upgrades.Count;i++){
                    if(_upgrades[i]==upgrade){
                        _boughtUpgrades[i]=1;
                    }
                }
                upgrade.purchased=true;
            }
        }
        public override void Purchase()
        {
            G._mps-=DPS;
            PurchasedCount++;
            G._mps+=DPS;
        }
        public override void BuyUpgrade(int upgradePos){
            Upgrade tempUpgrade = _upgrades[upgradePos];
            this.UpgradeMultiplier*=tempUpgrade.CpsMultiplier;
            G._boughtUpgrades.Add(tempUpgrade);
            _boughtUpgrades[upgradePos]=1;
            tempUpgrade.purchased=true;
        }
        public override void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D texture)
        {
            // Draw the background
            spriteBatch.Draw(texture, Rectangle, Color.LightGray);
            

            string formattedCost = Cost > 999 ? Cost.ToString("E2") : Cost.ToString("F2");
            // Draw the building name, cost, and CPS
            spriteBatch.DrawString(
                font,
                $"{Name} - {formattedCost} Dollars ({BaseDPS*BloodMultiplier*UpgradeMultiplier} M/s)",
                new Vector2(Rectangle.X + 5, Rectangle.Y + 5),
                Color.Black
            );

            // Draw the purchased count
            double ProductionPercent =100*(BaseDPS*PurchasedCount)/G._mps;
            System.Console.WriteLine($"G._mps: {G._mps} - BaseDPS:{BaseDPS} - ProductionPercent: {ProductionPercent}");
            //String formattedProduction = PurchasedCount > 0 ? ("("+ProductionPercent.ToString("F2")+"%)") : "(0%)";
            string formattedProductionPercent = PurchasedCount > 0 ? ("("+ ProductionPercent.ToString("F2") + "%)") : "(0%)";
            spriteBatch.DrawString(
                font,
                $"Purchased: {PurchasedCount} - Producing: {DPS} {formattedProductionPercent}",
                new Vector2(Rectangle.X + 5, Rectangle.Y + 45),
                Color.Black
            );
        }
    }
}