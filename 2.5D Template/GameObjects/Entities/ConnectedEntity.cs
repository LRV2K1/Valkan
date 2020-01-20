using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class ConnectedEntity : Entity
{
    int count;
    int count2;
    string connectedid;
    Vector2 connectedOrigin;

    Dictionary<string, string> animations;
    int animationNumber;

    public ConnectedEntity(string data)
        : base(0)
    {
        animations = new Dictionary<string, string>();
        animationNumber = 0;
        ReceiveData(data);
    }

    protected override void SendData() { }

    public void ReceiveData(string data)
    {
        Console.WriteLine("get data");
        Console.WriteLine(data);
        //Camera camera = GameWorld.GetObject("camera") as Camera;
        //position = camera.CameraPosition;

        string[] splitdata = data.Split(' ');

        connectedid = splitdata[1];
        Console.WriteLine("set id data");
        position = new Vector2(float.Parse(splitdata[2]), float.Parse(splitdata[3]));
        connectedOrigin = new Vector2(float.Parse(splitdata[4]), float.Parse(splitdata[5]));
        origin = connectedOrigin;
        Console.WriteLine("set position data");
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