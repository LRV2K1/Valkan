using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

public class GameObjectLibrary : GameObject
{
    //the gameobject library is also the gameworld
    protected GameObjectList root;
    protected Dictionary<string, GameObject> objects;

    public GameObjectLibrary(int layer = 0, string id = "")
        : base (layer, id)
    {
        //dictionary to save all objects with id
        objects = new Dictionary<string, GameObject>();

        //has its onw internal list
        root = new GameObjectList();
        root.GameWorld = this;
    }

    public virtual void Add(GameObject obj)
    {
        objects[obj.Id] = obj;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        root.HandleInput(inputHelper);
    }

    public override void Update(GameTime gameTime)
    {
        root.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        root.Draw(gameTime, spriteBatch);
    }

    public override void Reset()
    {
        base.Reset();
        root.Reset();
    }

    public override GameObjectList RootList
    {
        get { return root; }
    }

    public GameObject GetObject(string id)
    {
        if (objects.ContainsKey(id))
        {
            return objects[id];
        }
        return null;
    }

    public Dictionary<string, GameObject> Dictionary
    {
        get { return objects; }
    }

    public virtual void Remove(string id)
    {
        objects.Remove(id);
    }
}