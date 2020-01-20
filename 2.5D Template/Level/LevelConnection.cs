using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

partial class Level : GameObjectLibrary
{
    Dictionary<string, string> connectedEntities;
    string previousdata;
    private void SetupClient()
    {
        previousdata = "";
        connectedEntities = new Dictionary<string, string>();
        if (MultiplayerManager.online)
        {
            previousdata = MultiplayerManager.party.GetReceivedData();
        }
    }

    private void DistributeData()
    {

        if (previousdata != MultiplayerManager.party.GetReceivedData())
        {
            previousdata = MultiplayerManager.party.GetReceivedData();
            string[] variables = MultiplayerManager.party.GetReceivedData().Split(' '); //split data in Type, ID, posX, posY respectively
            if (variables[0] == "Entity:")
            {
                if (connectedEntities.ContainsKey(variables[1]))
                {
                    (GetObject(connectedEntities[variables[1]]) as ConnectedEntity).ReceiveData(previousdata);
                }
                else
                {
                    AddConnectedEntity(previousdata, variables[1]);
                }
            }
            if (variables[0] == "Camera:")
            {
                (GetObject("camera") as Camera).GetData(previousdata);
            }
        }
    }

    public void AddConnectedEntity(string data, string id)
    {
        Console.WriteLine("make object");
        ConnectedEntity entity = new ConnectedEntity(data);
        RootList.Add(entity);
        //(GetObject("entities") as GameObjectList).Add(entity);
        connectedEntities.Add(id, entity.Id);
        //SpriteGameObject test = new SpriteGameObject(entity.Sprite.AssetName, 102);
        //RootList.Add(test);
    }

    public void RemoveConnectedEntity(string connectedid)
    {
        connectedEntities.Remove(connectedid);
    }
}
