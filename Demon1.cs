using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using G = ShittyIdleGame.GlobalVariables;
namespace ShittyIdleGame
{
    public class Demon1 : BloodPact
    {
        public Demon1(int _buildingNumber) : base("Imp","Summon an imp to boost MoneyPrinter1",Basecost: 10, baseMultiplier: 1.5, MakeRectangle(_buildingNumber), _buildingNumber) {
        }
    }
}
