using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


class ConnectedPlayer : GameObject
{
    public ConnectedPlayer(string id = "player2")
        : base(0, id)
    {

    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.A))
        {
            MultiplayerManager.party.Send("Player: " + id + " " + "left" + " " + "true", 9999, false);
        }
        else if (inputHelper.KeyReleased(Keys.A))
        {
            MultiplayerManager.party.Send("Player: " + id + " " + "left" + " " + "false", 9999, false);
        }

        if (inputHelper.KeyPressed(Keys.D))
        {
            MultiplayerManager.party.Send("Player: " + id + " " + "right" + " " + "true", 9999, false);
        }
        else if (inputHelper.KeyReleased(Keys.D))
        {
            MultiplayerManager.party.Send("Player: " + id + " " + "right" + " " + "false", 9999, false);
        }

        if (inputHelper.KeyPressed(Keys.W))
        {
            MultiplayerManager.party.Send("Player: " + id + " " + "up" + " " + "true", 9999, false);
        }
        else if (inputHelper.KeyReleased(Keys.W))
        {
            MultiplayerManager.party.Send("Player: " + id + " " + "up" + " " + "false", 9999, false);
        }

        if (inputHelper.KeyPressed(Keys.S))
        {
            MultiplayerManager.party.Send("Player: " + id + " " + "down" + " " + "true", 9999, false);
        }
        else if (inputHelper.KeyReleased(Keys.S))
        {
            MultiplayerManager.party.Send("Player: " + id + " " + "down" + " " + "false", 9999, false);
        }
    }
}