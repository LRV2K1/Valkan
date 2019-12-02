using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class GameMouse : SpriteGameObject
{
    Vector2 mousePos;
    public GameMouse()
        : base("Sprites/Menu/spr_mouse",200, "mouse")
    {
          
    }
    
    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        Camera camera = GameWorld.GetObject("camera") as Camera;
        mousePos = inputHelper.MousePosition + camera.CameraPosition;
        position = inputHelper.MousePosition;
    }

    public string CeckEntitySelected()
    {
        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
        Vector2 gridpos = levelGrid.GridPosition(mousePos + new Vector2(0, levelGrid.CellHeight / 2));
        Point GridPos = new Point((int)gridpos.X, (int)gridpos.Y);
        Tile tile = levelGrid.Get(GridPos.X + 1, GridPos.Y + 1) as Tile;
        if (tile != null)
        {
            for (int i = 0; i < tile.Passengers.Count; i++)
            {
                if (GameWorld.GetObject(tile.Passengers[i]) is Item)
                {
                    return tile.Passengers[0];
                }
            }
        }
        return "";
    }

    public Vector2 MousePos
    {
        get { return mousePos; }
    }
}

