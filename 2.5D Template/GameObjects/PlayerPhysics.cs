using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


partial class Player : AnimatedGameObject
{
    private void DoPhysics()
    {
        HandleCollisions();
    }


    private void HandleCollisions()
    {
        LevelGrid tiles = GameWorld.Find("tiles") as LevelGrid;
        Vector2 gridPos = tiles.GridPosition(position);
        for (int x = (int)gridPos.X - 2; x <= (int)gridPos.X + 2; x++)
        {
            for (int y = (int)gridPos.Y - 2; y <= (int)gridPos.Y + 2; y++)
            {
                TileType tileType = tiles.GetTileType(x, y);
                Tile currentTile = tiles.Get(x, y) as Tile;
                if (tileType == TileType.Floor)
                {
                    continue;
                }

                int left = (int)(GlobalPosition.X - GetAnimation("boundingbox").Width / 2);
                int top = (int)(GlobalPosition.Y - GetAnimation("boundingbox").Height / 2);
                Vector2 tilePos = new Vector2(x * tiles.CellWidth / 2 - tiles.CellWidth / 2 * y, y * tiles.CellHeight / 2 + tiles.CellHeight / 2 * x);
                Rectangle tileBounds = new Rectangle((int)(tilePos.X - tiles.CellWidth / 2), (int)(tilePos.Y - tiles.CellHeight / 2), tiles.CellWidth, tiles.CellHeight);
                Rectangle boundingBox = new Rectangle(left, top, GetAnimation("boundingbox").Width, GetAnimation("boundingbox").Height);

                if (!tileBounds.Intersects(boundingBox))
                {
                    continue;
                }

                Vector2 depth = Collision.CalculateIntersectionDepth(boundingBox, tileBounds);
                if (Math.Abs(depth.X) < Math.Abs(depth.Y))
                {
                    position.X += depth.X;
                    continue;
                }
                position.Y += depth.Y;
            }
        }
    }
}