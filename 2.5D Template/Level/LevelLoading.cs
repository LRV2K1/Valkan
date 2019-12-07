using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;

partial class Level : GameObjectLibrary
{
    public void LoadLevel(string path)
    {
        GameObjectList entities = new GameObjectList(2, "entities");
        RootList.Add(entities);

        GameObjectList items = new GameObjectList(1, "items");
        entities.Add(items);

        Camera camera = new Camera("player", 0, "camera");
        RootList.Add(camera);

        GameMouse mouse = new GameMouse();
        RootList.Add(mouse);

        List<string> textLines = new List<string>();
        StreamReader streamReader = new StreamReader(path);

        string line = streamReader.ReadLine();
        int width = line.Length;
        while (line != null)
        {
            textLines.Add(line);
            line = streamReader.ReadLine();
        }

        LoadTiles(textLines, width);

        LoadOverlays();
    }

    public void LoadOverlays()
    {
        OverlayManager overlayManager = new OverlayManager();
        RootList.Add(overlayManager);

        overlayManager.AddOverlay("hud", new Hud(this));
        overlayManager.AddOverlay("inventory", new Inventory(this));

        overlayManager.SwitchTo("hud");
    }

    public void LoadTiles(List<string> textlines, int width)
    {
        LevelGrid level = new LevelGrid(width, textlines.Count, 0, "tiles");
        RootList.Add(level);
        level.CellWidth = 108;
        level.CellHeight = 54;

        Camera camera = GetObject("camera") as Camera;
        camera.Width = (width) * level.CellWidth / 2;
        camera.Height = (textlines.Count) * level.CellHeight;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < textlines.Count; y++)
            {
                Tile t = LoadTile(textlines[y][x], x, y);
                level.Add(t, x, y);
            }
        }
    }

    public Tile LoadTile(char tileType, int x, int y)
    {
        switch (tileType)
        {
            case '.':
                return new Tile(new Point(x, y), "Sprites/Tiles/spr_grass_sheet_0@4x4", TileType.Floor, TextureType.Grass);
            //return new Tile(new Point(x,y), "Sprites/Tiles/spr_floor_sheet_test_1@4x4", TileType.Floor, TextureType.Grass);
            //return new Tile(new Point(x, y));
            case 'w':
                //return new Tile(new Point(x, y), "Sprites/Tiles/spr_floor_itest_2", TileType.Wall, TextureType.Water);
                return new Tile(new Point(x, y), "Sprites/Tiles/spr_water_0", TileType.Wall, TextureType.Water);
            case '#':
                return new Tile(new Point(x, y), "Sprites/Tiles/spr_wall_itest_1", TileType.Wall);
            case '@':
                //return new WallTile(new Point(x, y), "Sprites/Tiles/spr_wall_sheet_test_1@5x4");
                return new WallTile(new Point(x, y), "Sprites/Tiles/spr_brick_wall_sheet_1@4x4");
            case '%':
                return new WallTile(new Point(x, y), "Sprites/Tiles/spr_wood_wall_sheet_0@5");
            case '+':
                return LoadTree(x, y);
            case 'i':
                return LoadItem(x, y);
            case '1':
                return LoadPlayer(x, y);
            case 'r':
                return new Tile(new Point(x, y));
            default:
                return new Tile(new Point(x, y));

        }
    }

    public Tile LoadTree(int x, int y)
    {
        int type = GameEnvironment.Random.Next(0, 5);
        return new Tile(new Point(x, y), "Sprites/Tiles/Trees/spr_tree_" + type, TileType.Wall);
        //return new Tile(new Point(x, y), "Sprites/Tiles/spr_tree_itest_1", TileType.Wall);
    }

    public Tile LoadPlayer(int x, int y)
    {            
        LevelGrid tiles = GetObject("tiles") as LevelGrid;
        Player player = new Player();
        GameObjectList entities = GetObject("entities") as GameObjectList;
        entities.Add(player);
        player.MovePositionOnGrid(x, y);
        //return new Tile(new Point(x, y), "Sprites/Tiles/spr_floor_sheet_test_1@4x4", TileType.Floor, TextureType.Grass);
        return new Tile(new Point(x, y), "Sprites/Tiles/spr_grass_sheet_0@4x4", TileType.Floor, TextureType.Grass);
    }

    public Tile LoadItem(int x, int y)
    {
        LevelGrid tiles = GetObject("tiles") as LevelGrid;
        Item item = new Item();
        GameObjectList entities = GetObject("entities") as GameObjectList;
        GameObjectList items = GetObject("items") as GameObjectList;
        items.Add(item);
        item.MovePositionOnGrid(x, y);
        //return new Tile(new Point(x, y), "Sprites/Tiles/spr_floor_sheet_test_1@4x4", TileType.Floor, TextureType.Grass);
        return new Tile(new Point(x, y), "Sprites/Tiles/spr_grass_sheet_0@4x4", TileType.Floor, TextureType.Grass);
    }
}