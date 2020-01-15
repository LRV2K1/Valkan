using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

partial class LevelEditer : GameObjectLibrary
{
    public void NewLevel(int width, int height)
    {
        LevelGrid levelGrid = new LevelGrid(width, height, 0, "levelgrid");
        RootList.Add(levelGrid);
        levelGrid.CellWidth = 108;
        levelGrid.CellHeight = 54;
        levelGrid.SetupGrid();

        ItemGrid itemGrid = new ItemGrid(width, height, 1, "itemgrid");
        RootList.Add(itemGrid);
        itemGrid.CellWidth = 108;
        itemGrid.CellHeight = 54;
        itemGrid.SetupGrid();
    }

    private void LoadOverlay()
    {
        RootList.Add(new EditorMouse());

        OverlayStatus overlay = new OverlayStatus(this);
        RootList.Add(overlay);

        overlay.AddStatus("Floor", new TileOverlay(this, "Content/Editor/Tiles/Floor.txt"));
        overlay.AddStatus("Wall", new TileOverlay(this, "Content/Editor/Tiles/Wall.txt"));
        overlay.AddStatus("Cave", new TileOverlay(this, "Content/Editor/Tiles/Cave.txt"));
        overlay.AddStatus("Tree", new TileOverlay(this, "Content/Editor/Tiles/Tree.txt"));
        overlay.AddStatus("Items", new EntityOverlay(this, "Content/Editor/Entities/Item.txt"));
        overlay.AddStatus("Objects", new EntityOverlay(this, "Content/Editor/Entities/Object.txt"));
        overlay.AddStatus("Cave_Objects", new EntityOverlay(this, "Content/Editor/Entities/Cave_Object.txt"));
        overlay.AddStatus("Enemies", new EntityOverlay(this, "Content/Editor/Entities/Enemy.txt"));
        overlay.AddStatus("Spawn", new EntityOverlay(this, "Content/Editor/Entities/Spawn.txt"));

        LevelGrid levelGrid = GetObject("levelgrid") as LevelGrid;
        Camera camera = new Camera();
        camera.SetupCamera = levelGrid.AnchorPosition(5, 5) - GameEnvironment.Screen.ToVector2() / 2;
        RootList.Add(camera);
    }

    private void SetUp()
    {

    }

    private void LoadTiles(List<string> textlines, int width, Dictionary<char, string> tiletypechar)
    {
        LevelGrid level = new LevelGrid(width, textlines.Count, 0, "levelgrid");
        RootList.Add(level);
        level.CellWidth = 108;
        level.CellHeight = 54;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < textlines.Count; y++)
            {
                try
                {
                    Tile t = LoadTile(x, y, tiletypechar[textlines[y][x]]);
                    level.Add(t, x, y);
                }
                catch
                {
                    Tile t = LoadTile(x, y, tiletypechar['a']);
                    level.Add(t, x, y);
                }
            }
        }
    }

    private Tile LoadTile(int x, int y, string tiletype)
    {
        string[] type = tiletype.Split(',');
        string asset = type[0];
        TileType tp = (TileType)Enum.Parse(typeof(TileType), type[1]);
        TextureType tt = (TextureType)Enum.Parse(typeof(TextureType), type[2]);

        switch (type[3])
        {
            case "Tile":
                return new Tile(new Point(x, y), asset, tp, tt);
            case "WallTile":
                return new WallTile(new Point(x, y), asset, tp, tt);
            case "TreeTile":
                return new TreeTile(new Point(x, y), asset, tp, tt);
            case "GrassTile":
                return new GrassTile(new Point(x, y), asset, tp, tt);
        }

        return new Tile(new Point(x, y));
    }

    private void LoadEntities(List<string> textlines, int width, Dictionary<char, string> entitytypechar)
    {
        ItemGrid level = new ItemGrid(width, textlines.Count, 0, "itemgrid");
        RootList.Add(level);
        level.CellWidth = 108;
        level.CellHeight = 54;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < textlines.Count; y++)
            {
                try
                {
                    EditorEntity e = LoadEntity(x, y, entitytypechar[textlines[y][x]]);
                    level.Add(e, x, y);
                }
                catch
                {
                    EditorEntity e = LoadEntity(x, y, "None");
                    level.Add(e, x, y);
                }
            }
        }
    }

    private EditorEntity LoadEntity(int x, int y, string entitytype)
    {
        string[] type = entitytype.Split(',');
        if (type[0] == "None")
        {
            return new EditorEntity(new Point(x, y));
        }

        string asset = type[0];
        int boundingy = int.Parse(type[1]);
        EntityType et = (EntityType)Enum.Parse(typeof(EntityType), type[2]);
        EditorEntity entity = new EditorEntity(new Point(x, y), asset, boundingy, et);
        //check for additional information
        if (et == EntityType.Enemy)
        {
            entity.EnemyType = (EnemyType)Enum.Parse(typeof(EnemyType), type[3]);
        }
        if (et == EntityType.SpriteItem || et == EntityType.AnimatedItem)
        {
            entity.ItemType = (ItemType)Enum.Parse(typeof(ItemType), type[3]);
        }
        return entity;
    }
}