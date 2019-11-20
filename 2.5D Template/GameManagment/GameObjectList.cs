using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameObjectList : GameObject
{
    protected List<string> children;

    public GameObjectList(int layer = 0, string id = "") : base(layer, id)
    {
        children = new List<string>();
    }

    public List<string> Children
    {
        get { return children; }
    }

    public virtual void Add(GameObject obj)
    {
        GameWorld.Add(obj);
        obj.Parent = this;
        for (int i = 0; i < children.Count; i++)
        {
            if (GameWorld.GetObject(children[i]).Layer > obj.Layer)
            {
                children.Insert(i, obj.Id);
                return;
            }
        }
        children.Add(obj.Id);
    }

    public void RemoveFromList(string id)
    {
        children.Remove(id);
        GameWorld.GetObject(id).Parent = null;
    }

    public void Remove(string id)
    {
        children.Remove(id);
        GameWorld.GetObject(id).Parent = null;
        GameWorld.Remove(id);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        for (int i = 0; i < children.Count; i++)
        {
            GameWorld.GetObject(children[i]).HandleInput(inputHelper);
        }
    }

    public override void Update(GameTime gameTime)
    {
        foreach (string id in children)
        {
            GameWorld.GetObject(id).Update(gameTime);
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!visible)
        {
            return;
        }
        List<string>.Enumerator e = children.GetEnumerator();
        while (e.MoveNext())
        {
            GameWorld.GetObject(e.Current).Draw(gameTime, spriteBatch);
        }
    }

    public override void Reset()
    {
        base.Reset();
        foreach (string id in children)
        {
            GameWorld.GetObject(id).Reset();
        }
    }
}
