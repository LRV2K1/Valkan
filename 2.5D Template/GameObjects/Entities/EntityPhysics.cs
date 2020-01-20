using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

abstract partial class Entity : AnimatedGameObject
{
    //physics and collision handeling for entities
    private void DoPhysics()
    {
        OutsideLevel();
        if (remove)
        {
            return;
        }
        HandleCollisions();
    }

    private void OutsideLevel()
    {
        LevelGrid tiles = GameWorld.GetObject("levelgrid") as LevelGrid;
        Vector2 loc = tiles.GridPosition(position);
        Tile currentTile = tiles.Get((int)loc.X, (int)loc.Y) as Tile;
        if (currentTile == null)
        {
            RemoveSelf();
        }
    }

    protected virtual void HandleCollisions()
    {
        LevelGrid tiles = GameWorld.GetObject("levelgrid") as LevelGrid;
        //check surrounding tiles
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
                            //check tile passenger collision
                            HandleEntityCollisions(currentTile.Passengers[i]);
                        }
                    }
                    //check collision
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

                //mouve position
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

    protected void HandleEntityCollisions(string id)
    {
        //check entity collision
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
            //move position
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

    public List<string> GetSurroundingTiles()
    {
        List<string> surroudingtiles = new List<string>();

        LevelGrid tiles = GameWorld.GetObject("levelgrid") as LevelGrid;

        for (int x = (int)gridPos.X - 3; x <= (int)gridPos.X + 3; x++)
        {
            for (int y = (int)gridPos.Y - 3; y <= (int)gridPos.Y + 3; y++)
            {
                Tile currentTile = tiles.Get(x, y) as Tile;

                if (currentTile != null)
                {
                    surroudingtiles.Add(currentTile.Id);
                }
            }
        }
        return surroudingtiles;
    }

    public List<string> GetSurroundingEntities()
    {
        //includes self
        List<string> surroundingentities = new List<string>();
        List<string> surroundingtiles = GetSurroundingTiles();

        LevelGrid tiles = GameWorld.GetObject("levelgrid") as LevelGrid;

        foreach (string id in surroundingtiles)
        {
            Tile tile = GameWorld.GetObject(id) as Tile;

            for (int i = 0; i < tile.Passengers.Count; i++)
            {
                surroundingentities.Add(tile.Passengers[i]);
            }
        }
        return surroundingentities;
    }
}
