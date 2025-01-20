using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using G = ShittyIdleGame.GlobalVariables;
namespace ShittyIdleGame
{
    public class Demon3 : BloodPact
    {
        public override double Cost => BaseCost;
        public Demon3(int _buildingNumber) : base("Demon","Sacrifice 10 Lesser Demons to ummon a normal Demon to boost MoneyPrinter3",Basecost: 1.1*System.Math.Pow(10, 3), baseMultiplier: 1.5, MakeRectangle(_buildingNumber), _buildingNumber) {
            sacrificeAmount=10;
        }
        public override void Purchase()
        {
            if(G._bloodPacts[1].PurchasedCount>=sacrificeAmount){
                PurchasedCount++;
                G.Magic-=BaseCost;
                G._bloodPacts[1].Sell(sacrificeAmount);
                UpdateMyBuilding();
            }
            
        }
    }
}
