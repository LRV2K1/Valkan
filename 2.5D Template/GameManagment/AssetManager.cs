using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;

public class TestSpriteExeption: Exception
{
    public TestSpriteExeption()
        : base()
    {
        Console.WriteLine("Can't find the test sprite");
    }
}

public class AssetManager
{
    protected ContentManager contentManager;
    protected Dictionary<string, Texture2D> textures;
    string spr_test;

    public AssetManager(ContentManager content)
    {
        contentManager = content;
        textures = new Dictionary<string, Texture2D>();

        spr_test = "Sprites/spr_test_1";
    }

    public Texture2D GetSprite(string assetName)
    {
        if (assetName == "")
        {
            return null;
        }
        else if (textures.ContainsKey(assetName))
        {
            return textures[assetName];
        }

        Texture2D tex = contentManager.Load<Texture2D>(assetName);
        textures.Add(assetName, tex);
        return tex;
    }

    public void PlayPartySound(string assetName)
    {
        if (assetName == "")
        {
            return;
        }
        try
        {
            SoundEffect snd = contentManager.Load<SoundEffect>(assetName);
            snd.Play();
            if (MultiplayerManager.Online)
            {
                MultiplayerManager.Party.Send("Sound: " + assetName, MultiplayerManager.PartyPort, false);
            }
        }
        catch
        {

        }
    }

    public void PlaySound(string assetName)
    {
        if (assetName == "")
        {
            return;
        }
        try
        {
            SoundEffect snd = contentManager.Load<SoundEffect>(assetName);
            snd.Play();
        }
        catch
        {

        }
    }

    public void PlayMusic(string assetName, bool repeat = true)
    {
        MediaPlayer.IsRepeating = repeat;
        MediaPlayer.Play(contentManager.Load<Song>(assetName));
    }

    public ContentManager Content
    {
        get { return contentManager; }
    }

    public string TestSprite
    {
        get { return spr_test; }
    }
}