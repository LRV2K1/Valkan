using System;
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
    protected LevelEditer LevelEditer;

    public EditorState()
    {
    }

    public override void Load()
    {
        string levelnum = GameEnvironment.GameSettingsManager.GetValue("level");
        LevelEditer = new LevelEditer(200, 200, true, levelnum);
        GameEnvironment.AssetManager.PlayMusic("Soundtracks/Valkan's Fate Soundtrack - TItle Screen 1");
    }

    public override void UnLoad()
    {
        LevelEditer = null;
    }

    public void NewLevel(int x, int y)
    {
        LevelEditer = new LevelEditer(x, y);
    }

    public void LoadLevel(string path)
    {
        LevelEditer = new LevelEditer(0, 0, true, path);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        LevelEditer.HandleInput(inputHelper);
    }

    public override void Update (GameTime gameTime)
    {
        LevelEditer.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        LevelEditer.Draw(gameTime, spriteBatch);
    }

    public override void Reset()
    {
        LevelEditer.Reset();
    }
}
