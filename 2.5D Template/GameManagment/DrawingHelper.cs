using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class DrawingHelper
{
    protected static Texture2D pixel;

    public static void Initialize(GraphicsDevice graphics)
    {
        pixel = new Texture2D(graphics, 1, 1);
        pixel.SetData(new[] { Color.White });
    }

    public static void DrawRectangle(Rectangle r, SpriteBatch spriteBatch, Color col)
    {
        int borderWith = 2;

        spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Top, borderWith, r.Height), col); // Left
        spriteBatch.Draw(pixel, new Rectangle(r.Right, r.Top, borderWith, r.Height), col); // Right
        spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Top, r.Width, borderWith), col); // Top
        spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Bottom, r.Width, borderWith), col); // Bottom
    }
}