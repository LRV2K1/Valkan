using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

class TitleScreenState : GameObjectList
{
    protected Button startButton, settingsButton;
    ScreenFade screenFade;
    public TitleScreenState()
    {
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey",100,"background");
        Add(titleScreen);

        startButton = new Button("Sprites/Menu/spr_button",101);
        startButton.Position = new Vector2((GameEnvironment.Screen.X - startButton.Width) / 2, 350);
        Add(startButton);

        settingsButton = new Button("Sprites/Menu/spr_button_intel", 101);
        settingsButton.Position = new Vector2((GameEnvironment.Screen.X - settingsButton.Width) / 2, 750);
        Add(settingsButton);

        screenFade = new ScreenFade();
        Add(screenFade);
        screenFade.FadeWhite();

        GameEnvironment.AssetManager.PlaySong("The_Cure_-_Lullaby_Transcription");
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if(screenFade.FadeToWhite)
        {
            return;
        }
        base.HandleInput(inputHelper);
        if (startButton.Pressed)
        {
            GameEnvironment.AssetManager.PlaySong("Sad");
            screenFade.FadeBlack();
        }
        else if (settingsButton.Pressed)
        {
            GameEnvironment.AssetManager.PlaySong("Valkan's Fate - Battle Theme(Garageband)");
            screenFade.FadeBlack();
        }
    }

}
