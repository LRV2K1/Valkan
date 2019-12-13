using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Player //basic player class
{
    Texture2D player;
    public Vector2 playerpos;
    Rectangle playerrect;
    Grid grid;
    public Player(Grid grid)
    {
        playerpos = new Vector2(5, 19);
        playerrect = new Rectangle((int)playerpos.X, (int)playerpos.Y, 40, 40);
        this.grid = grid;
    }
    public void LoadContent()
    {
        player = Game1.ContentManager.Load<Texture2D>("player");
    }
    public void Update(InputHelper inputHelper)
    {
        if (inputHelper.KeyDown(Keys.Right))
        {

            playerpos.X++;
            if (Collision() == true)
            {
                playerpos.X--;
            }
        }
        if (inputHelper.KeyDown(Keys.Left))
        {
            playerpos.X--;
            if (Collision() == true)
            {
                playerpos.X++;
            }
        }
        if (inputHelper.KeyDown(Keys.Up))
        {
            playerpos.Y--;
            if (Collision() == true)
            {
                playerpos.Y++;
            }
        }
        if (inputHelper.KeyDown(Keys.Down))
        {
            playerpos.Y++;
            if (Collision() == true)
            {
                playerpos.Y--;
            }
        }
        if (playerpos.X < 0)
            playerpos.X = 0;
        if (playerpos.X >= 24)
            playerpos.X = 24;
        if (playerpos.Y >= 19)
            playerpos.Y = 19;
        if (playerpos.Y < 0)
            playerpos.Y = 0;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        playerrect = new Rectangle((int)playerpos.X * 40, (int)playerpos.Y * 40, 40, 40);
        spriteBatch.Draw(player, playerrect, Color.White);
    }
    public bool Collision()
    {
        bool col = false;
        for (int i = 0; i < 25; i++)
        {
            for (int u = 0; u < 20; u++)
            {
                if (grid.grid[i, u] == 0 & playerpos.X == i & playerpos.Y == u)
                    col = false;
                if (grid.grid[i, u] == 1 & playerpos.X == i & playerpos.Y == u)
                    col = true;
            }
        }
        return col;
    }
}
