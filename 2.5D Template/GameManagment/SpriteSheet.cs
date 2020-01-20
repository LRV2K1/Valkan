using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

public class SpriteSheet
{
    protected Texture2D sprite;
    protected int sheetIndex;
    protected int sheetColumns;
    protected int sheetRows;
    protected bool mirror;
    protected Color color;
    protected Vector2 size;
    protected Rectangle cut;

    public SpriteSheet(string assetname, int sheetIndex = 0)
    {
        cut = new Rectangle(0,0,0,0);
        size = Vector2.One;
        // retrieve the sprite
        try
        {
            sprite = GameEnvironment.AssetManager.GetSprite(assetname);
        }
        catch (ContentLoadException e)
        {
            assetname = GameEnvironment.AssetManager.TestSprite;
            sprite = GameEnvironment.AssetManager.GetSprite(assetname);
            //throw new TestSpriteExeption();
        }
        color = Color.White;

        this.sheetIndex = sheetIndex;
        sheetColumns = 1;
        sheetRows = 1;

        // see if we can extract the number of sheet elements from the assetname
        string[] assetSplit = assetname.Split('@');
        if (assetSplit.Length <= 1)
        {
            return;
        }

        string sheetNrData = assetSplit[assetSplit.Length - 1];
        string[] colRow = sheetNrData.Split('x');
        sheetColumns = int.Parse(colRow[0]);
        if (colRow.Length == 2)
        {
            sheetRows = int.Parse(colRow[1]);
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin)
    {
        int columnIndex = sheetIndex % sheetColumns;
        int rowIndex = sheetIndex / sheetColumns % sheetRows;
        Rectangle spritePart = new Rectangle(columnIndex * Width + cut.X, rowIndex * Height + cut.Y, Width + cut.Width - cut.X, Height + cut.Height - cut.Y);
        SpriteEffects spriteEffects = SpriteEffects.None;
        if (mirror)
        {
            spriteEffects = SpriteEffects.FlipHorizontally;
        }
        spriteBatch.Draw(sprite, position, spritePart, color,
            0.0f, origin, size, spriteEffects, 0.0f);
    }

    public Texture2D Sprite
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
        { return sprite.Width / sheetColumns; }
    }

    public int Height
    {
        get
        { return sprite.Height / sheetRows; }
    }

    public bool Mirror
    {
        get { return mirror; }
        set { mirror = value; }
    }

    public int SheetIndex
    {
        get
        { return sheetIndex; }
        set
        {
            if (value < sheetColumns * sheetRows && value >= 0)
            {
                sheetIndex = value;
            }
        }
    }

    public int NumberSheetElements
    {
        get { return sheetColumns * sheetRows; }
    }

    public Color Color
    {
        get { return color; }
        set { color = value; }
    }

    public Vector2 Size
    {
        get { return size; }
        set { size = value; }
    }

    public Rectangle Cut
    {
        get { return cut; }
        set { cut = value; }
    }
}