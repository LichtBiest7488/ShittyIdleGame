using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using G = ShittyIdleGame.GlobalVariables;
namespace ShittyIdleGame
{
    public class BloodPact
    {
        public string Name { get; }
        public string Description { get; }
        public double BaseCost;
        public double Cost { get; }
        public double BaseMultiplier;
        //public double CpsMultiplier { get; }
        public virtual double ProductionMultiplier => (double) System.Math.Pow(BaseMultiplier, PurchasedCount);
        public int Unlock; //unlocked after #Unlock purchases 
        public int PurchasedCount;
        public Rectangle rectangle;
        public int Number;
        public int MyBuilding;
        public BloodPact(string name, string description, double Basecost, double baseMultiplier, Rectangle Rectangle, int myBuilding)
        {
            rectangle = Rectangle;
            Name = name;
            Description = description;
            BaseCost = Basecost;
            BaseMultiplier = baseMultiplier;
            PurchasedCount=0;
            MyBuilding = myBuilding;
        }
        public BloodPact(){
        }
        public void Purchase()
        {
            PurchasedCount++;
            UpdateMyBuilding();
        }
        public void UpdateMyBuilding(){
            G._buildings[MyBuilding].BloodMultiplier=ProductionMultiplier;
        }

        public static Rectangle MakeRectangle(int _buildingNumber){
            return new Rectangle(
                    G._buildingsArea.X + 10,  //padding inside the building area
                    _buildingNumber * (G._buildingButtonHeight + G._buildingButtonSpacing) + G._buildingButtonHeight +10,//spacing between buildings  
                    G._buildingsArea.Width -20,   //subtract padding
                    G._buildingButtonHeight  //height of each building
                    );
        }
        /*public void updatePos(int _buildingNumber)
        {
            Pos= MakeRectangle(_buildingNumber);
        }*/
        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D texture)
        {
            // Draw the background
            spriteBatch.Draw(texture, rectangle, Color.LightGray);
            

            string formattedCost = BaseCost > 999 ? BaseCost.ToString("E2") : BaseCost.ToString("F2");
            // Draw the building name, cost, and CPS
            spriteBatch.DrawString(
                font,
                $"{Name} - {formattedCost} Magic (boost {G._buildings[MyBuilding]} by 50%)",
                new Vector2(rectangle.X + 5, rectangle.Y + 5),
                Color.Black
            );

            // Draw the purchased count and boost amount
            string FortmattedBoost = (ProductionMultiplier-1)*100 > 999 ? ((ProductionMultiplier-1)*100).ToString("E2") : ((ProductionMultiplier-1)*100).ToString("F2");
            spriteBatch.DrawString(
                font,
                $"Purchased: {PurchasedCount} - Boosting by: {FortmattedBoost}%",
                new Vector2(rectangle.X + 5, rectangle.Y + 45),
                Color.Black
            );
        }
    }
}
