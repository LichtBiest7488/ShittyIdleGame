using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using G = ShittyIdleGame.GlobalVariables;
namespace ShittyIdleGame
{
    public class Incantation
    {
        public string Name { get; }
        public string Description { get; }
        public double Cost;
        public double BaseCost { get; }
        public Rectangle Pos;
        public int Number;
        public Building MyBuilding;
        public int PurchasedCount;
        public Incantation(string name, string description, double basecost, Rectangle pos)
        {
            Pos = pos;
            Name = name;
            Description = description;
            BaseCost = basecost;
            PurchasedCount=0;
        }
        public Incantation(){
        }
        public virtual void Purchase()
        {
            PurchasedCount++;
        }
        public virtual void SetPurchasedCount(int amount){
            PurchasedCount = amount;
        }

        public static Rectangle MakeRectangle(int _buildingNumber){
            return new Rectangle(
                    G._magicBuildingArea.X + 10,  //padding inside the building area
                    _buildingNumber * (G._buildingButtonHeight + G._buildingButtonSpacing) + G._magicBuildingArea.Y + G._buildingButtonHeight + 10,//spacing between buildings  
                    G._buildingsArea.Width -20,   //subtract padding
                    G._buildingButtonHeight  //height of each building
                    );
        }
        public void updatePos(int _buildingNumber)
        {
            Pos= MakeRectangle(_buildingNumber);
        }
        public override string ToString(){
            return Name;
        }
        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D texture)
        {
            // Draw the background
            spriteBatch.Draw(texture, Pos, Color.LightGray);
            string formattedCost = Cost > 999 ? Cost.ToString("E2") : Cost.ToString("F2");
            // Draw the building name, cost, and CPS
            spriteBatch.DrawString(
                font,
                $"{Name} - {formattedCost} Magic",
                new Vector2(Pos.X + 5, Pos.Y + 5),
                Color.Black
            );

            // Draw the purchased count
            spriteBatch.DrawString(
                font,
                $"Purchased: {PurchasedCount}",
                new Vector2(Pos.X + 5, Pos.Y + 45),
                Color.Black
            );
        }
    }
}
