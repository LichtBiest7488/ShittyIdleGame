using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using G = ShittyIdleGame.GlobalVariables;
namespace ShittyIdleGame
{
    public class Dollar : Building
    {
        public Dollar() : base("Dollar", Basecost: 10, 1, new Rectangle(
                                                                G.leftAreaWidth / 2 - 100, // Center the button horizontally
                                                                G.screenHeight / 2 - 50, // Center the button vertically
                                                                400, // Button width
                                                                200  // Button height
        )){

            _upgrades.Add(new Upgrade("Click upgrade 1","x2", unlock: 10, cost: G._buildings[0].GetBaseCost()*10, 2, this));//unlocks at MoneyPrinter1 level 10
            _upgrades.Add(new Upgrade("Click upgrade 2","x2", unlock: 50, G._buildings[0].GetBaseCost()*100, 2, this));
            _upgrades.Add(new Upgrade("Click upgrade 3","x2", unlock: 100, G._buildings[0].GetBaseCost()*1000, 2, this));

            _upgrades.Add(new Upgrade("Click upgrade 4","x2", unlock: 10, G._buildings[1].GetBaseCost()*10, 2, this));//unlocks at MoneyPrinter2 level 10
            _upgrades.Add(new Upgrade("Click upgrade 5","x2", unlock: 50, G._buildings[1].GetBaseCost()*100, 2, this));
            _upgrades.Add(new Upgrade("Click upgrade 6","x2", unlock: 100, G._buildings[1].GetBaseCost()*1000, 2, this));

            _upgrades.Add(new Upgrade("Click upgrade 7","x2", unlock: 10, G._buildings[2].GetBaseCost()*100, 2, this));//unlocks at MoneyPrinter3 level 10
            _upgrades.Add(new Upgrade("Click upgrade","x2", unlock: 50, G._buildings[2].GetBaseCost()*1000, 2, this));
            _upgrades.Add(new Upgrade("Click upgrade","x2", unlock: 100, G._buildings[2].GetBaseCost()*10000, 2, this));

            _upgrades.Add(new Upgrade("Click upgrade","x2", unlock: 10, G._buildings[3].GetBaseCost()*100, 2, this));//unlocks at MoneyPrinter4 level 10
            _upgrades.Add(new Upgrade("Click upgrade","x2", unlock: 50, G._buildings[3].GetBaseCost()*1000, 2, this));
            _upgrades.Add(new Upgrade("Click upgrade","x2", unlock: 100, G._buildings[3].GetBaseCost()*10000, 2, this));
            foreach(Upgrade upgrade in _upgrades){
                _boughtUpgrades.Add(0);
            }
        }

        public override void BuyUpgrade(Upgrade upgrade){
            if(_upgrades.Contains(upgrade)){
                G.ClickPower*=2;

                G._boughtUpgrades.Add(upgrade);
                for(int i=0; i<_upgrades.Count;i++){
                    if(_upgrades[i]==upgrade){
                        _boughtUpgrades[i]=1;
                    }
                }
                upgrade.purchased=true;
            }
        }
        public override void BuyUpgrade(int upgradePos){
            Upgrade tempUpgrade = _upgrades[upgradePos];

            G.ClickPower*=2;
            G._boughtUpgrades.Add(tempUpgrade);
            _boughtUpgrades[upgradePos]=1;
            tempUpgrade.purchased=true;
            
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            // Draw the "Click ME!!!" text above the button:
            var textSize = font.MeasureString($"Click ME!!! (Clickpower: {G.ClickPower})");
            var textPosition = new Vector2(
                Rectangle.X + (Rectangle.Width / 2) - (textSize.X / 2),
                Rectangle.Y - textSize.Y - 10
            );
            spriteBatch.DrawString(font, $"Click ME!!! (Clickpower: {G.ClickPower})", textPosition, Color.Black);
            // Draw the button
            spriteBatch.Draw(G._dollarTexture, Rectangle, Color.White);
        }
    }
}