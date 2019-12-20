using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class CloseAttack : PrimairySkill
{
    protected float range;
    protected Texture2D hit1, hit2;
    protected double angle;
    public CloseAttack(string assetname, float normaltimer = 1f, float longtimer = 3f, int normaldamage = 10, int longdamage = 30)
        : base(assetname, normaltimer, longtimer, normaldamage, longdamage)
    {
        range = 100;
        angle = Math.PI / 1.7;
        hit1 = GameEnvironment.AssetManager.GetSprite("Sprites/Player/spr_attackbox");
        hit2 = hit1;
    }
    public override void Use(float timer = 2)
    {
        Attack(timer);
    }

    //hitbox test
    /*
    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        GetTargets();
    }
    */

    public void Attack(float timer)
    {
        Player player = parent as Player;
        if (!heavy && player.Stamina >= 20)
        {
            base.Use(timer);
            player.Stamina -= 20;
            player.AttackAnimation();
            //attack targets
            List<string> targets = GetTargets();
            for (int i = 0; i < targets.Count; i++)
            {
                Enemy enemy = GameWorld.GetObject(targets[i]) as Enemy;
                enemy.Health -= 10;
            }
        }
    }

    private List<string> GetTargets()
    {
        List<string> targets = new List<string>();
        LevelGrid tiles = GameWorld.GetObject("tiles") as LevelGrid;
        Player player = parent as Player;
        Vector2 gridPos = player.GridPos;

        //check surrounding tiles
        for (int x = (int)gridPos.X - 4; x <= (int)gridPos.X + 4; x++)
        {
            for (int y = (int)gridPos.Y - 4; y <= (int)gridPos.Y + 4; y++)
            {
                Tile currentTile = tiles.Get(x, y) as Tile;
                if (currentTile == null)
                {
                    continue;
                }

                /*
                currentTile.Sprite.Color = Color.Red;
                if (x == (int)gridPos.X && y == (int)gridPos.Y)
                {
                    currentTile.Sprite.Color = Color.Blue;
                }
                */

                for (int i = 0; i < currentTile.Passengers.Count; i++)
                {
                    if (currentTile.Passengers[i] != id)
                    {
                        //check tile passenger hit
                        Enemy enemy = GameWorld.GetObject(currentTile.Passengers[i]) as Enemy;
                        if (enemy == null)
                        {
                            continue;
                        }

                        //check for hit
                        Vector2 difference = enemy.GlobalPosition - player.GlobalPosition;
                        double dir = Math.Atan2((double)difference.Y, (double)difference.X);
                        float range = (float)Math.Sqrt(difference.X * difference.X + difference.Y * difference.Y);
                        if (dir < 0)
                        {
                            dir += 2 * Math.PI;
                        }
                        if (range <= this.range && dir < player.Direction + angle && dir > player.Direction - angle)
                        {
                            //enemy.Sprite.Color = Color.Blue;
                            targets.Add(enemy.Id);
                            continue;
                        }

                        //enemy.Sprite.Color = Color.White;
                    }

                }
            }
        }
        return targets;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Camera camera = GameWorld.GetObject("camera") as Camera;
        base.Draw(gameTime, spriteBatch);
        Player player = parent as Player;
        //hitbox test
        //spriteBatch.Draw(hit1, player.GlobalPosition - camera.CameraPosition, null, Color.White, (float)player.Direction + (float)angle, new Vector2(0, 1), new Vector2(range / 100, 1), SpriteEffects.None, 0);
        //spriteBatch.Draw(hit2, player.GlobalPosition - camera.CameraPosition, null, Color.White, (float)player.Direction - (float)angle, new Vector2(0, 1), new Vector2(range / 100, 1), SpriteEffects.None, 0);
    }

    public float Range
    {
        get { return range; }
        set { range = value; }
    }
}

