using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

partial class Level : GameObjectLibrary
{
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (string id in RootList.Children)
        {
            if (id == "entities")
            {
                continue;
            }
            GetObject(id).Draw(gameTime, spriteBatch);
        }
    }
}