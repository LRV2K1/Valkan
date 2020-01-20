﻿using System;
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
        if (!first && MultiplayerManager.online && GameEnvironment.GameSettingsManager.GetValue("host") == "true")
        {
            SetupEnitites();
            first = true;
        }
        //DistributeData();
        for (int i = 0; i < RootList.Children.Count; i++)
        {
            if (RootList.Children[i] == "entities")
            {
                continue;
            }
            GetObject(RootList.Children[i]).Update(gameTime);
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