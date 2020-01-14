using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TextGameObject : GameObject
{
    protected SpriteFont spriteFont;
    protected Color color;
    protected string text;
    protected Vector2 size;

    public TextGameObject(string assetname, int layer = 0, string id = "")
        : base(layer, id)
    {
        spriteFont = GameEnvironment.AssetManager.Content.Load<SpriteFont>(assetname);
        color = Color.White;
        text = "";
        size = Vector2.One;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (visible)
        {
            spriteBatch.DrawString(spriteFont, text, GlobalPosition, color, 0.0f, Vector2.Zero, size, SpriteEffects.None, 0);
        }
    }

    public Color Color
    {
        get { return color; }
        set { color = value; }
    }

    public string Text
    {
        get { return text; }
        set { text = value; }
    }

    public override Rectangle BoundingBox
    {
        get { return new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y, (int)Size.X, (int)Size.Y); }
    }


    public Vector2 Size
    {
        get
        { return spriteFont.MeasureString(text); }
        set
        {
            size = value;
        }
    }
}