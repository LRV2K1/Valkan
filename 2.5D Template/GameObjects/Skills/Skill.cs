﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


class Skill : GameObject
{

    protected SkillTimer timer;
    int skill;


    //generic skill class
    public Skill(string assetname, int skill)
        : base()
    {
        this.skill = skill;
        timer = new SkillTimer(assetname);
    }

    //setup skill
    public void Setup()
    {
        GameWorld.Add(this);
        OverlayManager overlay = GameWorld.GetObject("overlay") as OverlayManager;
        Overlay hud = overlay.GetOverlay("hud") as Overlay;

        //add timer to the hud overlay
        hud.Add(timer);
    }

    public virtual void Button(bool button)
    {

    }

    protected List<Player> SurroundingPlayers(Vector2 position, float range)
    {
        List<Player> surroundingplayers = new List<Player>();
        List<string> players = (GameWorld as Level).PlayerList;
        foreach (string id in players)
        {
            Player player = GameWorld.GetObject(id) as Player;
            float distance = Vector2.Distance(position, player.GlobalPosition);
            if (distance <= range && !player.Dead)
            {
                surroundingplayers.Add(player);
            }
        }
        return surroundingplayers;
    }

    protected int GetSpriteDirection()
    {
        int dir;
        Player player = parent as Player;
        double direction = player.Direction;
        if (player.Selected)
        {
            Selected icon = GameWorld.GetObject("selected") as Selected;
            float dx = icon.Position.X - player.Position.X;
            float dy = icon.Position.Y - player.Position.Y;
            direction = Math.Atan2(dy, dx);
        }

        dir = (int)((direction + (Math.PI / 8) + (3 * Math.PI / 2)) / (Math.PI / 4));
        if (dir > 7)
        {
            dir -= 8;
        }

        return dir;
    }

    protected void MakeParticle(Vector2 position, string asset)
    {
        if (asset != "")
        {
            ParticleEffect particleEffect = new ParticleEffect(asset);
            particleEffect.Position = position;
            (GameWorld.GetObject("items") as GameObjectList).Add(particleEffect);
            particleEffect.Reset();
        }
    }

    public virtual void Use(float timer = 2f)
    {
        this.timer.Use(timer);
        Player player = parent as Player;
        if (MultiplayerManager.Online && !player.Host)
        {
            MultiplayerManager.Party.Send("CPlayer: " + player.Id + " skill" + skill + " " + timer, MultiplayerManager.PartyPort, false);
        }
    }

    public SkillTimer Timer
    {
        get { return timer; }
    }

    public virtual bool Ready
    {
        get { return timer.Ready; }
    }
}

