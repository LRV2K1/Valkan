using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class ConnectedEntity : Entity
{
    string connectedid;
    Vector2 connectedOrigin;
    string previousdata;

    Dictionary<string, string> animations;
    int animationNumber;

    public ConnectedEntity(string data)
        : base(0)
    {
        animations = new Dictionary<string, string>();
        animationNumber = 0;
        previousdata = "";
        ReceiveData(data);
        connectedOrigin = Vector2.Zero;
    }

    protected override void SendData() { }

    public void ReceiveData(string data)
    {
        if (data == previousdata)
        {
            return;
        }
        previousdata = data;

        string[] splitdata = data.Split(' ');

        connectedid = splitdata[1];
        position = new Vector2(float.Parse(splitdata[2]), float.Parse(splitdata[3]));
        connectedOrigin = new Vector2(float.Parse(splitdata[4]), float.Parse(splitdata[5]));
        origin = connectedOrigin;
        if (Current != null)
        {
            if (splitdata[6] != Current.AssetName)
            {
                if (animations.ContainsKey(splitdata[6]))
                {
                    SwitchAnimation(splitdata[6]);
                }
                else
                {
                    NewAnimation(splitdata[6], splitdata[7], splitdata[8]);
                }
            }
        }
        else
        {
            NewAnimation(splitdata[6], splitdata[7], splitdata[8]);
        }
    }

    private void SwitchAnimation(string animation)
    {
        PlayAnimation(animations[animation]);
    }

    private void NewAnimation(string animation, string islooping, string isbackandforth)
    {
        bool isLooping = bool.Parse(islooping);
        bool isBackAndForth = bool.Parse(isbackandforth);
        animations.Add(animation, animationNumber.ToString());
        animationNumber++;
        LoadAnimation(animation, animations[animation], isLooping, isBackAndForth);
        SwitchAnimation(animation);
    }

    public override void PlayAnimation(string id, bool isBackWards = false)
    {
        base.PlayAnimation(id, isBackWards);
        origin = connectedOrigin;
    }

    protected override void HandleCollisions() { }

    public override void RemoveSelf()
    {
        base.RemoveSelf();
        (GameWorld as Level).Remove(connectedid);
    }
}