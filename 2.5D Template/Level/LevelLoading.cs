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

        GameObjectList enemies = new GameObjectList(1, "enemies");
        entities.Add(enemies);

        Camera camera = new Camera("player",0, "camera");
        RootList.Add(camera);

        GameMouse mouse = new GameMouse();
        RootList.Add(mouse);

        LoadOverlays();

        LoadFile(path);
    }

    public void LoadOverlays()
    {
        OverlayManager overlayManager = new OverlayManager();
        RootList.Add(overlayManager);

        overlayManager.AddOverlay("hud", new Hud(this));
        overlayManager.AddOverlay("inventory", new Inventory(this));

        overlayManager.SwitchTo("hud");
    }

    private void LoadTiles(List<string> textlines, int width, Dictionary<char, string> tiletypechar)
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
                Tile t = LoadTile(x, y, tiletypechar[textlines[y][x]]);
                level.Add(t, x, y);
            }
        }
    }

    private Tile LoadTile(int x, int y, string tiletype)
    {
        string[] type = tiletype.Split(',');
        string asset = type[0];
        TileType tp = (TileType) Enum.Parse(typeof(TileType), type[1]);
        TextureType tt = (TextureType)Enum.Parse(typeof(TextureType), type[2]);

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

    private void LoadEntities(List<string> textlines, int width, Dictionary<char, string> entitytypechar)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < textlines.Count; y++)
            {
                LoadEntity(x, y, entitytypechar[textlines[y][x]]);
            }
        }
    }

    private void LoadEntity(int x, int y, string entitytype)
    {
        string[] type = entitytype.Split(',');
        if (type[0] == "None")
        {
            return;
        }

        string asset = type[0];
        int boundingy = int.Parse(type[1]);
        switch (type[2])
        {
            case "SpriteItem":
                LoadItem(x, y, asset, boundingy, false, type[3]);
                break;
            case "AnimatedItem":
                LoadItem(x, y, asset, boundingy, true, type[3]);
                break;
            case "Player":
                LoadPlayer(x, y);
                break;
            case "Enemy":
                LoadEnemy(x, y, asset, boundingy);
                break;
        }
    }

    private void LoadPlayer(int x, int y)
    {
        Player player = new Player();
        GameObjectList entities = GetObject("entities") as GameObjectList;
        entities.Add(player);
        player.SetupPlayer();
        player.MovePositionOnGrid(x, y);
    }

    private void LoadItem(int x, int y, string asset, int boundingy, bool animated, string et)
    {
        ItemType type = (ItemType)Enum.Parse(typeof(ItemType), et);
        Item item = new Item(asset, animated, type, boundingy);
        GameObjectList entities = GetObject("entities") as GameObjectList;
        GameObjectList items = GetObject("items") as GameObjectList;
        items.Add(item);
        //entities.Add(item);
        item.MovePositionOnGrid(x, y);
    }

    private void LoadEnemy(int x, int y, string asset, int boundingy)
    {
        Enemy enemy = new Enemy(asset, boundingy);
        GameObjectList enemies = GetObject("enemies") as GameObjectList;
        enemies.Add(enemy);
        enemy.MovePositionOnGrid(x, y);
    }
}
