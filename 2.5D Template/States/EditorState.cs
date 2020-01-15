﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

class EditorState : State
{
    //updates the level editor
    protected LevelEditor LevelEditor;
    protected TextButton save;

    public EditorState()
    {
        save = new TextButton("Fonts/Hud", "save", 102);
        save.Color = Color.Red;
        save.Position = new Vector2(30, 20);
    }

    public override void Load()
    {
        string levelnum = GameEnvironment.GameSettingsManager.GetValue("level");
        LevelEditor = new LevelEditor(200, 200, true, levelnum);
    }

    public override void UnLoad()
    {
        LevelEditor = null;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        save.HandleInput(inputHelper);
        if (save.Pressed)
        {
            string levelnum = GameEnvironment.GameSettingsManager.GetValue("level");
            LevelEditor.Save(levelnum);
            return;
        }
        LevelEditor.HandleInput(inputHelper);
    }

    public override void Update (GameTime gameTime)
    {
        save.Update(gameTime);
        LevelEditor.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        LevelEditor.Draw(gameTime, spriteBatch);
        save.Draw(gameTime, spriteBatch);
    }

    public override void Reset()
    {
        LevelEditor.Reset();
    }
}
