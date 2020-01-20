using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


class SelectEditState : GameObjectLibrary
{
    List<Button> buttonList;
    List<SpriteGameObject> levelList;
    protected Button returnButton;
    public SelectEditState()
        : base()
    {
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Menu/Screen1", 100, "background");
        RootList.Add(titleScreen);
        buttonList = new List<Button>();
        levelList = new List<SpriteGameObject>();
        LoadButtons();
    }

    private void LoadButtons()
    {
        Vector2 startposition = new Vector2((GameEnvironment.Screen.X / 30) * 8, (GameEnvironment.Screen.Y / 13) * 4);
        Vector2 newPosition;
        int xOffset = (int)(GameEnvironment.Screen.X / 30) * 5;
        int yOffset = (int)(GameEnvironment.Screen.Y / 4);
        for (int y = 0; y < 3; y++)
        {
            newPosition = new Vector2(startposition.X, startposition.Y + yOffset * y);
            for (int x = 0; x < 3; x++)
            {
                Button button = new Button("Sprites/Menu/Select_Button", 109);
                buttonList.Add(button);
                button.Sprite.Size = new Vector2(0.6f, 0.6f);
                button.Position = new Vector2(startposition.X + x * xOffset, newPosition.Y);
                RootList.Add(button);
                int nummer = (3) * y + x + 1;
                SpriteGameObject level = new SpriteGameObject("Sprites/Menu/Level_Button_" + nummer, 110);
                levelList.Add(level);
                level.Sprite.Size = new Vector2(0.6f, 0.6f);
                level.Position = new Vector2(startposition.X + x * xOffset, newPosition.Y - 150);
                RootList.Add(level);
            }
        }
        returnButton = new Button("Sprites/Menu/Return_Button", 101);
        returnButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - returnButton.Width / 2, (GameEnvironment.Screen.Y - returnButton.Height) / 16 * 15);
        RootList.Add(returnButton);
    }
    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (returnButton.Pressed)
        {
            GameEnvironment.ScreenFade.TransitionToScene("titleScreen", 5);
            return;
        }
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i].Pressed)
            {
                GameEnvironment.GameSettingsManager.SetValue("level", (i + 1).ToString());
                GameEnvironment.GameStateManager.SwitchTo("editorState");
                return;
            }
        }
    }
}

