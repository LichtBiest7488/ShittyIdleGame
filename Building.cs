using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using G = ShittyIdleGame.GlobalVariables;
using System;
namespace ShittyIdleGame
{
    public class Building
    {
        public string Name { get; set; }
        protected double BaseCost{ get; set; }
        public double BaseDPS;
        public double DPS =>(double) BaseDPS*PurchasedCount*BloodMultiplier*UpgradeMultiplier;
        public double BloodMultiplier{ get; set; }
        public double UpgradeMultiplier{ get; set; }
        public int PurchasedCount { get; set; } // New property to track purchases
        public virtual double Cost => (float)(BaseCost * System.Math.Pow(1.10, PurchasedCount)); // Scale cost by 10% per purchase
        protected List<Upgrade> _upgrades{ get; set; }
       
        protected List<int> _boughtUpgrades{ get; set; }
        public Rectangle Rectangle{ get; set; }
        
        

        public Building(string name, double Basecost, double baseDps, Rectangle rectangle)
        {
            Name = name;
            BaseCost = Basecost;
            BaseDPS = baseDps;
            PurchasedCount = 0;
            _upgrades = new List<Upgrade>();
            _boughtUpgrades = new List<int>();
            //System.Console.print
            Rectangle = rectangle;
            BloodMultiplier=1;
            UpgradeMultiplier=1;
        }
        public Building(){  
        }
        public double GetBaseCost(){
            return BaseCost;
        }
        public static Rectangle MakeRectangle(int _buildingNumber){
            return new Rectangle(
                    G._buildingsArea.X + 10,  //padding inside the building area
                    _buildingNumber * (G._buildingButtonHeight + G._buildingButtonSpacing) + G._buildingButtonHeight +10,//spacing between buildings  
                    G._buildingsArea.Width -20,   //subtract padding
                    G._buildingButtonHeight  //height of each building
                    );
        }
        public static Rectangle MakeMagicRectangle(int _buildingNumber){
            return new Rectangle(
                    G._magicBuildingArea.X + 10,  //padding inside the building area
                    _buildingNumber * (G._buildingButtonHeight + G._buildingButtonSpacing) + G._magicBuildingArea.Y + G._buildingButtonHeight + 10,//spacing between buildings  
                    G._buildingsArea.Width -20,   //subtract padding
                    G._buildingButtonHeight  //height of each building
                    );
        }
        public virtual void BuyUpgrade(Upgrade upgrade){
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
        public virtual void BuyUpgrade(int upgradePos){
            Upgrade tempUpgrade = _upgrades[upgradePos];
            this.UpgradeMultiplier*=tempUpgrade.CpsMultiplier;
            G._boughtUpgrades.Add(tempUpgrade);
            _boughtUpgrades[upgradePos]=1;
            tempUpgrade.purchased=true;
        }
        // Increment purchase count
        public virtual void Purchase()
        {
            PurchasedCount++;
        }
        public List<Upgrade> GetUpgrades(){
            return _upgrades;
        }
        public List<int> GetBoughtUpgrades(){
            return _boughtUpgrades;
        }
        public void SetBoughtUpgrades(List<int> list){
            this._boughtUpgrades=list;
        }
        public override string ToString()
        {
            return Name;
        }

        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D texture)
        {
            // Draw the background
            spriteBatch.Draw(texture, Rectangle, Color.LightGray);
            

            string formattedCost = Cost > 999 ? Cost.ToString("E2") : Cost.ToString("F2");
            // Draw the building name, cost, and CPS
            spriteBatch.DrawString(
                font,
                $"{Name} - {formattedCost} Dollars ({(BaseDPS*BloodMultiplier*UpgradeMultiplier).ToString("F2")} D/s)",
                new Vector2(Rectangle.X + 5, Rectangle.Y + 5),
                Color.Black
            );

            // Draw the purchased count
            double ProductionPercent =100*(DPS)/G._dps;
            string formattedProduction = (DPS) > 999 ? (DPS).ToString("E2") : (DPS).ToString("F2");
            string formattedProductionPercent = PurchasedCount > 0 ? "("+ProductionPercent.ToString("F2")+"%)" : "(0%)";
            spriteBatch.DrawString(
                font,
                $"Purchased: {PurchasedCount} - Producing: {formattedProduction} {formattedProductionPercent}",
                new Vector2(Rectangle.X + 5, Rectangle.Y + 45),
                Color.Black
            );
        }
    }
}
