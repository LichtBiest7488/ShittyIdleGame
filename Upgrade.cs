using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using G = ShittyIdleGame.GlobalVariables;
namespace ShittyIdleGame
{
    public class Upgrade
    {
        public string Name { get; }
        public string Description { get; }
        public double Cost { get; }
        public double CpsMultiplier { get; }
        public int Unlock; //unlocked after #Unlock purchases 
        public bool purchased=false;
        public Rectangle Pos;
        public int Number;
        public Building MyBuilding;
        public Upgrade(string name, string description, int unlock, double cost, double cpsMultiplier, Building building)
        {
            Unlock = unlock;
            Name = name;
            Description = description;
            Cost = cost;
            CpsMultiplier = cpsMultiplier;
            MyBuilding = building;
        }
        public Upgrade(){
        }

        public Rectangle MakeRectangle(int _buildingNumber){
            return new Rectangle(
                    G._buildingsArea.X + (_buildingNumber % 3) *(G._upgradeButtonLength +G._upgradeButtonSpacing) + 10,
                    (_buildingNumber/3) * (G._upgradeButtonHeight + G._upgradeButtonSpacing) + G._buildingButtonHeight + 10,
                    G._upgradeButtonLength,
                    G._upgradeButtonHeight
            );
        }
        public void updatePos(int _buildingNumber)
        {
            Pos= MakeRectangle(_buildingNumber);
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D texture)
        {
            // Draw the white button background
            spriteBatch.Draw(texture, destinationRectangle: Pos, Color.White);

            string formattedCost = Cost > 999 ? Cost.ToString("E2") : Cost.ToString("F2");
            // Draw the building name, cost, and CPS
            spriteBatch.DrawString(
                font,
                $"{Name}",
                new Vector2(Pos.X + 5, Pos.Y + 10),
                Color.Black
            );
            
            spriteBatch.DrawString(
                font,
                $"{formattedCost} Dollars",
                new Vector2(Pos.X + 5, Pos.Y + G._upgradeButtonHeight/2 +5),
                Color.Black
            );
            
        }
        public override string ToString(){
            return Name;
        }
    }
}
