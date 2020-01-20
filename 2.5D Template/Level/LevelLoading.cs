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
        overlayManager.AddOverlay("menu", new InGameMenu(this));
        overlayManager.AddOverlay("die", new Die(this, "Sprites/Overlay/spr_die"));
        overlayManager.AddOverlay("finish", new Die(this, "Sprites/Overlay/spr_finish"));
        overlayManager.SwitchTo("hud");
    }

    private void LoadTiles(List<string> textlines, int width, Dictionary<char, string> tiletypechar)
    {
        LevelGrid level = new LevelGrid(width, textlines.Count, 0, "levelgrid");
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
            case "GrassTile":
                return new GrassTile(new Point(x, y), asset, tp, tt);
        }

        return new Tile(new Point(x, y));
    }

    private void LoadEntities(List<string> textlines, int width, Dictionary<char, string> entitytypechar)
    {
        if (MultiplayerManager.online)
        {
            if (GameEnvironment.GameSettingsManager.GetValue("host") == "false")
            {
                LoadRemotePlayer();
                return;
            }
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < textlines.Count; y++)
            {
                try
                {
                    LoadEntity(x, y, entitytypechar[textlines[y][x]]);
                }
                catch
                {
                    LoadEntity(x, y, "None");
                }
            }
        }
    }

    private void LoadRemotePlayer()
    {
        foreach (LobbyPlayer lobbyplayer in MultiplayerManager.party.playerlist.playerlist)
        {
            if (lobbyplayer.ishost == false)
            {
                ConnectedPlayer connectedPlayer;
                PlayerType playerType = (PlayerType)Enum.Parse(typeof(PlayerType), GameEnvironment.GameSettingsManager.GetValue("character"));
                switch (playerType)
                {
                    case PlayerType.Bard:
                        connectedPlayer = new ConnectedBard();
                        break;
                    case PlayerType.Warrior:
                        connectedPlayer = new ConnectedWarrior();
                        break;
                    case PlayerType.Wizzard:
                        connectedPlayer = new ConnectedWizzard();
                        break;
                    default:
                        connectedPlayer = new ConnectedWarrior();
                        break;
                }
                RootList.Add(connectedPlayer);
                connectedPlayer.PlayerSetup();
                return;
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
        Player player;
        try
        {
            PlayerType playerType = (PlayerType)Enum.Parse(typeof(PlayerType), GameEnvironment.GameSettingsManager.GetValue("character"));
            switch (playerType)
            {
                case PlayerType.Bard:
                    player = new Bard();
                    break;
                case PlayerType.Warrior:
                    player = new Warrior();
                    break;
                case PlayerType.Wizzard:
                    player = new Wizzard();
                    break;
                default:
                    player = new Warrior();
                    break;
            }
        }
        catch
        {
            player = new Warrior();
        }
        GameObjectList entities = GetObject("entities") as GameObjectList;
        entities.Add(player);
        player.SetupPlayer();
        player.MovePositionOnGrid(x, y);

        if (MultiplayerManager.online)
        {
            foreach (LobbyPlayer lobbyplayer in MultiplayerManager.party.playerlist.playerlist)
            {
                if (lobbyplayer.ishost == false)
                {
                    Player player2;
                    PlayerType playerType = (PlayerType)Enum.Parse(typeof(PlayerType), GameEnvironment.GameSettingsManager.GetValue("character"));
                    switch (playerType)
                    {
                        case PlayerType.Bard:
                            player2 = new Bard();
                            break;
                        case PlayerType.Warrior:
                            player2 = new Warrior();
                            break;
                        case PlayerType.Wizzard:
                            player2 = new Wizzard();
                            break;
                        default:
                            player2 = new Warrior();
                            break;
                    }
                    entities.Add(player2);
                    player2.SetupPlayer();
                    player2.MovePositionOnGrid(x, y + 1);
                    return;
                }
            }
        }

        /*
        if (MultiplayerManager.online && false)
        {
            foreach (LobbyPlayer lobbyplayer in MultiplayerManager.party.playerlist.playerlist)
            {
                if (lobbyplayer.ishost == false)
                {
                    Item item = new Item(id: "player2");
                    GameObjectList items = GetObject("items") as GameObjectList;
                    entities.Add(item);
                    item.MovePositionOnGrid(50, 50);
                }
            }
        }
        */
    }

    private void LoadItem(int x, int y, string asset, int boundingy, bool animated, string it)
    {
        ItemType type = (ItemType)Enum.Parse(typeof(ItemType), it);
        Item item = new Item(asset, animated, type, boundingy);
        GameObjectList entities = GetObject("entities") as GameObjectList;
        GameObjectList items = GetObject("items") as GameObjectList;
        items.Add(item);
        item.MovePositionOnGrid(x, y);
    }

    private void LoadEnemy(int x, int y, string asset, int boundingy)
    {
        Enemy enemy = new Enemy(asset, boundingy);
        if (enemy.NoData)
        {
            return;
        }
        GameObjectList enemies = GetObject("enemies") as GameObjectList;
        enemies.Add(enemy);
        enemy.MovePositionOnGrid(x, y);
        enemycount++;
    }
}