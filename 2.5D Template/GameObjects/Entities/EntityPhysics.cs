using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

abstract partial class Entity : AnimatedGameObject
{
    private void DoPhysics()
    {
        HandleCollisions();
    }


    private void HandleCollisions()
    {
        LevelGrid tiles = GameWorld.GetObject("tiles") as LevelGrid;
        Vector2 gridPos = tiles.GridPosition(position);
        for (int x = (int)gridPos.X - 2; x <= (int)gridPos.X + 2; x++)
        {
            for (int y = (int)gridPos.Y - 2; y <= (int)gridPos.Y + 2; y++)
            {
                TileType tileType = tiles.GetTileType(x, y);
                Tile currentTile = tiles.Get(x, y) as Tile;

                Vector2 tilePos = new Vector2(x * tiles.CellWidth / 2 - tiles.CellWidth / 2 * y, y * tiles.CellHeight / 2 + tiles.CellHeight / 2 * x);
                Rectangle tileBounds;    
                if (currentTile == null)
                {
                    tileBounds = new Rectangle((int)(tilePos.X - tiles.CellWidth / 2), (int)(tilePos.Y - tiles.CellHeight / 2), tiles.CellWidth, tiles.CellHeight);
                }
                else
                {
                    for (int i = 0; i < currentTile.Passengers.Count; i++)
                    {
                        if (currentTile.Passengers[i] != id)
                        {
                            HandleEntityCollisions(currentTile.Passengers[i]);
                        }
                    }
                    if (tileType == TileType.Floor)
                    {
                        continue;
                    }
                    tileBounds = currentTile.GetBoundingBox();
                }


                if (!tileBounds.Intersects(BoundingBox))
                {
                    continue;
                }

                Vector2 depth = Collision.CalculateIntersectionDepth(BoundingBox, tileBounds);
                if (Math.Abs(depth.X) < Math.Abs(depth.Y))
                {
                    position.X += depth.X;
                    continue;
                }
                position.Y += depth.Y;
            }
        }
    }

    private void HandleEntityCollisions(string id)
    {
        Entity entity = GameWorld.GetObject(id) as Entity;
        if (entity == null || !BoundingBox.Intersects(entity.BoundingBox))
        {
            return;
        }

        Vector2 depth = Collision.CalculateIntersectionDepth(BoundingBox, entity.BoundingBox);
        int totalWeight = weight + entity.Weight;
        float push = ((float)weight / (float)totalWeight);
        Item item = entity as Item;
        if (Math.Abs(depth.X) < Math.Abs(depth.Y))
        {
            position.X += depth.X;
            if (item != null && item.ItemType == ItemType.InMovible)
            {
                return;
            }
            entity.position -= new Vector2(depth.X * push, 0);
            return;
        }
        position.Y += depth.Y;
        if (item != null && item.ItemType == ItemType.InMovible)
        {
            return;
        }
        entity.position -= new Vector2(0, depth.Y * push);
    }
}
