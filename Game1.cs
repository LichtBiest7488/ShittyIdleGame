using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using G = ShittyIdleGame.GlobalVariables;
namespace ShittyIdleGame;

public class Game1 : Game
{
    private MouseState _previousMouseState;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private SpriteFont _ScoreFont;
    
    //Button stuff

    private Texture2D _buttonTexture;
    
    private Dollar dollar;
    

    private enum UIState
    {
        Buildings,
        Upgrades,
        Magic
    }
    private UIState _currentUIState;
    private List<Rectangle> _uiSwitchButtons;
    private List<string> _uiSwitchButtonLabels;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Set the game window size to the screen resolution
        _graphics.PreferredBackBufferWidth = G.screenWidth;
        _graphics.PreferredBackBufferHeight = G.screenHeight;
        // Make the window borderless
        Window.IsBorderless = false;
        // Center the window on the screen
        Window.Position = new Point(0, 0);
        _graphics.ApplyChanges();


        // Reserve space for buildings UI
        GlobalVariables._buildingsArea = new Rectangle(
            G.screenWidth - G.buildingsAreaWidth, // start 1/4 from right
            0, G.buildingsAreaWidth, G.screenHeight
        );

        //Define buttons for UI switching
        _uiSwitchButtons = new List<Rectangle>();
        _uiSwitchButtonLabels = new List<string> { "Buildings", "Upgrades", "Magic" };

        int buttonWidth = GlobalVariables._buildingsArea.Width / _uiSwitchButtonLabels.Count;

        int yPosition = GlobalVariables._buildingsArea.Bottom - G.buttonHeight;

        for (int i = 0; i < _uiSwitchButtonLabels.Count; i++)
        {
            _uiSwitchButtons.Add(new Rectangle(
                G._buildingsArea.X + i * buttonWidth,
                yPosition,
                buttonWidth,
                G.buttonHeight
            ));
        }


