using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class SkillHoverButton : Button
{
    protected TextGameObject descritption;
    protected SpriteGameObject box;
    string text;
    public SkillHoverButton(string assetname, int layer = 101, string id = "")
        : base(assetname, layer, id)
    {
        descritption = new TextGameObject("Fonts/Hud", 101);
        box = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey", 101);
        box.Sprite.Color = Color.Red;
        box.Sprite.Size = new Vector2(0.3f, 0.1f);
        box.Position = new Vector2(0, 90);
        descritption.Position = new Vector2(5, 95);
        descritption.Visible = false;
        box.Visible = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (box.Parent == null || descritption.Parent == null)
        {
            box.Parent = this;
            descritption.Parent = this;
            GameWorld.Add(box);
            GameWorld.Add(descritption);
        }
        box.Visible = highLighted;
        descritption.Visible = highLighted;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
        box.Draw(gameTime, spriteBatch);
        descritption.Draw(gameTime, spriteBatch);
    }

    public string Text
    {
        set
        {
            text = value;
            descritption.Text = text;
        }
    }
}

