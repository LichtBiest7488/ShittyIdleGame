using System.Collections.Generic;
namespace ShittyIdleGame;
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
