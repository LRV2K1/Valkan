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
class HostClientSelectionState : GameObjectLibrary
{
    protected Button startButton, settingsButton, returnButton;
    protected bool firstTime = true;
    public HostClientSelectionState()
    {
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey", 100, "background");
        RootList.Add(titleScreen);
        //Create Game button
        startButton = new Button("Sprites/Menu/spr_button", 101);
        startButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - startButton.Width / 2, (GameEnvironment.Screen.Y - startButton.Height) / 4);
        RootList.Add(startButton);
        //Join Game button
        settingsButton = new Button("Sprites/Menu/spr_button_intel", 101);
        settingsButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - settingsButton.Width / 2, (GameEnvironment.Screen.Y - settingsButton.Height) / 2);
        RootList.Add(settingsButton);
        //Return button
        returnButton = new Button("Sprites/Menu/spr_button_exit", 101);
        returnButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - returnButton.Width / 2, (GameEnvironment.Screen.Y - returnButton.Height) / 8 * 7);
        RootList.Add(returnButton);
    }

    public override void Update(GameTime gameTime)
    {
        if (firstTime)
        {
            GameEnvironment.AssetManager.PlayMusic("Soundtracks/Valkan's Fate - Battle Theme(Garageband)");
            firstTime = false;
        }
        base.Update(gameTime);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (startButton.Pressed)
        {
            GameEnvironment.ScreenFade.TransitionToScene("hostSelectionState");
        }
        else if (settingsButton.Pressed)
        {
            GameEnvironment.ScreenFade.TransitionToScene("portSelectionState");
        }
        else if (returnButton.Pressed)
        {
            GameEnvironment.ScreenFade.TransitionToScene("titleScreen");
        }
    }

    public override void Reset()
    {
        firstTime = true;
    }

}
