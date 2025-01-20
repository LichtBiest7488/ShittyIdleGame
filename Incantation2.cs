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

    public class Incantation2 : Incantation
    {   
        public new double Cost => BaseCost* Math.Pow(1.4,PurchasedCount);
        public Incantation2(int _buildingNumber) : base("Lesser Demon efficiency","Reduce imps needed by 1", 500, MakeRectangle(_buildingNumber)) {
        }
       
        public override void Purchase()
        {
            PurchasedCount++;
            G._bloodPacts[1].sacrificeAmount--;
        }
        public override void SetPurchasedCount(int amount){
            G._bloodPacts[1].sacrificeAmount+=PurchasedCount;
            PurchasedCount = amount;
            G._bloodPacts[1].sacrificeAmount-=PurchasedCount;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D texture)
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
                $"Purchased: {PurchasedCount} - Reduce imps needed by {PurchasedCount}",
                new Vector2(Pos.X + 5, Pos.Y + 45),
                Color.Black
            );
        }
    }
}