using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using G = ShittyIdleGame.GlobalVariables;
namespace ShittyIdleGame
{
    public class Demon2 : BloodPact
    {   
        public override double Cost => BaseCost;
        public Demon2(int _buildingNumber) : base("Lesser Demon","Sacrifice 10 Imps to summon a Lesser Demon to boost MoneyPrinter2",Basecost: 100, baseMultiplier: 1.5, MakeRectangle(_buildingNumber), _buildingNumber) {
            sacrificeAmount=10;
        }
        public override void Purchase()
        {
            if(G._bloodPacts[0].PurchasedCount>=sacrificeAmount){
                PurchasedCount++;
                G.Magic-=BaseCost;
                G._bloodPacts[0].Sell(sacrificeAmount);
                UpdateMyBuilding();
            }
            
        }
    }
}
