using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Player
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

            playerpos = new Vector2 ((playerpos.X)+1, (playerpos.Y)+0);
            if (Collision() == true)
            {
                playerpos = new Vector2((playerpos.X) - 1, (playerpos.Y) + 0);
            }
        }
        if (inputHelper.KeyDown(Keys.Left))
        {
            playerpos = new Vector2((playerpos.X) - 1, (playerpos.Y) + 0);
            if (Collision() == true)
            {
                playerpos = new Vector2((playerpos.X) + 1, (playerpos.Y) + 0);
            }
        }
        if (inputHelper.KeyDown(Keys.Up))
        {
            playerpos = new Vector2((playerpos.X) + 0, (playerpos.Y) - 1);
            if (Collision() == true)
            {
                playerpos = new Vector2((playerpos.X) + 0, (playerpos.Y) + 1);
            }
        }
        if (inputHelper.KeyDown(Keys.Down))
        {
            playerpos = new Vector2((playerpos.X) + 0, (playerpos.Y) + 1);
            if (Collision() == true)
            {
                playerpos = new Vector2((playerpos.X) - 0, (playerpos.Y) - 1);
            }
        }
        if (playerpos.X < 0)
            playerpos = new Vector2(0, (playerpos.Y));
        if (playerpos.X >= 24)
            playerpos = new Vector2(24, (playerpos.Y));
        if (playerpos.Y >= 19)
            playerpos = new Vector2((playerpos.X), 19);
        if (playerpos.Y < 0)
            playerpos = new Vector2((playerpos.X), 0);
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
