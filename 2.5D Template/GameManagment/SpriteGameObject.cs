using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SpriteGameObject : GameObject
{
    protected SpriteSheet sprite;
    protected Vector2 origin;

    public SpriteGameObject(string assetName, int layer = 0, string id = "", int sheetIndex = 0)
        : base(layer, id)
    {
        if (assetName != "")
        {
            sprite = new SpriteSheet(assetName, sheetIndex);
        }
        else
        {
            sprite = null;
        }
    }    

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Camera camera = null;
        if (!visible || sprite == null)
        {
            return;
        }

        if(GameWorld != null)
        {
            //get camera
            camera = GameWorld.GetObject("camera") as Camera;

        }
        //check layer
        if (this.layer > 90)
        {
            sprite.Draw(spriteBatch, this.GlobalPosition, origin);
        }
        else
        {
            //draw in reference to camera
            if (camera != null)
            {
                if (camera.OnScreen(GlobalPosition))
                {
                    sprite.Draw(spriteBatch, this.GlobalPosition - camera.CameraPosition, origin);
                }
            }
        }
    }

    public SpriteSheet Sprite
    {
        get { return sprite; }
    }

    public Vector2 Center
    {
        get { return new Vector2(Width, Height) / 2; }
    }

    public int Width
    {
        get
        {
            return sprite.Width;
        }
    }

    public int Height
    {
        get
        {
            return sprite.Height;
        }
    }

    public bool Mirror
    {
        get { return sprite.Mirror; }
        set { sprite.Mirror = value; }
    }

    public Vector2 Origin
    {
        get { return origin; }
        set { origin = value; }
    }

    public override Rectangle BoundingBox
    {
        get
        {
            int left = (int)(GlobalPosition.X - origin.X);
            int top = (int)(GlobalPosition.Y - origin.Y);
            return new Rectangle(left, top, (int)(Width * sprite.Size.X), (int)(Height * sprite.Size.Y));
        }
    }

    public bool CollidesWith(SpriteGameObject obj)
    {
        return SpriteBounds.Intersects(obj.SpriteBounds);
    }

    public Rectangle SpriteBounds 
    {
        get
        {
            int left = (int)(GlobalPosition.X - origin.X);
            int top = (int)(GlobalPosition.Y - origin.Y);
            return new Rectangle(left, top, Width, Height);
        }
    }

    public bool OnSprite(Vector2 pos)
    {
        int left = (int)(GlobalPosition.X - origin.X);
        int top = (int)(GlobalPosition.Y - origin.Y);
        return new Rectangle(left, top, Width, Height).Contains(new Point((int)pos.X, (int)pos.Y));
    }
}

