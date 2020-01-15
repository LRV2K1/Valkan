using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class OverlayStatus : GameObjectList
{
    //shows current selected overlay
    Dictionary<string, string> status;
    TextButton save;
    string activestatus;
    int height;
    public OverlayStatus(GameObjectLibrary gameworld ,int layer = 101, string id = "overlay")
        : base(layer, id)
    {
        height = 100;
        GameWorld = gameworld;
        status = new Dictionary<string, string>();
        activestatus = "";

        save = new TextButton("Fonts/Hud", "save", 102);
        save.Color = Color.Red;
        save.Position = new Vector2(30, 20);
        Add(save);

        Add(new SpriteGameObject("Sprites/Menu/spr_overlay2", 101));
        SpriteGameObject overlay1 = new SpriteGameObject("Sprites/Menu/spr_overlay1", 101);
        overlay1.Position = new Vector2(0, 830);
        Add(overlay1);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (save.Pressed)
        {
            LevelEditor.Save();
        }
    }

    public void AddStatus(string key, GameObject obj)
    {
        Add(obj);
        obj.Visible = false;
        obj.Position = new Vector2(0, 830);
        status.Add(key, obj.Id);
        OverlayButton button = new OverlayButton(key, 102);
        button.Position = new Vector2(30, height);
        button.Color = Color.Black;
        Add(button);
        height += 30;
    }
    
    public void ActiveStatus(string key)
    {
        if (status.ContainsKey(key))
        {
            if (activestatus != "")
            {
                GameWorld.GetObject(activestatus).Visible = false;
            }
            activestatus = status[key];
            GameWorld.GetObject(activestatus).Visible = true;
        }
    }
}