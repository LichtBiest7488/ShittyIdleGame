using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using G = ShittyIdleGame.GlobalVariables;
namespace ShittyIdleGame
{
    public class Demon4 : BloodPact
    {
        public override double Cost => BaseCost;
        public Demon4(int _buildingNumber) : base("Greater Demon","Sacrifice 10 Demons to s2ummon a greater Demon to boost MoneyPrinter4",Basecost: 1.2*System.Math.Pow(10, 4), baseMultiplier: 1.5, MakeRectangle(_buildingNumber), _buildingNumber) {
            sacrificeAmount=10;
        }
        public override void Purchase()
        {
            if(G._bloodPacts[2].PurchasedCount>=sacrificeAmount){
                PurchasedCount++;
                G.Magic-=BaseCost;
                G._bloodPacts[2].Sell(sacrificeAmount);
                UpdateMyBuilding();
            }
            
        }
    }
}
