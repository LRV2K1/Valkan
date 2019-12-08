using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class SettingsState : GameObjectList
{
    protected Button startButton, settingsButton;
    public SettingsState()
    {
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey", 100, "background");
        Add(titleScreen);

        startButton = new Button("Sprites/Menu/spr_button_exit", 101);
        startButton.Position = new Vector2((GameEnvironment.Screen.X - startButton.Width) / 2, 540);
        Add(startButton);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (startButton.Pressed)
        {
            GameEnvironment.GameStateManager.SwitchTo("titleScreen");
        }
    }

}
