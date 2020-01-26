using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

partial class Level : GameObjectLibrary
{
    bool first = false;
    public override void Update(GameTime gameTime)
    {
        //DistributeData();
        for (int i = 0; i < RootList.Children.Count; i++)
        {
            if (RootList.Children[i] == "entities") 
            {
                continue;
            }
            GetObject(RootList.Children[i]).Update(gameTime);
        }
        if (!first && MultiplayerManager.Online && GameEnvironment.GameSettingsManager.GetValue("host") == "true")
        {
            SetupEnitites();
            first = true;
        }
    }

    //loops the level
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        //DistributeData();
        //skip entities
        for (int i = 0; i < RootList.Children.Count; i++)
        {
            if (RootList.Children[i] == "entities")
            {
                continue;
            }
            GetObject(RootList.Children[i]).Draw(gameTime, spriteBatch);
        }
    }
}