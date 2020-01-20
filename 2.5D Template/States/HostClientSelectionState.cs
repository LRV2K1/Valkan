using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

//This is the Selection Screen for creating or joining a game.
class HostClientSelectionState : GameObjectLibrary
{
    protected Button createGameButton, joinGameButton, returnButton;
    public HostClientSelectionState()
    {
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey", 100, "background");
        RootList.Add(titleScreen);
        createGameButton = new Button("Sprites/Menu/CreateGame_Button", 101);
        createGameButton.Sprite.Size = new Vector2(2f, 3f);
        createGameButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - createGameButton.BoundingBox.Width / 2, (GameEnvironment.Screen.Y - createGameButton.BoundingBox.Height) / 4);
        RootList.Add(createGameButton);
        joinGameButton = new Button("Sprites/Menu/JoinGame_Button", 101);
        joinGameButton.Sprite.Size = new Vector2(2f, 3f);
        joinGameButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - joinGameButton.BoundingBox.Width / 2, (GameEnvironment.Screen.Y - joinGameButton.BoundingBox.Height) / 2);
        RootList.Add(joinGameButton);
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
        if (createGameButton.Pressed && MultiplayerManager.party == null)
        {
            MultiplayerManager.Connect(9999);
            MultiplayerManager.party.playerlist.Modify(Connection.MyIP(), true, true);
            GameEnvironment.ScreenFade.TransitionToScene("hostSelectionState", 5);
        }
        else if (joinGameButton.Pressed)
        {
            MultiplayerManager.Connect(1000);
            GameEnvironment.ScreenFade.TransitionToScene("portSelectionState", 5);
        }
        else if (returnButton.Pressed)
        {
            GameEnvironment.ScreenFade.TransitionToScene("modeSelectionState", 5);
        }
    }
}
