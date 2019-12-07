using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class Menu : GameObject
{
    protected int menuNumber; //Indicates which menu screen should be displayed.
    protected bool isLoaded;
    public Menu() :
        base(0, "menu")
    {
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if(!isLoaded) //Only go through this piece of code if the menu isn't loaded yet.
        {
            switch (menuNumber) //Check which menu should be loaded
            {
                case 0: //Load Main Menu
                    break;
                case 1: //Load Settings Menu
                    break;
                case 2: //Load Credits Menu
                    break;
                case 3: //In-Game Menu
                    break;
                case 4: //In-Game Settings Menu
                    break;
            }
            //Drie

        }
    }


}
