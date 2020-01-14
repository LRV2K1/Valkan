using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class MapSelectionPopUp : PopUp
{
    Button selectionButton1, selectionButton2, selectionButton3, selectionButton4, selectionButton5, selectionButton6, selectionButton7, selectionButton8, selectionButton9;
    List<Button> buttonList;
    public MapSelectionPopUp(string assetname, Vector2 boxSize, int layer = 108, string id = "mapSelection") :
        base(assetname, boxSize, layer, id)
    {
        this.Sprite.Size = boxSize;
        this.Origin = Center;
        this.Sprite.Color = Color.Red;
        this.Position = new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2);
        buttonList = new List<Button>(){selectionButton1, selectionButton2, selectionButton3, selectionButton4, selectionButton5, selectionButton6, selectionButton7, selectionButton8, selectionButton9 };
    }

    public override void Update(GameTime gameTime)
    {
        if(!active)
        {
            this.Visible = false;
            for(int i = 0; i < 9; i++)
            {
                buttonList[i].Visible = false;
            }
        }
        else
        {
            this.Visible = true;
            for (int i = 0; i < 9; i++)
            {
                buttonList[i].Visible = true;
            }
        }
        base.Update(gameTime);
    }

    public void LoadButtons()
    {
        Vector2 startposition = new Vector2((GameEnvironment.Screen.X / 30) * 8, (GameEnvironment.Screen.Y / 13) * 4);
        Vector2 newPosition;
        int num = 0;
        int xOffset = (int)(GameEnvironment.Screen.X / 30) * 5;
        int yOffset = (int)(GameEnvironment.Screen.Y / 4);
        for (int i = 0; i < 3; i++)
        {
            newPosition = new Vector2(startposition.X, startposition.Y + yOffset * i);
            for(int j = 0; j < 3; j++)
            {
                buttonList[num] = new Button("Sprites/Menu/Select_Button", 109);
                buttonList[num].Sprite.Size = new Vector2(0.6f,0.6f);
                buttonList[num].Position = new Vector2(startposition.X + j * xOffset, newPosition.Y);
                RootList.Add(buttonList[num]);
                num++;
            }
        }
    }

    public void ActivatePopup()
    {
        this.Visible = true;
    }

}
