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

    public class Incantation1 : Incantation
    {   
        public new double Cost => BaseCost;   
        private int multiplier;
        public Incantation1(int _buildingNumber) : base("Mental Training","Boost mana cap by 4", 20, MakeRectangle(_buildingNumber)) {
            multiplier=4;
        }
       
        public override void Purchase()
        {
            PurchasedCount++;
            G.MagicCap+=multiplier;
        }
        public override void SetPurchasedCount(int amount){
            G.MagicCap-=PurchasedCount*multiplier;
            PurchasedCount = amount;
            G.MagicCap+=PurchasedCount*multiplier;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D texture)
        {
            // Draw the background
            spriteBatch.Draw(texture, Pos, Color.LightGray);
            string formattedCost = BaseCost > 999 ? BaseCost.ToString("E2") : BaseCost.ToString("F2");
            // Draw the building name, cost, and CPS
            spriteBatch.DrawString(
                font,
                $"{Name} - {formattedCost} Magic (Boost Magic Cap by {multiplier})",
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