﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class ConnectedEntity : Entity
{
    string connectedid;
    Vector2 connectedOrigin;
    string previousdata;

    Dictionary<string, string> savedAnimations;
    int animationNumber;

    public ConnectedEntity(string data)
        : base(0)
    {
        savedAnimations = new Dictionary<string, string>();
        animationNumber = 0;
        previousdata = "";
        ReceiveData(data);
        connectedOrigin = Vector2.Zero;
    }

    public override void SendData(string data = "") { }

    public void ReceiveData(string data)
    {
        if (data == previousdata)
        {
            return;
        }
        previousdata = data;

        string[] splitdata = data.Split(' ');

        connectedid = splitdata[1];
        if (splitdata[2] == "remove")
        {
            RemoveSelf();
            return;
        }

        position = new Vector2(float.Parse(splitdata[2]), float.Parse(splitdata[3]));
        connectedOrigin = new Vector2(float.Parse(splitdata[4]), float.Parse(splitdata[5]));
        origin = connectedOrigin;
        if (Current != null)
        {
            if (splitdata[6] != Current.AssetName)
            {
                if (savedAnimations.ContainsKey(splitdata[6]))
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
            NewAnimation(splitdata[8], splitdata[9], splitdata[10]);
        }
        if (drawHost == "" && GameWorld != null)
        {
            NewHost();
        }
    }

    private void SwitchAnimation(string animation)
    {
        PlayAnimation(savedAnimations[animation]);
    }

    private void NewAnimation(string animation, string islooping, string isbackandforth)
    {
        bool isLooping = bool.Parse(islooping);
        bool isBackAndForth = bool.Parse(isbackandforth);
        savedAnimations.Add(animation, animationNumber.ToString());
        animationNumber++;
        LoadAnimation(animation, savedAnimations[animation], isLooping, isBackAndForth);
        SwitchAnimation(animation);
    }

    protected override void SetAnimationData()
    {
        origin = connectedOrigin;
    }

    protected override void HandleCollisions() { }

    public override void RemoveSelf()
    {
        (GameWorld as Level).RemoveConnectedEntity(connectedid);
        base.RemoveSelf();
    }
}