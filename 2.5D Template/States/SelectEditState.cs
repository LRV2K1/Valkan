using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


class SelectEditState : GameObjectLibrary
{
    List<Button> buttonList;
    public SelectEditState()
        : base()
    {
        buttonList = new List<Button>();
        LoadButtons();
    }

    private void LoadButtons()
    {
        Vector2 startposition = new Vector2((GameEnvironment.Screen.X / 30) * 8, (GameEnvironment.Screen.Y / 13) * 4);
        Vector2 newPosition;
        int xOffset = (int)(GameEnvironment.Screen.X / 30) * 5;
        int yOffset = (int)(GameEnvironment.Screen.Y / 4);
        for (int i = 0; i < 3; i++)
        {
            newPosition = new Vector2(startposition.X, startposition.Y + yOffset * i);
            for (int j = 0; j < 3; j++)
            {
                Button button = new Button("Sprites/Menu/Select_Button", 109);
                buttonList.Add(button);
                button.Sprite.Size = new Vector2(0.6f, 0.6f);
                button.Position = new Vector2(startposition.X + j * xOffset, newPosition.Y);
                RootList.Add(button);
            }
        }
    }
    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
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

