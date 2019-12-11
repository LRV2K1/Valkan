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
        Vector2 vpos = levelGrid.GridPosition(mousePos + new Vector2(0, levelGrid.CellHeight / 2));
        Point gridpos = new Point((int)vpos.X + 1, (int)vpos.Y + 1);

        string entity = "";
        float closedistance = 100f;

        for (int x = gridpos.X - 1; x <= gridpos.X + 1; x++)
        {
            for (int y = gridpos.Y -1; y <= gridpos.Y + 1; y++)
            {
                Tile tile = levelGrid.Get(x, y) as Tile;
                if (tile == null)
                {
                    continue;
                }
                for (int i = 0; i < tile.Passengers.Count; i++)
                {
                    Enemy enemy = GameWorld.GetObject(tile.Passengers[i]) as Enemy;
                    if (enemy == null)
                    {
                        continue;
                    }
                    if (enemy.OnSprite(mousePos))
                    {
                        float xd = mousePos.X - enemy.GlobalPosition.X;
                        float yd = mousePos.Y - enemy.GlobalPosition.Y;
                        float distance = (float)Math.Sqrt(xd*xd+yd*yd);
                        if (distance < closedistance)
                        {
                            entity = enemy.Id;
                            closedistance = distance;
                        }
                    }
                }
            }
        }
        return entity;
    }

    public Vector2 MousePos
    {
        get { return mousePos; }
    }
}