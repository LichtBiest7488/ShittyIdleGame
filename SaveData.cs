using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShittyIdleGame;
using G = ShittyIdleGame.GlobalVariables;


public class SaveData
{
    public double Dollars { get; set; }
    public double Magic { get; set; }
    public DateTime LastSavedTime{get; set;}
    public List<BuildingData> BuildingDatas { get; set; } = new List<BuildingData>();
    public List<BuildingData> MagicBuildingDatas { get; set; } = new List<BuildingData>();
    public List<int> DollarUpgrades  { get; set; } = new List<int>();
    public List<BloodPactData> BloodPactDatas { get; set; } = new List<BloodPactData>();

    public SaveData()
    {

    }
}
