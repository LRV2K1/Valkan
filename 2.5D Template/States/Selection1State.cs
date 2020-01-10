using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

//This is the Selection Screen for Offline or Online mode.
//If Offline is Selected, the screen will change options accordingly
class Selection1State : GameObjectLibrary
{
    protected Button startButton, settingsButton, returnButton;
    protected bool firstTime = true;
    protected bool offline, mapSelection;
    public Selection1State()
    {
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey", 100, "background");
        RootList.Add(titleScreen);

        startButton = new Button("Sprites/Menu/spr_button", 101);
        startButton.Position = new Vector2((GameEnvironment.Screen.X - startButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 4);
        RootList.Add(startButton);

        settingsButton = new Button("Sprites/Menu/spr_button_intel", 101);
        settingsButton.Position = new Vector2((GameEnvironment.Screen.X - settingsButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 2);
        RootList.Add(settingsButton);

        returnButton = new Button("Sprites/Menu/spr_button_exit", 101);
        returnButton.Position = new Vector2((GameEnvironment.Screen.X - settingsButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 4 * 3);
        RootList.Add(returnButton);
    }

    public override void Update(GameTime gameTime)
    {
        if (firstTime)
        {
            //In dit scherm selecteer je of je online of offline wilt spelen. Selecteer je online. Dan ga je naar de Online Selection State
            //Offline Knop
            startButton.Active = true;
            //Online Knop
            GameEnvironment.AssetManager.PlayMusic("Soundtracks/Valkan's Fate - Battle Theme(Garageband)");
            firstTime = false;
        }
        if(offline)
        {
            //In dit scherm selecteer je welke Map je wilt spelen.
            //Knop om de game te beginnen met de standaard geselecteerde map.
            //Kleine afbeelding voor de map die geselecteerd is.
            //Knop om de map te selecteren.
            settingsButton.Active = true;
        }
        if(mapSelection)
        {
            //Hier selecteer je de map die je wilt spelen, zodra je iets hebt geselecteerd, dan verdwijnt dit scherm weer.
            //Een kleine overlay met 9 slots voor maps te selecteren, elke map heeft n afbeelding en een naam.
            returnButton.Active = true;
        }
        base.Update(gameTime);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (startButton.Pressed)
        {
            offline = true;
        }
        else if (settingsButton.Pressed)
        {
            mapSelection = true;
        }
        else if (returnButton.Pressed)
        {
            mapSelection = false;
            returnButton.Active = false;
        }
    }

    public override void Reset()
    {
        firstTime = true;
    }

}