        G.MagicCap=20;
        var saveData = SaveManager.Load();
        if (saveData != null)
        {//load Game
            G.Dollars = saveData.Dollars;
            G._dps = 0;
            G.Magic = saveData.Magic;
            G._mps=0;
            int i=0;

            //load buildings and building upgrades
            foreach(BuildingData buildingData in saveData.BuildingDatas){
                String name = buildingData.Name;
                switch(name){
                    case "Bad Money Printer":
                    G._buildings.Add(new MoneyPrinter1(G._buildings.Count));
                    break;
                    case "Okay Money Printer":
                    G._buildings.Add(new MoneyPrinter2(G._buildings.Count));
                    break;
                    case "Money Printer":
                    G._buildings.Add(new MoneyPrinter3(G._buildings.Count));
                    break;
                    case "Good Money Printer":
                    G._buildings.Add(new MoneyPrinter4(G._buildings.Count));
                    break;
                }
                G._buildings[i].PurchasedCount=buildingData.PurchasedCount;
                int j=0;
                foreach(int upgrade in buildingData.boughtUpgrades){
                    if(upgrade==1){
                        G._buildings[i].BuyUpgrade(j);
                    }else{
                        Upgrade _thisUpgrade=G._buildings[i].GetUpgrades()[j];
                        if(_thisUpgrade.Unlock<=G._buildings[i].PurchasedCount){
                            if(!G._availableUpgrades.Contains(_thisUpgrade)){
                                    _thisUpgrade.updatePos(G._availableUpgrades.Count);
                                    _thisUpgrade.Number=G._availableUpgrades.Count;
                                    G._availableUpgrades.Add(_thisUpgrade);
                                }
                        }
                    }
                    j++;
                }

                G._dps+=G._buildings[i].DPS*G._buildings[i].PurchasedCount;
                i++;
            }

            //load magic buildings and their upgrades
            i=0;
            foreach(BuildingData buildingData in saveData.MagicBuildingDatas){
                String name = buildingData.Name;
                switch(name){
                    case "BloodAltar":
                    G._magicBuildings.Add(new BloodAltar(G._magicBuildings.Count));
                    break;
                }
                G._magicBuildings[i].PurchasedCount=buildingData.PurchasedCount;
                int j=0;
                foreach(int upgrade in buildingData.boughtUpgrades){
                    if(upgrade==1){
                        G._magicBuildings[i].BuyUpgrade(j);
                    }else{
                        Upgrade _thisUpgrade=G._magicBuildings[i].GetUpgrades()[j];
                        if(_thisUpgrade.Unlock<=G._magicBuildings[i].PurchasedCount){
                            if(!G._availableUpgrades.Contains(_thisUpgrade)){
                                    _thisUpgrade.updatePos(G._availableUpgrades.Count);
                                    _thisUpgrade.Number=G._availableUpgrades.Count;
                                    G._availableUpgrades.Add(_thisUpgrade);
                                }
                        }
                    }
                    j++;
                }
                G._mps+=G._magicBuildings[i].DPS*G._magicBuildings[i].PurchasedCount;
                i++;
            }
            //Initialize the Dollar button
            dollar = new Dollar();

            //load Click Upgrades
            int UpgradeID=0;
            foreach(int upgrade in saveData.DollarUpgrades){
                if(upgrade==1){
                    dollar.BuyUpgrade(UpgradeID);
                }else{
                    Upgrade _thisUpgrade = dollar.GetUpgrades()[UpgradeID];
                    if(_thisUpgrade.Unlock<=G._buildings[UpgradeID/3].PurchasedCount){
                        if(!G._availableUpgrades.Contains(_thisUpgrade)){
                            _thisUpgrade.updatePos(G._availableUpgrades.Count);
                            _thisUpgrade.Number=G._availableUpgrades.Count;
                            G._availableUpgrades.Add(_thisUpgrade);
                        }
                    }
                }
                UpgradeID++;
            }

            //Initialize the Bloodpacts
            foreach(BloodPactData bloodPactData in saveData.BloodPactDatas){
                String name = bloodPactData.Name;
                switch(name){
                    case "Imp":
                    G._bloodPacts.Add(new Demon1(G._bloodPacts.Count));
                    G._bloodPacts[0].PurchasedCount=bloodPactData.PurchasedCount;
                    G._bloodPacts[0].UpdateMyBuilding();
                    break;
                }
                
            }
            //calculate D/s
            G._dps=0;
            foreach(Building building in G._buildings){
                G._dps+=building.DPS;
            }
            TimeSpan offlineDuration = (DateTime.Now-saveData.LastSavedTime);
            double offlineSeconds = offlineDuration.TotalSeconds;
            System.Console.WriteLine($" Dollars before save: {G.Dollars.ToString("E2")} - DPS: {G._dps.ToString("E2")}");
            System.Console.WriteLine($"offlineSeconds: {offlineSeconds} - Produced Dollars: {(offlineSeconds*G._dps).ToString("E2")} - ");
            G.Dollars+=(offlineSeconds*G._dps);
            System.Console.WriteLine($"new Dollars: {G.Dollars.ToString("E2")}");
        }else
        {//make new Game
            G.Dollars=10;
            G.Magic=0;
            G._buildings.Add(new MoneyPrinter1(G._buildings.Count));
            G._buildings.Add(new MoneyPrinter2(G._buildings.Count));
            G._buildings.Add(new MoneyPrinter3(G._buildings.Count));
            G._buildings.Add(new MoneyPrinter4(G._buildings.Count));
            G._magicBuildings.Add(new BloodAltar(G._magicBuildings.Count));
            G._bloodPacts.Add(new Demon1(G._bloodPacts.Count));
            dollar = new Dollar();
        }
        base.Initialize();
    }



        protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("DefaultFont");
        _ScoreFont = Content.Load<SpriteFont>("ScoreFont");


        _buttonTexture = new Texture2D(GraphicsDevice, 1,1);
        _buttonTexture.SetData(new[]{Color.White});

        // Load the dollar texture
        G._dollarTexture = Content.Load<Texture2D>("Dollar");
    }

    protected override void OnExiting(object sender, ExitingEventArgs args)
    {   
        //calculate D/s
        G._dps=0;
        foreach(Building building in G._buildings){
            G._dps+=building.DPS;
        }
        //calculate M/s
        G._mps=0;
        foreach(Building building in G._magicBuildings){
            G._mps+=building.DPS;
        }
        List<BuildingData> buildingDatas = new List<BuildingData>();
        List<BuildingData> magicBuildingDatas = new List<BuildingData>();
        foreach (Building building in G._buildings)
        {
            buildingDatas.Add(new BuildingData{
                PurchasedCount = building.PurchasedCount,
                Name = building.Name,
                boughtUpgrades = building.GetBoughtUpgrades()
            });
        }
        foreach (Building building in G._magicBuildings)
        {
           magicBuildingDatas.Add(new BuildingData{
                PurchasedCount = building.PurchasedCount,
                Name = building.Name,
                boughtUpgrades = building.GetBoughtUpgrades()
            });
        }
        List<BloodPactData> blooPactDatas = new List<BloodPactData>();
        foreach(BloodPact bloodPact in G._bloodPacts){
            blooPactDatas.Add(new BloodPactData{
                Name = bloodPact.Name,
                PurchasedCount = bloodPact.PurchasedCount
            });
        }
        SaveData savedata =new SaveData{
            Dollars= G.Dollars,
            BuildingDatas = buildingDatas,
            MagicBuildingDatas = magicBuildingDatas,
            LastSavedTime = DateTime.Now,
            Magic = G.Magic,
            DollarUpgrades = dollar.GetBoughtUpgrades(),
            BloodPactDatas = blooPactDatas
        };
        
        SaveManager.Save(savedata);
        //System.Console.WriteLine($"Dollars: {G.Dollars}, DPS: {G._dps}, BoughtUpgrades: {G._boughtUpgrades.Count}, AvailableUpgrades: {G._availableUpgrades.Count}, Buildings: {G._buildings.Count}");
        //System.Console.WriteLine(JsonSerializer.Serialize(savedata));
        base.OnExiting(sender, args);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        //System.Console.WriteLine($"BloodMultiplier: {G._buildings[0].BloodMultiplier} - UpgradeMult: {G._buildings[0].UpgradeMultiplier}");

        /*
        string upgradeNames = "";
        foreach(Upgrade upgrade in G._availableUpgrades){
            upgradeNames+= (upgrade.ToString()+", ");
        }
        System.Console.WriteLine($"Number of available ugrades: {G._availableUpgrades.Count} - Upgrades: {upgradeNames}");
        */

        //Get MouseState
        MouseState currentMouseState = Mouse.GetState();

        // Check for button clicks
        if (currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
        {
            Point mousePos = currentMouseState.Position;

            // Check button
            if (dollar.Rectangle.Contains(mousePos))
            {   
                //System.Console.WriteLine($"Clicked the Button - Producing: {G.ClickPower}");
                G.Dollars+=G.ClickPower;
            }

            // Check if any UI switch button is clicked
            for (int i = 0; i < _uiSwitchButtons.Count; i++)
            {
                if (_uiSwitchButtons[i].Contains(mousePos))
                {
                    _currentUIState = (UIState)i; // Update the UI state based on the button index
                }
            }
            if(_currentUIState == UIState.Buildings){
                // Check if building is bought
                for (int i = 0; i < G._buildings.Count; i++)
                {
                    if (G._buildings[i].Rectangle.Contains(mousePos) && G.Dollars >= G._buildings[i].Cost)
                    {
                        G.Dollars -= G._buildings[i].Cost;
                        G._buildings[i].Purchase();

                        int j=0;
                        //check click upgrades
                        foreach(Upgrade upgrade in dollar.GetUpgrades()){
                            if(upgrade.Unlock<=G._buildings[j/3].PurchasedCount && dollar.GetBoughtUpgrades()[j]!=1 && !G._availableUpgrades.Contains(upgrade)){
                                //System.Console.WriteLine($"{upgrade.Unlock} <= {G._buildings[j/3].PurchasedCount} && {dollar.GetBoughtUpgrades()[j]} != 1 ");
                                upgrade.updatePos(G._availableUpgrades.Count);
                                upgrade.Number=G._availableUpgrades.Count;
                                G._availableUpgrades.Add(upgrade);
                            }
                            j++;
                        }


                        j=0;
                        //check Building upgrades
                        foreach(Upgrade upgrade in G._buildings[i].GetUpgrades()){
                            if(upgrade.Unlock<=G._buildings[i].PurchasedCount && G._buildings[i].GetBoughtUpgrades()[j]!=1){
                                if(!G._availableUpgrades.Contains(upgrade)){
                                    upgrade.updatePos(G._availableUpgrades.Count);
                                    upgrade.Number=G._availableUpgrades.Count;
                                    G._availableUpgrades.Add(upgrade);
                                }
                            }
                            j++;
                        }
                    }
                }
                //Check if magic building is bought
                for(int i=0; i< G._magicBuildings.Count; i++)
                {
                    if (G._magicBuildings[i].Rectangle.Contains(mousePos) && G.Dollars >= G._magicBuildings[i].Cost)
                    {   
                        System.Console.WriteLine($"we are in building: {G._magicBuildings[i].Name}");
                        G.Dollars -= G._magicBuildings[i].Cost;
                        G._mps += G._magicBuildings[i].DPS;
                        G._magicBuildings[i].Purchase();
                        int j=0;
                        foreach(Upgrade upgrade in G._magicBuildings[i].GetUpgrades()){
                            if(upgrade.Unlock<=G._magicBuildings[i].PurchasedCount && G._magicBuildings[i].GetBoughtUpgrades()[j]!=1){
                                if(!G._availableUpgrades.Contains(upgrade)){
                                    upgrade.updatePos(G._availableUpgrades.Count);
                                    upgrade.Number=G._availableUpgrades.Count;
                                    G._availableUpgrades.Add(upgrade);
                                }
                            }
                            j++;
                        }
                    }
                }
            }
            if(_currentUIState == UIState.Upgrades){
            //check if upgrade is bought
                List<Upgrade> itemsToRemove = new List<Upgrade>();
                foreach(Upgrade upgrade in G._availableUpgrades){
                    if(upgrade.Pos.Contains(mousePos) && G.Dollars>= upgrade.Cost){
                        G.Dollars-= upgrade.Cost;
                        upgrade.MyBuilding.BuyUpgrade(upgrade);
                        itemsToRemove.Add(upgrade);

                    }
                }
                foreach(Upgrade upgrade in itemsToRemove){
                    G._availableUpgrades.Remove(upgrade);
                }
                int i=0;
                foreach(Upgrade upgrade in G._availableUpgrades){
                    upgrade.updatePos(i);
                    i++;
                }
            }
            if(_currentUIState == UIState.Magic){
                foreach(BloodPact bloodPact in G._bloodPacts){
                    if(bloodPact.rectangle.Contains(mousePos) && G.Magic>= bloodPact.BaseCost){
                        G.Magic-=bloodPact.BaseCost;
                        bloodPact.Purchase();
                    }
                }
            }

        }

        //calculate D/s
        G._dps=0;
        foreach(Building building in G._buildings){
            G._dps+=building.DPS;
        }
        //calculate M/s
        G._mps=0;
        foreach(Building building in G._magicBuildings){
            G._mps+=building.DPS;
        }

        // Add D/s and M/s
        G.Dollars += G._dps * gameTime.ElapsedGameTime.TotalSeconds;
        G.Magic = Math.Min(G.MagicCap, G.Magic+ G._mps* gameTime.ElapsedGameTime.TotalSeconds);


        _previousMouseState = currentMouseState;
        base.Update(gameTime);
    }


    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        // Define the box for "Clicks" and "D/s":
        int boxWidth = 750; // Width of the box
        int boxHeight = 200; // Height of the box

        //Dollar and D/s Boxes
        Rectangle DollarBox = new Rectangle(10, 10, boxWidth, boxHeight);
        // Draw the box background:
        Texture2D boxTexture = new Texture2D(GraphicsDevice, 1, 1);
        boxTexture.SetData(new[] { Color.LightGray });
        _spriteBatch.Draw(boxTexture, DollarBox, Color.LightGray);
        // Draw border around the box:
        int borderThickness = 4;
        Texture2D borderTexture = new Texture2D(GraphicsDevice, 1, 1);
        borderTexture.SetData(new[] { Color.Black });
        _spriteBatch.Draw(borderTexture, new Rectangle(DollarBox.X, DollarBox.Y, boxWidth, borderThickness), Color.Black); // Top border
        _spriteBatch.Draw(borderTexture, new Rectangle(DollarBox.X, DollarBox.Y, borderThickness, boxHeight), Color.Black); // Left border
        _spriteBatch.Draw(borderTexture, new Rectangle(DollarBox.X + boxWidth - borderThickness, DollarBox.Y, borderThickness, boxHeight), Color.Black); // Right border
        _spriteBatch.Draw(borderTexture, new Rectangle(DollarBox.X, DollarBox.Y + boxHeight - borderThickness, boxWidth, borderThickness), Color.Black); // Bottom border
        // Draw a horizontal line between "Clicks" and "CPS"
        _spriteBatch.Draw(borderTexture, new Rectangle(DollarBox.X + 5, DollarBox.Y + 100, boxWidth - 10, borderThickness), Color.Black);
        // Format the score: use scientific notation if greater than 999:
        string formattedScore = G.Dollars > 999 ? G.Dollars.ToString("E2") : G.Dollars.ToString("F2");
        string formattedProduction = G._dps > 999 ? G._dps.ToString("E2") : G._dps.ToString("F2");
        // Draw "Clicks" and "CPS" text inside the box:
        _spriteBatch.DrawString(_ScoreFont, $"Dollars: {formattedScore}", new Vector2(DollarBox.X + 20, DollarBox.Y + 5), Color.Black);
        _spriteBatch.DrawString(_ScoreFont, $"D/s: {formattedProduction}", new Vector2(DollarBox.X + 20, DollarBox.Y + 120), Color.Black);

        //Magic and M/s Boxes
        Rectangle MagicBox = new Rectangle(6+boxWidth, 10, boxWidth, boxHeight);
        // Draw the box background:
        _spriteBatch.Draw(boxTexture, MagicBox, Color.LightGray);
        // Draw border around the box:
        _spriteBatch.Draw(borderTexture, new Rectangle(MagicBox.X, MagicBox.Y, boxWidth, borderThickness), Color.Black); // Top border
        _spriteBatch.Draw(borderTexture, new Rectangle(MagicBox.X, MagicBox.Y, borderThickness, boxHeight), Color.Black); // Left border
        _spriteBatch.Draw(borderTexture, new Rectangle(MagicBox.X + boxWidth - borderThickness, MagicBox.Y, borderThickness, boxHeight), Color.Black); // Right border
        _spriteBatch.Draw(borderTexture, new Rectangle(MagicBox.X, MagicBox.Y + boxHeight - borderThickness, boxWidth, borderThickness), Color.Black); // Bottom border
        // Draw a horizontal line between "Clicks" and "CPS"
        _spriteBatch.Draw(borderTexture, new Rectangle(MagicBox.X + 5, MagicBox.Y + 100, boxWidth - 10, borderThickness), Color.Black);
        // Format the score: use scientific notation if greater than 999:
        formattedScore = G.Magic > 999 ? G.Magic.ToString("E2") : G.Magic.ToString("F2");
        formattedProduction = G._mps > 999 ? G._mps.ToString("E2") : G._mps.ToString("F2");
        // Draw "Clicks" and "CPS" text inside the box:
        _spriteBatch.DrawString(_ScoreFont, $"Magic: {formattedScore} / {G.MagicCap}", new Vector2(MagicBox.X + 20, MagicBox.Y + 5), Color.Black);
        _spriteBatch.DrawString(_ScoreFont, $"M/s: {formattedProduction}", new Vector2(MagicBox.X + 20, MagicBox.Y + 120), Color.Black);



        //Draw the Dollar Button
        dollar.Draw(_spriteBatch, _font);


        // Draw the UI area background
        _spriteBatch.Draw(_buttonTexture, G._buildingsArea, Color.Gray);

        // Draw buttons for switching UI
        for (int i = 0; i < _uiSwitchButtons.Count; i++)
        {
            
            // Draw the button background
            _spriteBatch.Draw(_buttonTexture, _uiSwitchButtons[i], Color.Gray);

            // Draw border around the box:
            _spriteBatch.Draw(borderTexture, new Rectangle(_uiSwitchButtons[i].X, _uiSwitchButtons[i].Y, _uiSwitchButtons[i].Width, borderThickness), Color.Black); // Top border
            _spriteBatch.Draw(borderTexture, new Rectangle(_uiSwitchButtons[i].X, _uiSwitchButtons[i].Y, borderThickness, _uiSwitchButtons[i].Height), Color.Black); // Left border
            _spriteBatch.Draw(borderTexture, new Rectangle(_uiSwitchButtons[i].X + _uiSwitchButtons[i].Width, _uiSwitchButtons[i].Y, borderThickness, _uiSwitchButtons[i].Height), Color.Black); // Right border
            _spriteBatch.Draw(borderTexture, new Rectangle(_uiSwitchButtons[i].X, _uiSwitchButtons[i].Y + _uiSwitchButtons[i].Height - borderThickness, _uiSwitchButtons[i].Width, borderThickness), Color.Black); // Bottom border

            // Center the label on the button
            var textSize = _font.MeasureString(_uiSwitchButtonLabels[i]);
            var textPosition = new Vector2(
                _uiSwitchButtons[i].X + (_uiSwitchButtons[i].Width - textSize.X) / 2,
                _uiSwitchButtons[i].Y + (_uiSwitchButtons[i].Height - textSize.Y) / 2
            );
            _spriteBatch.DrawString(_font, _uiSwitchButtonLabels[i], textPosition, Color.Black);
        }
        // Render different UI areas based on the current state
        switch (_currentUIState)
        {
            case UIState.Buildings:
                DrawBuildingsUI();
                break;

            case UIState.Upgrades:
                DrawUpgradesUI();
                break;

            case UIState.Magic:
                DrawMagicUI();
                break;
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }
    private void DrawBuildingsUI()
    {   

        // Draw border around head of dollar building area
        int borderThickness = 3;
        Texture2D borderTexture = new Texture2D(GraphicsDevice, 1, 1);
        borderTexture.SetData(new[] { Color.Black });
        _spriteBatch.Draw(borderTexture, new Rectangle(G._buildingsArea.X, G._buildingsArea.Y, G.buildingsAreaWidth, borderThickness), Color.Black); // Top border
        _spriteBatch.Draw(borderTexture, new Rectangle(G._buildingsArea.X, G._buildingsArea.Y, borderThickness, G.screenHeight), Color.Black); // Left border
        _spriteBatch.Draw(borderTexture, new Rectangle(G._buildingsArea.X + G.buildingsAreaWidth - borderThickness, G._buildingsArea.Y, borderThickness, G._buildingButtonHeight), Color.Black); // Right border
        _spriteBatch.Draw(borderTexture, new Rectangle(G._buildingsArea.X, G._buildingsArea.Y + G._buildingButtonHeight - borderThickness, G.buildingsAreaWidth, borderThickness), Color.Black); // Bottom border

        // Center the label on the button
        var textSize = _font.MeasureString("Dollar Buildings");
        var textPosition = new Vector2(
            G._buildingsArea.X + (G.buildingsAreaWidth - textSize.X) / 2,
            G._buildingsArea.Y + (G._buildingButtonHeight - textSize.Y) / 2
        );
        _spriteBatch.DrawString(_font, "Dollar Buildings", textPosition, Color.Black);

        //_spriteBatch.DrawString(_font, "Dollar Buildings", new Vector2(G._buildingsArea.X + 10, G._buildingsArea.Y + G._buildingButtonHeight/3), Color.Black);
        // Draw each normal building
        for (int i = 0; i < G._buildings.Count; i++)
        {
            G._buildings[i].Draw(_spriteBatch, _font, _buttonTexture);
        }

        //Setup Magic Area
        //_spriteBatch.Draw(_buttonTexture, G._magicBuildingArea, Color.Gray);
        borderTexture = new Texture2D(GraphicsDevice, 1, 1);
        borderTexture.SetData(new[] { Color.Black });
        //_spriteBatch.Draw(borderTexture, new Rectangle(G._buildingsArea.X, G._magicBuildingArea.Y-3, G.screenWidth/4, 3), Color.Black); //Black Line between Areas

        //border around head of Magic building area
        _spriteBatch.Draw(borderTexture, new Rectangle(G._magicBuildingArea.X, G._magicBuildingArea.Y, G.buildingsAreaWidth, borderThickness), Color.Black); // Top border
        _spriteBatch.Draw(borderTexture, new Rectangle(G._magicBuildingArea.X, G._magicBuildingArea.Y, borderThickness, G._buildingButtonHeight), Color.Black); // Left border
        _spriteBatch.Draw(borderTexture, new Rectangle(G._magicBuildingArea.X + G.buildingsAreaWidth - borderThickness, G._magicBuildingArea.Y, borderThickness, G._buildingButtonHeight), Color.Black); // Right border
        _spriteBatch.Draw(borderTexture, new Rectangle(G._magicBuildingArea.X, G._magicBuildingArea.Y + G._buildingButtonHeight - borderThickness, G.buildingsAreaWidth, borderThickness), Color.Black); // Bottom border

        // Center the label on the button
        textSize = _font.MeasureString("Magic Buildings");
        textPosition = new Vector2(
            G._magicBuildingArea.X + (G.buildingsAreaWidth - textSize.X) / 2,
            G._magicBuildingArea.Y + (G._buildingButtonHeight - textSize.Y) / 2
        );
        _spriteBatch.DrawString(_font, "Magic Buildings", textPosition, Color.Black);
        //Draw each Magic building
        for(int i=0; i< G._magicBuildings.Count;i++){
            G._magicBuildings[i].Draw(_spriteBatch, _font, _buttonTexture);
        }

    }

    private void DrawUpgradesUI()
    {   
        // Draw border around head of Upgrade area
        int borderThickness = 3;
        Texture2D borderTexture = new Texture2D(GraphicsDevice, 1, 1);
        borderTexture.SetData(new[] { Color.Black });
        _spriteBatch.Draw(borderTexture, new Rectangle(G._buildingsArea.X, G._buildingsArea.Y, G.buildingsAreaWidth, borderThickness), Color.Black); // Top border
        _spriteBatch.Draw(borderTexture, new Rectangle(G._buildingsArea.X, G._buildingsArea.Y, borderThickness, G._buildingButtonHeight), Color.Black); // Left border
        _spriteBatch.Draw(borderTexture, new Rectangle(G._buildingsArea.X + G.buildingsAreaWidth - borderThickness, G._buildingsArea.Y, borderThickness, G._buildingButtonHeight), Color.Black); // Right border
        _spriteBatch.Draw(borderTexture, new Rectangle(G._buildingsArea.X, G._buildingsArea.Y + G._buildingButtonHeight - borderThickness, G.buildingsAreaWidth, borderThickness), Color.Black); // Bottom border

        // Center the label on the button
        var textSize = _font.MeasureString("Upgrades");
        var textPosition = new Vector2(
            G._buildingsArea.X + (G.buildingsAreaWidth - textSize.X) / 2,
            G._buildingsArea.Y + (G._buildingButtonHeight - textSize.Y) / 2
        );
        _spriteBatch.DrawString(_font, "Upgrades", textPosition, Color.Black);

        for (int i = 0; i < G._availableUpgrades.Count; i++)
        {   
            G._availableUpgrades[i].Draw(_spriteBatch, _font, _buttonTexture);
        }
    }

    private void DrawMagicUI()
    {
        // Draw border around head of Magic area
        int borderThickness = 3;
        Texture2D borderTexture = new Texture2D(GraphicsDevice, 1, 1);
        borderTexture.SetData(new[] { Color.Black });
        _spriteBatch.Draw(borderTexture, new Rectangle(G._buildingsArea.X, G._buildingsArea.Y, G.buildingsAreaWidth, borderThickness), Color.Black); // Top border
        _spriteBatch.Draw(borderTexture, new Rectangle(G._buildingsArea.X, G._buildingsArea.Y, borderThickness, G._buildingButtonHeight), Color.Black); // Left border
        _spriteBatch.Draw(borderTexture, new Rectangle(G._buildingsArea.X + G.buildingsAreaWidth - borderThickness, G._buildingsArea.Y, borderThickness, G._buildingButtonHeight), Color.Black); // Right border
        _spriteBatch.Draw(borderTexture, new Rectangle(G._buildingsArea.X, G._buildingsArea.Y + G._buildingButtonHeight - borderThickness, G.buildingsAreaWidth, borderThickness), Color.Black); // Bottom border

        // Center the label on the button
        var textSize = _font.MeasureString("Blood Pacts");
        var textPosition = new Vector2(
            G._buildingsArea.X + (G.buildingsAreaWidth - textSize.X) / 2,
            G._buildingsArea.Y + (G._buildingButtonHeight - textSize.Y) / 2
        );
        _spriteBatch.DrawString(_font, "Blood Pacts", textPosition, Color.Black);

        for (int i = 0; i < G._bloodPacts.Count; i++)
        {
            G._bloodPacts[i].Draw(_spriteBatch, _font, _buttonTexture);
        }
    }


}
