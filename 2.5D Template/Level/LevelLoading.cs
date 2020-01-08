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

        Camera camera = new Camera("player",0, "camera");
        RootList.Add(camera);

        GameMouse mouse = new GameMouse();
        RootList.Add(mouse);

        LoadFile(path);

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

    public void LoadTiles(List<string> textlines, int width, Dictionary<char, string> tiletypechar)
    {
        LevelGrid level = new LevelGrid(width, textlines.Count, 0, "tiles");
        RootList.Add(level);
        level.CellWidth = 108;
        level.CellHeight = 54;

        Camera camera = GetObject("camera") as Camera;
        camera.Width = (width) * level.CellWidth/2;
        camera.Height = (textlines.Count) * level.CellHeight;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < textlines.Count; y++)
            {
                Tile t;
                if (textlines[y][x] == '1')
                {
                    t = LoadPlayer(x, y);
                }
                else
                {
                    t = LoadTile(x, y, tiletypechar[textlines[y][x]]);
                }
                level.Add(t, x, y);
            }
        }
    }

    public Tile LoadTile(int x, int y, string tiletype)
    {
        string[] type = tiletype.Split(',');
        string asset = type[0];
        TileType tp = TileType.Background;
        TextureType tt = TextureType.None;

        switch (type[1])
        {
            case "Floor":
                tp = TileType.Floor;
                break;
            case "Background":
                tp = TileType.Background;
                break;
            case "Wall":
                tp = TileType.Wall;
                break;
        }

        switch (type[2])
        {
            case "None":
                tt = TextureType.None;
                break;
            case "Grass":
                tt = TextureType.Grass;
                break;
            case "Water":
                tt = TextureType.Water;
                break;
        }

        switch (type[3])
        {
            case "Tile":
                return new Tile(new Point(x, y), asset, tp, tt);
            case "WallTile":
                return new WallTile(new Point(x, y), asset, tp, tt);
            case "TreeTile":
                return new TreeTile(new Point(x, y), asset, tp, tt);
        }

        return new Tile(new Point(x, y));
    }

    public Tile LoadPlayer(int x, int y)
    {
        LevelGrid tiles = GetObject("tiles") as LevelGrid;
        Player player = new Player("1");
        Console.WriteLine(player.Height.ToString());
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
