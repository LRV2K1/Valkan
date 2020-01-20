using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class OverlayManager : GameObject
{
    Dictionary<string, GameObject> overlays;
    GameObject currentOverlay;
    string currentid;

    //updates current overlay
    public OverlayManager(int layer = 101, string id = "overlay")
        : base(layer, id)
    {
        overlays = new Dictionary<string, GameObject>();
        currentOverlay = null;
        currentid = "";
    }

    public void AddOverlay(string name, GameObject overlay)
    {
        overlay.Parent = this;
        overlays[name] = overlay;
    }

    public GameObject GetOverlay(string name)
    {
        return overlays[name];
    }

    public void SwitchTo(string name)
    {
        if (overlays.ContainsKey(name))
        {
            currentOverlay = overlays[name];
            currentid = name;
            currentOverlay.Reset();
        }
        else
        {
            //throw new KeyNotFoundException("Could not find game state: " + name);
        }
    }

    public GameObject CurrentOverlay
    {
        get
        {
            return currentOverlay;
        }
    }

    public string CurrentOverlayID
    {
        get
        {
            return currentid;
        }
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (currentOverlay != null)
        {
            currentOverlay.HandleInput(inputHelper);
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (currentOverlay != null)
        {
            currentOverlay.Update(gameTime);
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentOverlay != null)
        {
            currentOverlay.Draw(gameTime, spriteBatch);
        }
    }

    public override void Reset()
    {
        if (currentOverlay != null)
        {
            currentOverlay.Reset();
        }
    }
}

