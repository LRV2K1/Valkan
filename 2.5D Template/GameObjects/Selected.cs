using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


class Selected : SpriteGameObject
{
    protected string selectedentity;

    public Selected(int layer = 1, string id = "")
        : base("Sprites/Player/spr_selected", layer, id)
    {
        selectedentity = "";
        origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (selectedentity != "")
        {
            position = GameWorld.GetObject(selectedentity).GlobalPosition;
        }
    }

    public string SelectedEntity
    {
        get { return selectedentity; }
        set { selectedentity = value; }
    }
}

