using System.Collections.Generic;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace ShittyIdleGame
{
    public static class GlobalVariables
    {
        public static double Dollars;
        public static double _dps;
        public static double ClickPower=1;
        public static double Magic;
        public static double _mps;
        public static double MagicCap;
        public static int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        public static int PlayerScore = 0;
        public static int Coins = 0;
        public static float GameSpeed = 1.0f;
        public static int buildingsAreaWidth = screenWidth/4;
        public static int leftAreaWidth = screenWidth - buildingsAreaWidth;
        public static Texture2D _dollarTexture;
        public static Rectangle _buttonBounds;
        public static int _buildingButtonHeight=80;
        public static int _buildingButtonSpacing=20;
        
        public static int buttonHeight = 50;
        public static Rectangle _buildingsArea = new Rectangle(
        screenWidth - buildingsAreaWidth, // start 1/4 from right
        0, 
        buildingsAreaWidth, 
        screenHeight
        );
        public static int _upgradeButtonLength=_buildingsArea.Width/3 - 20/3; //220;
        public static int _upgradeButtonHeight=100;
        public static int _upgradeButtonSpacing=5;

        public static Rectangle _magicBuildingArea = new Rectangle(
        screenWidth - buildingsAreaWidth, // start 1/4 from right
        screenHeight/2, //half of building area
        buildingsAreaWidth, 
        screenHeight/2 -buttonHeight-3
        );

        public static List<Building> _buildings= new List<Building>();
        public static List<Building> _magicBuildings = new List<Building>();
        public static List<Upgrade> _availableUpgrades = new List<Upgrade>();
        public static List<Upgrade> _genericUpgrades = new List<Upgrade>();
        public static List<int> _boughtGenericUpgrades =  new List<int>();
        public static List<Upgrade> _boughtUpgrades = new List<Upgrade>();
        public static List<BloodPact> _bloodPacts = new List<BloodPact>();
        public static List<Incantation> _incantations = new List<Incantation>();
    }
}
