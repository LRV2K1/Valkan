using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;

partial class Level : GameObjectList
{
    public void LoadLevel(string path)
    {
        Camera camera = new Camera("player",0, "camera");
        Add(camera);

        GameMouse mouse = new GameMouse();
        Add(mouse);

        List<string> textLines = new List<string>();
        StreamReader streamReader = new StreamReader(path);

        string line = streamReader.ReadLine();
        int width = line.Length;
        while(line != null)
        {
            textLines.Add(line);
            line = streamReader.ReadLine();
        }

        LoadTiles(textLines, width);
    }

    public void LoadTiles(List<string> textlines, int width)
    {
        LevelGrid level = new LevelGrid(width, textlines.Count, 1, "tiles");
        Add(level);
        level.CellWidth = 108;
        level.CellHeight = 54;

        Camera camera = Find("camera") as Camera;
        camera.Width = (width) * level.CellWidth/2;
        camera.Height = (textlines.Count) * level.CellHeight;

        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < textlines.Count; y++)
            {
                Tile t = LoadTile(textlines[y][x], x, y);
                level.Add(t, x, y);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < textlines.Count; y++)
            {
                Tile obj = level.Get(x, y) as Tile;
                obj.CheckSprite();
            }
        }
    }

    public Tile LoadTile(char tileType, int x, int y)
    {
        switch (tileType)
        {
            case '.':
                return new Tile("Sprites/Tiles/spr_grass_1", TileType.Floor);
            case '#':
                return new Tile("Sprites/Tiles/spr_wall_itest_1", TileType.Wall);
            case '@':
                return new Tile("Sprites/Tiles/spr_wall_itest_2", TileType.Wall);
            case '!':
                return new Tile("Sprites/Tiles/spr_wall_itest_3", TileType.Wall);
            case '$':
                return new Tile("Sprites/Tiles/spr_wall_itest_4", TileType.Wall);
            case '%':
                return new Tile("Sprites/Tiles/spr_wall_itest_5", TileType.Wall);
            case '&':
                return new Tile("Sprites/Tiles/spr_wall_itest_6", TileType.Wall);
            case '*':
                return new Tile("Sprites/Tiles/spr_wall_itest_7", TileType.Wall);
            case '(':
                return new Tile("Sprites/Tiles/spr_wall_itest_8", TileType.Wall);
            case ')':
                return new Tile("Sprites/Tiles/spr_wall_itest_9", TileType.Wall);
            case '1':
                return LoadPlayer(x, y);
            default:
                return new Tile("");

        }
    }

    public Tile LoadPlayer(int x, int y)
    {
        LevelGrid tiles = Find("tiles") as LevelGrid;
        Vector2 startPosition = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y +0.5f) * tiles.CellHeight);
        Player player = new Player();
        Add(player);
        player.MovePositionOnGrid(x, y);
        return new Tile("Sprites/Tiles/spr_floor_itest_1", TileType.Floor);
    }
}
