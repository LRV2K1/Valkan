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
class ModeSelectionState : GameObjectLibrary
{
    protected Button offlineButton, onlineButton, returnButton;
    public ModeSelectionState()
    {
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey", 100, "background");
        titleScreen.Sprite.Color = Color.Red;
        RootList.Add(titleScreen);
        offlineButton = new Button("Sprites/Menu/PlayOffline_Button", 101);
        offlineButton.Sprite.Size = new Vector2(2f, 3f);
        offlineButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - offlineButton.BoundingBox.Width / 2, (GameEnvironment.Screen.Y - offlineButton.BoundingBox.Height) / 4);
        RootList.Add(offlineButton);
        onlineButton = new Button("Sprites/Menu/PlayOnline_Button", 101);
        onlineButton.Sprite.Size = new Vector2(2f, 3f);
        onlineButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - onlineButton.BoundingBox.Width / 2, (GameEnvironment.Screen.Y - onlineButton.BoundingBox.Height) / 2);
        RootList.Add(onlineButton);
        returnButton = new Button("Sprites/Menu/Return_Button", 101);
        returnButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - returnButton.Width / 2, (GameEnvironment.Screen.Y - returnButton.Height) / 8 * 7);
        RootList.Add(returnButton);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (offlineButton.Pressed)
        {
            GameEnvironment.GameSettingsManager.SetValue("connection", "offline");
            GameEnvironment.ScreenFade.TransitionToScene("offlineSelectionState", 5);
        }
        else if (onlineButton.Pressed)
        {
            GameEnvironment.GameSettingsManager.SetValue("connection", "online");
            GameEnvironment.ScreenFade.TransitionToScene("hostClientSelectionState", 5);
        }
        else if (returnButton.Pressed)
        {
            GameEnvironment.ScreenFade.TransitionToScene("titleScreen", 5);
        }
    }
}
