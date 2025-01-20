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
    public List<IncantationData> IncantationDatas { get; set; } = new List<IncantationData>();
    public SaveData(){

    } 
    
}


public class BloodPactData
{
    public string Name { get; set; }
    public int PurchasedCount { get; set; }
//    public double cost;

    public BloodPactData()
    {
    }
}
public class IncantationData
{
    public string Name { get; set; }
    public int PurchasedCount { get; set; }
//    public double cost;

    public IncantationData()
    {
    }
}

public class BuildingData
{
    public string Name { get; set; }
    public int PurchasedCount { get; set; }
    public List<int> boughtUpgrades{ get; set;}
//    public double cost;

    public BuildingData()
    {
        boughtUpgrades = new List<int>();
    }
}
