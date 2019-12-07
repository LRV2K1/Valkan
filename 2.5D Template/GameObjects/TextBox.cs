using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

public class TextBox : SpriteGameObject
{
    public TextBox(int layer = 99, string id = "textbox")
        : base("Sprites/Overlay/Menu_BG_Grey", layer, id)
    {
        this.Visible = false;
        this.Position = new Vector2(-GameEnvironment.Screen.X / 40, -(GameEnvironment.Screen.Y / 20));
        Vector2 resize = this.sprite.Size;
        resize.X = (float)GameEnvironment.Screen.X / (float)this.Sprite.Width;
        resize.Y = (float)GameEnvironment.Screen.Y / (float)this.Sprite.Height;
        Debug.WriteLine(resize.Y);
        this.sprite.Size = new Vector2(resize.X * 0.8f, resize.Y * 0.25f);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
    }
}
