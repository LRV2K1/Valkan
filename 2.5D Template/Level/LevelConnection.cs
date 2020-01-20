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
        try
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
                }
                else
                {
                    AddConnectedEntity(previousdata, variables[1]);
                }
            }
        }
        catch
        {

        }
    }

    public void AddConnectedEntity(string data, string id)
    {
        ConnectedEntity entity = new ConnectedEntity(data);
        (GameWorld.GetObject("entities") as GameObjectList).Add(entity);
        connectedEntities.Add(id, entity.Id);
    }

    public void RemoveConnectedEntity(string connectedid)
    {
        connectedEntities.Remove(connectedid);
    }
}
